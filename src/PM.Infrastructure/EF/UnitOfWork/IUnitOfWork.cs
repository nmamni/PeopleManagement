using PM.Common.CommonModels;
using PM.Domain.Cities;
using PM.Domain.Interfaces.Repository;
using PM.Domain.People;
using PM.Infrastructure.EF.Context;
using PM.Infrastructure.EF.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PM.Infrastructure.EF.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        PMContext Context { get; }
        IRepository<Person> People { get; }
        IRepository<City> Cities { get; }
    }
}
