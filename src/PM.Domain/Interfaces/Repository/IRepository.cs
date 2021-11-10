using PM.Common.CommonModels;
using PM.Domain.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PM.Domain.Interfaces.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : BaseEntity
    {
        TEntity Get(int ID);
        Task<TEntity> GetAsync(int ID);

        ICollection<TEntity> GetAll();
        Task<ICollection<TEntity>> GetAllAsync();

        Result<int> Add(TEntity entity);
        Task<Result<int>> AddAsync(TEntity entity);
        Result<IEnumerable<int>> AddRange(IEnumerable<TEntity> entities);
        Task<Result<IEnumerable<int>>> AddRangeAsync(IEnumerable<TEntity> entities);

        Result Update(int id, TEntity entity);
        Task<Result> UpdateAsync(int id, TEntity entity);

        Result Remove(int entityID);
        Task<Result> RemoveAsync(int entityID);

        int Count();
        Task<int> CountAsync();
    }
}
