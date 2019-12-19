using Anc.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anc.EntityFrameworkCore.Uow
{
    public class EfCoreUnitOfWork<TDbContext> : UnitOfWorkBase where TDbContext : DbContext
    {
        private readonly DbContext _context;

        public EfCoreUnitOfWork(TDbContext context)
        {
            _context = context;
        }

        protected override IUnitOfWork BeginUow()
        {
            return this;
        }

        protected override void CompleteUow()
        {
            SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        protected override Task CompleteUowAsync()
        {
            return _context.SaveChangesAsync();
        }

        protected override void DisposeUow()
        {
            //if (Options.IsTransactional == true)
            //{
            //    _transactionStrategy.Dispose(IocResolver);
            //}
            //else
            //{
            //    foreach (var context in GetAllActiveDbContexts())
            //    {
            //        Release(context);
            //    }
            //}

            //ActiveDbContexts.Clear();
        }

        protected override void RollbackUow()
        {
        }
    }
}