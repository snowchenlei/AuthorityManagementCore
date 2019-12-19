using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Anc.Application.Services.Dto;

namespace Anc.Application.Services.Dto
{
    /// <summary>
    /// Simply implements <see cref="IPagedResultRequest"/>.
    /// </summary>
    [Serializable]
    public class PagedResultRequestDto : LimitedResultRequestDto, IPagedResultRequest
    {
        [Range(0, int.MaxValue)]
        public virtual int SkipCount { get; set; }
    }
}