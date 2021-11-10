using PM.Domain.People;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PM.Domain.Interfaces.Repository
{
    public interface IPeopleRepository : IRepository<Person>
    {
        ICollection<Person> Filter(string searchWord, int index, int showPerPage, string sortingColumn);
        Task<ICollection<Person>> FilterAsync(string searchWord, int index, int showPerPage, string sortingColumn);

        int FilterCount(string searchWord);

        Task<int> FilterCountAsync(string searchWord);
        Task<IEnumerable<RelatedPerson>> GetRelationPeople(int id);
        Task<ICollection<Person>> FilterInDetailsAsync(PeopleFilter filter, int v, int showPerPage, string sortingColumn);
        Task<int> FilterDetailsCountAsync(PeopleFilter filter);
    }
}
