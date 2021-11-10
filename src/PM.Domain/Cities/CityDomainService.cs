using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PM.Common.CommonModels;
using PM.Domain.Interfaces.Repository;

namespace PM.Domain.Cities
{
    public class CityDomainService : ICityDomainService
    {
        private readonly ICitiesRepository _citiesRepository;

        public CityDomainService(ICitiesRepository citiesRepository)
        {
            _citiesRepository = citiesRepository;
        }

        public async Task<Result<int>> Create(City city)
        {
            return await _citiesRepository.AddAsync(city);
        }

        public async Task<Result> Delete(int id)
        {
            return await _citiesRepository.RemoveAsync(id);
        }

        public async Task<(IEnumerable<City>, int)> Filter(string filter, int index, int showPerPage, string sortingColumn)
        {
            var cities = await _citiesRepository.FilterAsync(filter, index, showPerPage, sortingColumn);
            var quantity = await _citiesRepository.FilterCountAsync(filter, index, showPerPage, sortingColumn);
            return (cities, quantity);
        }
    }
}
