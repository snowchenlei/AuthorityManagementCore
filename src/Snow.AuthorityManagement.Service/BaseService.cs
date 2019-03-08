﻿using Snow.AuthorityManagement.Data;
using Snow.AuthorityManagement.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Service
{
    public class BaseService<T> where T : class, new()
    {
        #region 获取具体的操作类的实例

        protected readonly DbContext CurrentContext = null;
        protected readonly IBaseRepository<T> CurrentRepository = null;

        public BaseService(AuthorityManagementContext context, IBaseRepository<T> currentRepository)
        {
            CurrentRepository = currentRepository;
            CurrentContext = context;
        }

        #endregion 获取具体的操作类的实例

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> whereLamada)
        {
            return await CurrentRepository.FirstOrDefaultAsync(whereLamada);
        }

        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLamada)
        {
            return CurrentRepository.LoadEntities(whereLamada);
        }
    }
}