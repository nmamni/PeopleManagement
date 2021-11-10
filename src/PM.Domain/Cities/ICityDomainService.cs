using PM.Common.CommonModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PM.Domain.Cities
{
    public interface ICityDomainService
    {
        Task<Result<int>> Create(City city);

        Task<Result> Delete(int id);

        Task<(IEnumerable<City>, int)> Filter(string filter, int index, int showPerPage, string sortingColumn);
    }
}
