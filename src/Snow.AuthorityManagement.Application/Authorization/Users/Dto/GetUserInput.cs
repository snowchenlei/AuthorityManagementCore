using System;
using Snow.AuthorityManagement.Application.Dto;

namespace Snow.AuthorityManagement.Application.Authorization.Users.Dto
{
    public class GetUserInput : PagedAndSortedInputDto
    {
        public string UserName { get; set; }
        public string Date { get; set; }
    }
}