using PM.Common.CommonModels;
using PM.Domain.Cities;
using PM.Domain.Interfaces.Repository;
using PM.Domain.People;
using PM.Infrastructure.EF.Context;
using System.Threading.Tasks;

namespace PM.Infrastructure.EF.UnitOfWork
{
    class UnitOfWork : IUnitOfWork
    {
        private readonly PMContext _context;

        public PMContext Context
        {
            get { return _context; }
        }

        public UnitOfWork(PMContext context, IPeopleRepository peopleRepository, ICitiesRepository citiesRepository)
        {
            _context = context;
            People = peopleRepository;
            Cities = citiesRepository;
        }

        public IRepository<Person> People { get; private set; }
        public IRepository<City> Cities { get; private set; }

        public void Dispose()
        {
            Context?.Dispose();
            People?.Dispose();
            Cities?.Dispose();
        }

        public async Task<Result<int>> Save()
        {
            var result = await _context.SaveChangesAsync();
            return Result<int>.GetSuccessInstance(result);
        }
    }
}
