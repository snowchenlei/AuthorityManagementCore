using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Application.Services.Dto
{
    public class PagedAndSortedInputDto : PagedInputDto, ISortedResultRequest
    {
        /// <summary>
        /// 排序
        /// </summary>
        public string Sorting { get; set; }

        public OrderType Order { get; set; }
    }
}