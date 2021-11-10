using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PM.Domain.Interfaces.Repository;
using PM.Infrastructure.EF.Context;
using PM.Infrastructure.EF.Repository;
using PM.Infrastructure.EF.UnitOfWork;
using PM.Infrastructure.FileSytem;
using PM.Infrastructure.Mapper;
using PM.Infrastructure.Repository;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PM.Infrastructure
{
    public static class InfrastructureServiceCOnfigurationExtension
    {
        public static void RegisterInfrastructure(this IServiceCollection services, IConfiguration conf)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PMContext>();
            optionsBuilder.UseSqlServer(conf.GetConnectionString("DefaultConnection"));

            services.AddDbContext<PMContext>(options => options.UseSqlServer(conf["ConnectionStrings:DefaultConnection"]));
            //services.AddDbContext<PMContext>(options => options.UseInMemoryDatabase(databaseName: "PM"));

            using (var context = new PMContext(optionsBuilder.Options))
            {
                context.Database.Migrate();
            }

            services.AddAutoMapper(typeof(EFProfile).Assembly);
            services.AddScoped<IPeopleRepository, PeopleRepository>();
            services.AddScoped<ICitiesRepository, CitiesRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var uploadsPath = conf.GetSection("Filesystem")["uploads"];
            var logsPath = conf.GetSection("Filesystem")["logs"];
            services.AddScoped<IFileSystemClient>(t => new FileSystemClient(uploadsPath));


            services.AddSingleton<ILogger>(s=> new LoggerConfiguration()
                            .WriteTo.File(logsPath, rollingInterval: RollingInterval.Day)
                            .CreateLogger());

            services.AddLocalization(o => { o.ResourcesPath = "SharedResources/Resources"; });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                CultureInfo[] supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ka"),
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }
    }
}
