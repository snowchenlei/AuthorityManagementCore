using System;
using System.Collections.Generic;
using System.Text;
using Anc.Domain.Uow;
using Microsoft.AspNetCore.Routing;

namespace Anc.AspNetCore.AspNetCore.Configuration
{
    public class AbpAspNetCoreConfiguration : IAncAspNetCoreConfiguration
    {
        public UnitOfWorkAttribute DefaultUnitOfWorkAttribute { get; }

        public List<Type> FormBodyBindingIgnoredTypes { get; }

        public bool IsValidationEnabledForControllers { get; set; }

        public bool IsAuditingEnabled { get; set; }

        public bool SetNoCacheForAjaxResponses { get; set; }

        public bool UseMvcDateTimeFormatForAppServices { get; set; }

        public List<Action<IRouteBuilder>> RouteConfiguration { get; }

        public AbpAspNetCoreConfiguration()
        {
            DefaultUnitOfWorkAttribute = new UnitOfWorkAttribute();
            FormBodyBindingIgnoredTypes = new List<Type>();
            RouteConfiguration = new List<Action<IRouteBuilder>>();
            IsValidationEnabledForControllers = true;
            SetNoCacheForAjaxResponses = true;
            IsAuditingEnabled = true;
            UseMvcDateTimeFormatForAppServices = false;
        }
    }
}