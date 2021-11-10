using PM.Domain.Cities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PM.Domain.Interfaces.Repository
{
    public interface ICitiesRepository : IRepository<City>
    {
        ICollection<City> Filter(string searchWord, int index, int showPerPage, string sortingColumn);
        Task<ICollection<City>> FilterAsync(string searchWord, int index, int showPerPage, string sortingColumn);

        int FilterCount(string searchWord, int index, int showPerPage, string sortingColumn);
        Task<int> FilterCountAsync(string searchWord, int index, int showPerPage, string sortingColumn);
    }
}
