using Anc.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Anc.Domain.Uow
{
    public class UnitOfWorkManager : IUnitOfWorkManager, ITransientDependency
    {
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
            return null;
        }
    }
}