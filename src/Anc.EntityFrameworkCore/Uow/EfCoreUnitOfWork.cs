using Anc.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Anc.EntityFrameworkCore.Uow
{
    public class EfCoreUnitOfWork : UnitOfWorkBase
    {
        private readonly DbContext _context;

        public EfCoreUnitOfWork(DbContext context)
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

        public override void Dispose()
        {
        }

        protected override void RollbackUow()
        {
        }
    }
}