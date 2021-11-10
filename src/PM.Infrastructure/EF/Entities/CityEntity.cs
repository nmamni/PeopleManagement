using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Infrastructure.EF.Entities
{
    public class CityEntity : TEntity
    {
        public string Name { get; set; }
        public virtual List<PersonEntity> People { get; set; }
    }
}
