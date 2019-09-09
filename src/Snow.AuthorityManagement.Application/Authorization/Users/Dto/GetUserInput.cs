using System;
using Anc.Application.Services.Dto;
using Snow.AuthorityManagement.Application.Dto;

namespace Snow.AuthorityManagement.Application.Authorization.Users.Dto
{
    public class GetUserInput : PagedAndSortedInputDto
    {
        /// <summary>
        /// 构造
        /// </summary>
        public GetUserInput()
        {
            Sorting = "ID";
        }

        public string UserName { get; set; }
        public string Date { get; set; }
    }
}