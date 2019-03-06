using System;
using System.ComponentModel.DataAnnotations;

namespace Snow.AuthorityManagement.Core.Dto
{
    public class PagedAndSortedInputDto
    {
        public PagedAndSortedInputDto()
        {
            if (String.IsNullOrEmpty(Sorting))
            {
                Sorting = "ID";
            }
        }

        public string Sorting { get; set; }

        /// <summary>
        /// 跳过多少条
        /// </summary>
        [Range(0, int.MaxValue)]
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        [Range(1, 500)]
        public int PageSize { get; set; }
    }
}