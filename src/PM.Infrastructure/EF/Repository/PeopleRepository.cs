using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PM.Common.CommonModels;
using PM.Domain.Interfaces.Repository;
using PM.Domain.People;
using PM.Infrastructure.EF.Context;
using PM.Infrastructure.EF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM.Infrastructure.Repository
{
    public class PeopleRepository : Repository<Person, PersonEntity>, IPeopleRepository
    {
        public PeopleRepository(PMContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override async Task<Person> GetAsync(int ID)
        {
            var res = await Context
                .Set<PersonEntity>()
                .Include(p => p.City)
                .FirstOrDefaultAsync(p => p.ID == ID);

            var entity = res == null || res.IsDeleted ? null : res;
            return mapper.Map<Person>(entity);
        }

        public override Result Update(int id, Person newEntity)
        {
            var old = Context.Set<PersonEntity>()
                .Include(p => p.Relations)
                .FirstOrDefault(p => p.ID == id);

            old.BirthDate = newEntity.BirthDate;
            old.CityID = newEntity.CityID;
            old.FirstName = newEntity.FirstName;
            old.Gender = newEntity.Gender;
            old.LastName = newEntity.LastName;
            old.PersonalNumber = newEntity.PersonalNumber;
            old.PhoneNumber = newEntity.PhoneNumber.Number.Value;
            old.PhoneNumberType = newEntity.PhoneNumber.PhoneNumberType;
            old.LastUpdateDate = DateTime.Now;

            UpdateRelations(id, newEntity, old);

            Context.SaveChanges();
            return Result.GetSuccessInstance();
        }

        public override async Task<Result> UpdateAsync(int id, Person newEntity)
        {
            var old = Context.Set<PersonEntity>()
                .Include(p => p.Relations)
                .FirstOrDefault(p => p.ID == id);

            old.BirthDate = newEntity.BirthDate;
            old.CityID = newEntity.CityID;
            old.FirstName = newEntity.FirstName;
            old.Gender = newEntity.Gender;
            old.LastName = newEntity.LastName;
            old.PersonalNumber = newEntity.PersonalNumber;
            old.PhoneNumber = newEntity.PhoneNumber.Number.Value;
            old.PhoneNumberType = newEntity.PhoneNumber.PhoneNumberType;
            old.ImageUrl = newEntity.ImageUrl;
            old.LastUpdateDate = DateTime.Now;

            UpdateRelations(id, newEntity, old);

            await Context.SaveChangesAsync();
            return Result.GetSuccessInstance();
        }

        private void UpdateRelations(int id, Person newEntity, PersonEntity old)
        {
            if (old.Relations == null)
                old.Relations = new List<PeopleRelationEntity>();

            var permRelations = new List<RelatedPerson>(newEntity.RelatedPeople);
            for (int i = 0; i < old.Relations.Count; i++)
            {
                var r = old.Relations[i];
                var res = permRelations.FirstOrDefault(rp => rp.ID == r.RelatedPersonID && rp.RelationType != r.RelationType);
                if (res == null)
                {
                    old.Relations.RemoveAt(i);
                    i--;
                }
                else
                    permRelations.Remove(res);
            }

            foreach (var r in permRelations)
            {
                old.Relations.Add(new PeopleRelationEntity { PersonID = id, RelatedPersonID = r.ID, RelationType = r.RelationType });
            }
        }

        public ICollection<Person> Filter(string searchWord, int skip, int take, string sortingColumn)
        {
            IQueryable<PersonEntity> entities = from p in Context.Set<PersonEntity>()
                                                where !p.IsDeleted
                                                orderby sortingColumn descending
                                                select p;

            if (!string.IsNullOrEmpty(searchWord))
            {

                entities = from p in entities
                           where p.FirstName.Contains(searchWord)
                           || p.LastName.Contains(searchWord)
                           || p.PersonalNumber.Contains(searchWord)
                           select p;
            }

            return mapper.Map<List<Person>>(entities.ToList());
        }

        public async Task<ICollection<Person>> FilterAsync(string searchWord, int skip, int take, string sortingColumn)
        {
            IQueryable<PersonEntity> entities = from p in Context.Set<PersonEntity>().Include(p=>p.Relations)
                                                where !p.IsDeleted
                                                orderby sortingColumn descending
                                                select p;

            if (!string.IsNullOrEmpty(searchWord))
            {

                entities = from p in entities
                           where p.FirstName.Contains(searchWord)
                           || p.LastName.Contains(searchWord)
                           || p.PersonalNumber.Contains(searchWord)
                           select p;
            }

            var result = await entities
                                .Skip(skip)
                                .Take(take)
                                .ToListAsync();

            return mapper.Map<List<Person>>(result);
        }

        public int FilterCount(string searchWord)
        {
            var entities = from p in Context.Set<PersonEntity>()
                           where !p.IsDeleted
                           select p;

            if (!string.IsNullOrEmpty(searchWord))
            {

                entities = from p in entities
                           where p.FirstName.Contains(searchWord)
                           || p.LastName.Contains(searchWord)
                           || p.PersonalNumber.Contains(searchWord)
                           select p;
            }

            return entities.Count();
        }

        public async Task<int> FilterCountAsync(string searchWord)
        {
            var entities = from p in Context.Set<PersonEntity>()
                           where !p.IsDeleted
                           select p;

            if (!string.IsNullOrEmpty(searchWord))
            {

                entities = from p in entities
                           where p.FirstName.Contains(searchWord)
                           || p.LastName.Contains(searchWord)
                           || p.PersonalNumber.Contains(searchWord)
                           select p;
            }

            return await entities.CountAsync();
        }

        public async Task<IEnumerable<RelatedPerson>> GetRelationPeople(int id)
        {
            var result = from rp in Context.Set<PeopleRelationEntity>()
                         let p = rp.RelatedPerson
                         where !p.IsDeleted
                         select new RelatedPerson
                         {
                             BirthDate = p.BirthDate,
                             City = p.City == null ? null : p.City.Name,
                             CityID = p.CityID,
                             RelationType = rp.RelationType,
                             FirstName = p.FirstName,
                             Gender = p.Gender,
                             ImageUrl = p.ImageUrl,
                             LastName = p.LastName,
                             ID = p.ID,
                             PersonalNumber = p.PersonalNumber,
                             PhoneNumber = p.PhoneNumber,
                             PhoneNumberType = p.PhoneNumberType,
                             RelationID = rp.ID
                         };

            return await result.ToListAsync();
        }

        public async Task<ICollection<Person>> FilterInDetailsAsync(PeopleFilter filter, int skip, int take, string sortingColumn)
        {
            IQueryable<PersonEntity> entities = from p in Context.Set<PersonEntity>()
                                                where !p.IsDeleted
                                                orderby sortingColumn descending
                                                select p;

            if (!string.IsNullOrEmpty(filter.FirstName))
                entities = entities.Where(p => p.FirstName == filter.FirstName);

            if (!string.IsNullOrEmpty(filter.LastName))
                entities = entities.Where(p => p.LastName == filter.LastName);

            if (!string.IsNullOrEmpty(filter.PersonalNumber))
                entities = entities.Where(p => p.PersonalNumber == filter.PersonalNumber);

            if (!string.IsNullOrEmpty(filter.PhoneNumber))
                entities = entities.Where(p => p.PhoneNumber == filter.PhoneNumber);

            if(filter.ID != null)
                entities = entities.Where(p => p.ID == filter.ID);

            var result = await entities
                                .Skip(skip)
                                .Take(take)
                                .ToListAsync();

            return mapper.Map<List<Person>>(result);
        }

        public async Task<int> FilterDetailsCountAsync(PeopleFilter filter)
        {
            IQueryable<PersonEntity> entities = from p in Context.Set<PersonEntity>()
                                                where !p.IsDeleted
                                                select p;

            if (!string.IsNullOrEmpty(filter.FirstName))
                entities = entities.Where(p => p.FirstName == filter.FirstName);

            if (!string.IsNullOrEmpty(filter.LastName))
                entities = entities.Where(p => p.LastName == filter.LastName);

            if (!string.IsNullOrEmpty(filter.PersonalNumber))
                entities = entities.Where(p => p.PersonalNumber == filter.PersonalNumber);

            if (!string.IsNullOrEmpty(filter.PhoneNumber))
                entities = entities.Where(p => p.PhoneNumber == filter.PhoneNumber);

            if (filter.ID != null)
                entities = entities.Where(p => p.ID == filter.ID);

            return await entities.CountAsync();
        }
    }
}
