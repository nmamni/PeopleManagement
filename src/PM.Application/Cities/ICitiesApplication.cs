using PM.Common.CommonModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PM.Application.Cities
{
    public interface ICitiesApplication
    {
        Task<Result<int>> Create(CreateCityCommand cmd);

        Task<FilterResponse<IEnumerable<CityListItem>>> Filter(FilterModel<string> filterModel);

        Task<Result> Delete(int id);
    }
}
