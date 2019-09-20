using Anc.Application.Navigation;
using Anc.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Core.Authorization.Menus.DomainService
{
    public interface IMenuManager : IDomainService
    {
        Task<MenuDefinition> CreateMenuDefinitionAsync();
    }
}