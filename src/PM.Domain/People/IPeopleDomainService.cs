using PM.Common.CommonModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PM.Domain.People
{
    public interface IPeopleDomainService
    {
        Task<Result<int>> CreatePerson(Person person);

        Task<(IEnumerable<Person>, int)> FastSearch(string filter, int index, int showPerPage, string sortingColumn);

        Task<Person> GetPerson(int id);

        Task<Result> UpdatePerson(int personID, Person person);

        Task<Result> Delete(int id);

        Task<(IEnumerable<Person>, int)> DeepSearch(PeopleFilter filter, int index, int showPerPage, string sortingColumn);
    }
}
