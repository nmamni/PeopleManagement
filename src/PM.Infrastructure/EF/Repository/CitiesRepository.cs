using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PM.Common.CommonModels;
using PM.Domain.Cities;
using PM.Domain.Interfaces.Repository;
using PM.Infrastructure.EF.Context;
using PM.Infrastructure.EF.Entities;
using PM.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM.Infrastructure.EF.Repository
{
    public class CitiesRepository : Repository<City, CityEntity>, ICitiesRepository
    {
        public CitiesRepository(PMContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public ICollection<City> Filter(string searchWord, int index, int showPerPage, string sortingColumn)
        {
            IQueryable<CityEntity> entities = from p in Context.Set<CityEntity>()
                                                where !p.IsDeleted
                                                orderby sortingColumn descending
                                                select p;

            if (!string.IsNullOrEmpty(searchWord))
            {

                entities = from p in entities
                           where p.Name.Contains(searchWord)
                           select p;
            }

            return mapper.Map<List<City>>(entities.ToList());
        }

        public async Task<ICollection<City>> FilterAsync(string searchWord, int index, int showPerPage, string sortingColumn)
        {
            IQueryable<CityEntity> entities = from p in Context.Set<CityEntity>()
                                              where !p.IsDeleted
                                              orderby sortingColumn descending
                                              select p;

            if (!string.IsNullOrEmpty(searchWord))
            {

                entities = from p in entities
                           where p.Name.Contains(searchWord)
                           select p;
            }

            return mapper.Map<List<City>>(await entities.ToListAsync());
        }

        public int FilterCount(string searchWord, int index, int showPerPage, string sortingColumn)
        {
            IQueryable<CityEntity> entities = from p in Context.Set<CityEntity>()
                                              where !p.IsDeleted
                                              orderby sortingColumn descending
                                              select p;

            if (!string.IsNullOrEmpty(searchWord))
            {

                entities = from p in entities
                           where p.Name.Contains(searchWord)
                           select p;
            }

            return entities.Count();
        }

        public async Task<int> FilterCountAsync(string searchWord, int index, int showPerPage, string sortingColumn)
        {
            IQueryable<CityEntity> entities = from p in Context.Set<CityEntity>()
                                              where !p.IsDeleted
                                              orderby sortingColumn descending
                                              select p;

            if (!string.IsNullOrEmpty(searchWord))
            {

                entities = from p in entities
                           where p.Name.Contains(searchWord)
                           select p;
            }

            return await entities.CountAsync();
        }

        public override Result Update(int id, City entity)
        {
            var old = Context.Set<CityEntity>().Find(id);
            old.Name = entity.Name;
            old.LastUpdateDate = DateTime.Now;
            Context.SaveChanges();
            return Result.GetSuccessInstance();
        }

        public override async Task<Result> UpdateAsync(int id, City entity)
        {
            var old = Context.Set<CityEntity>().Find(id);
            old.Name = entity.Name;
            old.LastUpdateDate = DateTime.Now;
            await Context.SaveChangesAsync();
            return Result.GetSuccessInstance();
        }
    }
}
