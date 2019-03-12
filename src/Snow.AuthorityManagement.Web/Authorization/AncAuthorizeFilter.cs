using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Snow.AuthorityManagement.Core.Exception;
using Snow.AuthorityManagement.Web.Extensions;
using Snow.AuthorityManagement.Web.Results;

namespace Snow.AuthorityManagement.Web.Authorization
{
    public class AncAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly ILogger<AncAuthorizeFilter> _logger;
        private readonly AuthorizationHelper _authorizationHelper;

        public AncAuthorizeFilter(ILogger<AncAuthorizeFilter> logger, AuthorizationHelper authorizationHelper)
        {
            _logger = logger;
            _authorizationHelper = authorizationHelper;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }

            if (!context.ActionDescriptor.IsControllerAction())
            {
                return;
            }

            //TODO: Avoid using try/catch, use conditional checking
            try
            {
                await _authorizationHelper.CheckPermissionsAsync(
                    context.ActionDescriptor.GetMethodInfo(),
                    context.ActionDescriptor.GetMethodInfo().DeclaringType);
            }
            catch (AncAuthorizationException ex)
            {
                _logger.LogWarning(ex.ToString(), ex);

                //_eventBus.Trigger(this, new AbpHandledExceptionData(ex));

                if (ActionResultHelper.IsObjectResult(context.ActionDescriptor.GetMethodInfo().ReturnType))
                {
                    context.Result = new ObjectResult(new
                    {
                        Error = ex,
                        UnAuthorizedRequest = true,
                        Success = false,
                    })
                    {
                        StatusCode = context.HttpContext.User.Identity.IsAuthenticated
                            ? (int)System.Net.HttpStatusCode.Forbidden
                            : (int)System.Net.HttpStatusCode.Unauthorized
                    };
                }
                else
                {
                    context.Result = new ChallengeResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);

                if (ActionResultHelper.IsObjectResult(context.ActionDescriptor.GetMethodInfo().ReturnType))
                {
                    context.Result = new ObjectResult(new
                    {
                        Error = ex,
                        UnAuthorizedRequest = false,
                        Success = false,
                    })
                    {
                        StatusCode = (int)System.Net.HttpStatusCode.InternalServerError
                    };
                }
                else
                {
                    //TODO: How to return Error page?
                    context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.InternalServerError);
                }
            }
        }
    }
}