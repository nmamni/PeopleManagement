using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PM.Common.CommonModels;
using PM.Domain.BaseClasses;
using PM.Domain.Interfaces.Repository;
using PM.Infrastructure.EF.Context;
using PM.Infrastructure.EF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PM.Infrastructure.Repository
{
    public abstract class Repository<TDomainEntity, TDBEntity> : IRepository<TDomainEntity> where TDomainEntity : BaseEntity where TDBEntity : TEntity
    {
        protected readonly PMContext Context;
        protected readonly IMapper mapper;

        public Repository(PMContext context, IMapper mapper)
        {
            Context = context;
            this.mapper = mapper;
        }

        public virtual Result<int> Add(TDomainEntity entity)
        {
            var dbEntity = mapper.Map<TDBEntity>(entity);
            Context.Set<TDBEntity>().Add(dbEntity);
            Context.SaveChanges();
            return Result<int>.GetSuccessInstance(entity.ID);
        }

        public virtual async Task<Result<int>> AddAsync(TDomainEntity entity)
        {
            var dbEntity = mapper.Map<TDBEntity>(entity);
            Context.Set<TDBEntity>().Add(dbEntity);
            await Context.SaveChangesAsync();
            return Result<int>.GetSuccessInstance(entity.ID);
        }

        public virtual Result<IEnumerable<int>> AddRange(IEnumerable<TDomainEntity> entities)
        {
            var dbEntities = mapper.Map<IEnumerable<TDBEntity>>(entities);
            Context.Set<TDBEntity>().AddRange(dbEntities);
            Context.SaveChanges();
            return Result<IEnumerable<int>>
                .GetSuccessInstance(dbEntities.Select(e => e.ID));
        }

        public virtual async Task<Result<IEnumerable<int>>> AddRangeAsync(IEnumerable<TDomainEntity> entities)
        {
            var dbEntities = mapper.Map<IEnumerable<TDBEntity>>(entities);
            Context.Set<TDBEntity>().AddRange(dbEntities);
            await Context.SaveChangesAsync();
            return Result<IEnumerable<int>>
                .GetSuccessInstance(dbEntities.Select(e => e.ID));
        }

        public virtual TDomainEntity Get(int ID)
        {
            var res = Context.Set<TDBEntity>().Find(ID);
            var entity = res.IsDeleted ? null : res;
            return mapper.Map<TDomainEntity>(entity);
        }

        public virtual async Task<TDomainEntity> GetAsync(int ID)
        {
            var res = await Context.Set<TDBEntity>().FindAsync(ID);
            var entity = res == null || res.IsDeleted ? null : res;
            return mapper.Map<TDomainEntity>(entity);
        }

        public virtual ICollection<TDomainEntity> GetAll()
        {
            var entities = Context
                .Set<TDBEntity>()
                .Where(t => !t.IsDeleted);

            return mapper.Map<List<TDomainEntity>>(entities);
        }

        public virtual async Task<ICollection<TDomainEntity>> GetAllAsync()
        {
            var entities = await Context
                                    .Set<TDBEntity>()
                                    .Where(t => !t.IsDeleted)
                                    .ToListAsync();

            return mapper.Map<List<TDomainEntity>>(entities);
        }

        public virtual Result Remove(int entityID)
        {
            var entity = Context
                .Set<TDBEntity>()
                .Find(entityID);

            if (entity == null)
                return new Result(-1, false, "Item does not exist");

            entity.IsDeleted = true;
            entity.DeleteDate = DateTime.Now;
            Context.SaveChanges();
            return Result.GetSuccessInstance();
        }

        public virtual async Task<Result> RemoveAsync(int entityID)
        {
            var entity = Context
                .Set<TDBEntity>()
                .Find(entityID);

            if (entity == null)
                return new Result(-1, false, "Item does not exist");

            entity.IsDeleted = true;
            entity.DeleteDate = DateTime.Now;
            await Context.SaveChangesAsync();
            return Result.GetSuccessInstance();
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public abstract Result Update(int id, TDomainEntity entity);
        public abstract Task<Result> UpdateAsync(int id, TDomainEntity entity);

        public int Count()
        {
            return Context
                 .Set<TDBEntity>()
                 .Count();
        }

        public async Task<int> CountAsync()
        {
            return await Context
                  .Set<TDBEntity>()
                  .CountAsync();
        }
    }
}
