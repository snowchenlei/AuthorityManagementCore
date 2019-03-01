using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cl.AuthorityManagement.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cl.AuthorityManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        protected IOrderedQueryable<T> Sort<T, S>(IQueryable<T> resource, Expression<Func<T, S>> orderbyLamada, OrderType orderType)
        {
            IOrderedQueryable<T> result = null;
            switch (orderType)
            {
                default:
                case OrderType.ASC:
                    result = resource.OrderBy(orderbyLamada);
                    break;
                case OrderType.DESC:
                    result = resource.OrderByDescending(orderbyLamada);
                    break;
            }
            return result;
        }
    }
}