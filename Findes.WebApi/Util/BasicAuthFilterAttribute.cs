using Findes.Standard.Core.Util;
using Findes.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;

namespace Findes.WebApi.Util
{
    internal class BasicAuthFilterAttribute : ActionFilterAttribute
    {
        private StringValues authorizationToken;
        IOptions<AppSettings> AppSettings;

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            AppSettings = ((NacionalController)actionContext.Controller).AppSettings;

            authorizationToken = String.Empty;
            actionContext.HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);

            if (String.IsNullOrEmpty(authorizationToken)
                || authorizationToken.ToString() != AppSettings.Value.token)
            {
                actionContext.Result = new ObjectResult(null)
                {
                    StatusCode = 401,
                };
            }
        }
    }
}