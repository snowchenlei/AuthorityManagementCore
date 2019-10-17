using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anc.DependencyInjection;
using Anc.Threading;

namespace Anc.Auditing
{
    internal class AuditingManager : IAuditingManager, ITransientDependency
    {
        private const string AmbientContextKey = "Volo.Abp.Auditing.IAuditLogScope";

        protected IServiceProvider ServiceProvider { get; }
        private readonly IAuditingHelper _auditingHelper;
        private readonly IAuditingStore _auditingStore;
        private readonly IAmbientScopeProvider<AuditLogInfo> _ambientScopeProvider;

        public AuditingManager(
            IAmbientScopeProvider<AuditLogInfo> ambientScopeProvider,
            IAuditingStore auditingStore,
            IAuditingHelper auditingHelper,
            IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

            _ambientScopeProvider = ambientScopeProvider;
            _auditingHelper = auditingHelper;
            _auditingStore = auditingStore;
        }

        public AuditLogInfo Current => _ambientScopeProvider.GetValue(AmbientContextKey);

        public IAuditLogSaveHandle BeginScope()
        {
            var ambientScope = _ambientScopeProvider.BeginScope(
                AmbientContextKey,
                _auditingHelper.CreateAuditLogInfo()
            );

            Debug.Assert(Current != null, "Current != null");

            return new DisposableSaveHandle(this, ambientScope, Current, Stopwatch.StartNew());
        }

        protected virtual void BeforeSave(DisposableSaveHandle saveHandle)
        {
            saveHandle.StopWatch.Stop();
            saveHandle.AuditLog.ExecutionDuration = Convert.ToInt32(saveHandle.StopWatch.Elapsed.TotalMilliseconds);
        }

        protected virtual async Task SaveAsync(DisposableSaveHandle saveHandle)
        {
            BeforeSave(saveHandle);

            if (ShouldSave(saveHandle.AuditLog))
            {
                await _auditingStore.SaveAsync(saveHandle.AuditLog);
            }
        }

        protected virtual void Save(DisposableSaveHandle saveHandle)
        {
            BeforeSave(saveHandle);

            if (ShouldSave(saveHandle.AuditLog))
            {
                _auditingStore.Save(saveHandle.AuditLog);
            }
        }

        protected bool ShouldSave(AuditLogInfo auditLog)
        {
            if (!auditLog.Actions.Any())
            {
                return false;
            }

            return true;
        }

        protected class DisposableSaveHandle : IAuditLogSaveHandle
        {
            public AuditLogInfo AuditLog { get; }
            public Stopwatch StopWatch { get; }

            private readonly AuditingManager _auditingManager;
            private readonly IDisposable _scope;

            public DisposableSaveHandle(
                AuditingManager auditingManager,
                IDisposable scope,
                AuditLogInfo auditLog,
                Stopwatch stopWatch)
            {
                _auditingManager = auditingManager;
                _scope = scope;
                AuditLog = auditLog;
                StopWatch = stopWatch;
            }

            public async Task SaveAsync()
            {
                await _auditingManager.SaveAsync(this);
            }

            public void Save()
            {
                _auditingManager.Save(this);
            }

            public void Dispose()
            {
                _scope.Dispose();
            }
        }
    }
}