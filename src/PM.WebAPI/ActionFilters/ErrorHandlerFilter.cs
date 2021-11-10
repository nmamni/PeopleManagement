using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using PM.Common.CommonModels;
using PM.Common.Exceptions;
using PM.Infrastructure.SharedResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PM.WebAPI.ActionFilters
{
    public class ErrorHandlerFilter : IAsyncActionFilter
    {
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        public ErrorHandlerFilter(IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _sharedLocalizer = sharedLocalizer;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();
            if (result.Exception != null)
            {
                string errorText = string.Empty;
                if (result.Exception is LocalizableException)
                {
                    var ex = result.Exception as LocalizableException;
                    errorText = _sharedLocalizer[ex.MessageKey]?.Value;
                }
                else
                    errorText = _sharedLocalizer["unexpected"]?.Value;

                result.ExceptionHandled = true;
                Result resultObj = new Result(-1, false, errorText);
                result.Result = new BadRequestObjectResult(resultObj);
            }
        }
    }
}
