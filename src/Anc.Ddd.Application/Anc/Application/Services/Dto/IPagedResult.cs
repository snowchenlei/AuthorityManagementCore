using System;
using System.Collections.Generic;
using System.Text;
using Anc.Application.Services.Dto;

namespace Anc.Application.Services.Dto
{
    /// <summary>
    /// This interface is defined to standardize to return a page of items to clients.
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="P:Abp.Application.Services.Dto.IListResult`1.Items" /> list</typeparam>
    public interface IPagedResult<T> : IListResult<T>, IHasTotalCount
    {
    }
}