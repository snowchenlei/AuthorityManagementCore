using System.ComponentModel.DataAnnotations;

namespace Anc.Application.Services.Dto
{
    public class PagedInputDto
    {
        /// <summary>
        /// 每页记录数
        /// </summary>
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 页索引
        /// </summary>
        [Range(0, int.MaxValue)]
        public int PageIndex { get; set; }
    }
}