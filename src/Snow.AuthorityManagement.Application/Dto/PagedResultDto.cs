using System.Collections.Generic;

namespace Snow.AuthorityManagement.Application.Dto
{
    public class PagedResultDto<T>
    {
        public int TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}