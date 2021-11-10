using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PM.Common.CommonModels;
using PM.Domain.Cities;

namespace PM.Application.Cities
{
    public class CitiesApplication : ICitiesApplication
    {
        private readonly ICityDomainService _cityDomainService;
        public CitiesApplication(ICityDomainService cityDomainService)
        {
            _cityDomainService = cityDomainService;
        }
        public async Task<Result<int>> Create(CreateCityCommand cmd)
        {
            var city = new City(cmd.Name);
            return await _cityDomainService.Create(city);
        }

        public async Task<Result> Delete(int id)
        {
            return await _cityDomainService.Delete(id);
        }

        public async Task<FilterResponse<IEnumerable<CityListItem>>> Filter(FilterModel<string> fm)
        {
            var res = await _cityDomainService.Filter(fm.Filter,
                fm.PageRequest.Index,
                fm.PageRequest.ShowPerPage,
                fm.PageRequest.SortingColumn);

            var cities = res.Item1.Select(i => new CityListItem { ID = i.ID, Name = i.Name });
            return new FilterResponse<IEnumerable<CityListItem>>(cities, res.Item2);
        }
    }   
}
