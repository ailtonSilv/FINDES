using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Findes.WebApi.Util
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = new JsonResult(new ApiErrorResult() { Message = context.Exception.Message });
            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            base.OnException(context);
        }

        public class ApiErrorResult
        {
            public string Message { get; set; }
        }
    }
}
