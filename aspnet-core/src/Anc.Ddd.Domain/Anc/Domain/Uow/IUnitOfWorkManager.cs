using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Anc.Domain.Uow
{
    public interface IUnitOfWorkManager
    {
        IUnitOfWork Begin();

        IUnitOfWork Begin(TransactionScopeOption scope);

        IUnitOfWork Begin(UnitOfWorkOptions options);
    }
}