using Anc.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Anc.Domain.Uow
{
    /// <summary>
    /// Unit of work manager. Used to begin and control a unit of work.
    /// </summary>
    public interface IUnitOfWork : IDisposable, ITransientDependency
    {
        /// <summary>
        /// Begins a new unit of work.
        /// </summary>
        /// <returns>A handle to be able to complete the unit of work</returns>
        IUnitOfWork Begin();

        /// <summary>
        /// Begins a new unit of work.
        /// </summary>
        /// <returns>A handle to be able to complete the unit of work</returns>
        IUnitOfWork Begin(TransactionScopeOption scope);

        /// <summary>
        /// Begins a new unit of work.
        /// </summary>
        /// <returns>A handle to be able to complete the unit of work</returns>
        IUnitOfWork Begin(UnitOfWorkOptions options);

        /// <summary>
        /// Completes this unit of work. It saves all changes and commit transaction if exists.
        /// </summary>
        void Commit();

        /// <summary>
        /// Completes this unit of work. It saves all changes and commit transaction if exists.
        /// </summary>
        Task CommitAsync();

        void Rollback();
    }
}