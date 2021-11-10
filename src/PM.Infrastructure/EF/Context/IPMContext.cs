using Microsoft.EntityFrameworkCore;
using PM.Infrastructure.EF.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Infrastructure.EF.Context
{
    public interface IPMContext : IDisposable
    {
        DbSet<PersonEntity> People { get;  }
        DbSet<PeopleRelationEntity> PeopleRelations { get; }
        DbSet<CityEntity> Cities { get;  }
    }
}
