using Anc.Domain.Uow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Anc.Dapper.Uow
{
    public class DapperUnitOfWork : UnitOfWorkBase
    {
        private IDbConnection _connection = null;
        private IDbTransaction _transaction = null;

        public DapperUnitOfWork(IDbConnection connection)
        {
            _connection = connection;
        }

        protected override IUnitOfWork BeginUow()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            _transaction = _connection.BeginTransaction();
            return this;
        }

        protected override void CompleteUow()
        {
            _transaction?.Commit();
        }

        protected override Task CompleteUowAsync()
        {
            CompleteUow();
            return Task.CompletedTask;
        }

        protected override void RollbackUow()
        {
            _transaction?.Rollback();
        }

        public override void Dispose()
        {
            _transaction?.Dispose();
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}