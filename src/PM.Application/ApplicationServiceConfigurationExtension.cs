//using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PM.Application.Cities;
using PM.Application.People;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Application
{
    public static class ApplicationServiceConfigurationExtension
    {
        public static void RegisterApplication(this IServiceCollection services)
        {
            services.AddScoped<IPeopleApplication, PeopleApplication>();
            services.AddScoped<ICitiesApplication, CitiesApplication>();
        }
    }
}
