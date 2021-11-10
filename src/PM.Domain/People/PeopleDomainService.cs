using PM.Common.CommonModels;
using PM.Domain.Interfaces.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PM.Domain.People
{
    public class PeopleDomainService : IPeopleDomainService
    {
        private readonly IPeopleRepository _peopleRepository;

        public PeopleDomainService(IPeopleRepository peopleRepository)
        {
            _peopleRepository = peopleRepository;
        }

        public async Task<(IEnumerable<Person>, int)> FastSearch(string filter, int index, int showPerPage, string sortingColumn)
        {
            var people = await _peopleRepository.FilterAsync(filter, index * showPerPage, showPerPage, sortingColumn);
            var quantity = await _peopleRepository.FilterCountAsync(filter);
            return (people, quantity);

        }

        public async Task<Result<int>> CreatePerson(Person person)
        {
            return await _peopleRepository.AddAsync(person);
        }

        public async Task<Person> GetPerson(int id)
        {
            var person = await _peopleRepository.GetAsync(id);
            person.AddRelatedPeople(await _peopleRepository.GetRelationPeople(id));
            return person;
        }

        public async Task<Result> UpdatePerson(int personID, Person person)
        {
            return await _peopleRepository.UpdateAsync(personID, person);
        }

        public async Task<Result> Delete(int id)
        {
            return await _peopleRepository.RemoveAsync(id);
        }

        public async Task<(IEnumerable<Person>, int)> DeepSearch(PeopleFilter person, int index, int showPerPage, string sortingColumn)
        {
            var people = await _peopleRepository.FilterInDetailsAsync(person, index * showPerPage, showPerPage, sortingColumn);
            var quantity = await _peopleRepository.FilterDetailsCountAsync(person);
            return (people, quantity);
        }
    }
}
