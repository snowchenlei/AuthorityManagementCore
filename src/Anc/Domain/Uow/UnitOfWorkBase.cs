using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Anc.Domain.Uow
{
    /// <summary>
    /// Unit of work manager.
    /// </summary>
    public abstract class UnitOfWork : IUnitOfWork
    {
        public UnitOfWorkOptions Options { get; private set; }

        /// <summary>
        /// Is this unit of work successfully completed.
        /// </summary>
        private bool _succeed;

        /// <summary>
        /// A reference to the exception if this unit of work failed.
        /// </summary>
        private Exception _exception;

        public void Begin()
        {
            Begin(new UnitOfWorkOptions());
        }

        public void Begin(TransactionScopeOption scope)
        {
            Begin(new UnitOfWorkOptions { Scope = scope });
        }

        public void Begin(UnitOfWorkOptions options)
        {
            Options = options;
            BeginUow();
        }

        public void Complete()
        {
            try
            {
                CompleteUow();
                _succeed = true;
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        public Task CompleteAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Can be implemented by derived classes to start UOW.
        /// </summary>
        protected abstract void BeginUow();

        /// <summary>
        /// Should be implemented by derived classes to complete UOW.
        /// </summary>
        protected abstract void CompleteUow();

        /// <summary>
        /// Should be implemented by derived classes to complete UOW.
        /// </summary>
        protected abstract Task CompleteUowAsync();
    }
}