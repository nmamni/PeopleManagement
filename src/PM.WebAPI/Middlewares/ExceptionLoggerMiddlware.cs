using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PM.WebAPI.Middlewares
{
    public static class ExceptionLoggerMiddlware
    {
        public static void ConfigureExceptionLoggerMiddlware(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(options =>
            {
                options.Run(
                    async context =>
                    {
                        var logger = (ILogger)options.ApplicationServices.GetService(typeof(ILogger));
                        var ex = context.Features.Get<IExceptionHandlerFeature>();
                        if (ex != null)
                            logger.Error(ex.Error, "global");
                    });
            });
        }
    }
}
