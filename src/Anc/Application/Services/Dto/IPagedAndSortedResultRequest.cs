using System;
using System.Collections.Generic;
using System.Text;
using Anc.Application.Services.Dto;

namespace Anc.Application.Services.Dto
{
    /// <summary>
    /// This interface is defined to standardize to request a paged and sorted result.
    /// </summary>
    public interface IPagedAndSortedResultRequest : IPagedResultRequest, ISortedResultRequest
    {
    }
}