using System;
using Snow.AuthorityManagement.Enum;
using OrderType = Snow.AuthorityManagement.Core.Enum.OrderType;

namespace Snow.AuthorityManagement.Core.Dto.User
{
    public class GetUserInput : PagedAndSortedInputDto
    {
        public string Sort { get; set; }
        public OrderType Order { get; set; }
        public string UserName { get; set; }
        public string Date { get; set; }

        public GetUserInput()
        {
            if (String.IsNullOrEmpty(Sorting))

            {
                Sorting = "ID";
            }
        }
    }
}