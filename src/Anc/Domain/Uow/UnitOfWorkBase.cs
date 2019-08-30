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
    public abstract class UnitOfWorkBase : IUnitOfWork
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

        public IUnitOfWork Begin()
        {
            return Begin(new UnitOfWorkOptions());
        }

        public IUnitOfWork Begin(TransactionScopeOption scope)
        {
            return Begin(new UnitOfWorkOptions { Scope = scope });
        }

        public IUnitOfWork Begin(UnitOfWorkOptions options)
        {
            Options = options;
            return BeginUow();
        }

        public void Commit()
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

        public Task CommitAsync()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            try
            {
                RollbackUow();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        /// <summary>
        /// Can be implemented by derived classes to start UOW.
        /// </summary>
        protected abstract IUnitOfWork BeginUow();

        /// <summary>
        /// Should be implemented by derived classes to complete UOW.
        /// </summary>
        protected abstract void CompleteUow();

        /// <summary>
        /// Should be implemented by derived classes to complete UOW.
        /// </summary>
        protected abstract Task CompleteUowAsync();

        protected abstract void RollbackUow();

        public abstract void Dispose();
    }
}