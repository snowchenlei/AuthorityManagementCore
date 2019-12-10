using Anc.Application.Services.Dto;
using System.Collections.Generic;

namespace Snow.AuthorityManagement.Application.Dto
{
    public class PagedResultDto<T> : ListResultDto<T>, IPagedResult<T>
    {
        public int TotalCount { get; set; }
        //public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// 页索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount => TotalCount / PageSize + (TotalCount % PageSize > 0 ? 1 : 0);

        /// <summary>
        /// 是否有前页
        /// </summary>
        public bool HasPrevious => PageIndex > 0 && PageCount > 0;

        /// <summary>
        /// 是否有后页
        /// </summary>
        public bool HasNext => PageIndex < PageCount - 1;

        public PagedResultDto()
        {
        }

        /// <summary>
        /// Creates a new <see cref="PagedResultDto{T}"/> object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="items">List of items in current page</param>
        public PagedResultDto(int pageIndex, int pageSize, int totalCount, IReadOnlyList<T> items)
            : base(items)
        {
            TotalCount = totalCount;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}