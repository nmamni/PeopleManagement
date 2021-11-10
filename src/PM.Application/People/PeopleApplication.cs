using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using PM.Common.CommonModels;
using PM.Common.Exceptions;
using PM.Domain.People;
using PM.Infrastructure.FileSytem;
using PM.Infrastructure.SharedResources;

namespace PM.Application.People
{
    public class PeopleApplication : IPeopleApplication
    {
        private readonly IPeopleDomainService _peopleDomainService;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly IFileSystemClient _fileSystemClien;
        private readonly IMapper mapper;

        public PeopleApplication(IPeopleDomainService peopleDomainService,
            IStringLocalizer<SharedResource> sharedLocalizer,
            IFileSystemClient fileSystemClient,
            IMapper mapper)
        {
            _peopleDomainService = peopleDomainService;
            _sharedLocalizer = sharedLocalizer;
            _fileSystemClien = fileSystemClient;
            this.mapper = mapper;
        }

        public async Task<Result<int>> CreatePerson(CreatePersonCommand cmd)
        {

            var person = new Person(cmd.FirstName,
                                cmd.LastName,
                                (GenderTypes)cmd.Gender,
                                cmd.PersonalNumber,
                                cmd.BirthDate);

            person.CityID = cmd.CityID;
            if (!string.IsNullOrEmpty(cmd.PhoneNumber))
                person.PhoneNumber =
                    new PhoneNumber(cmd.PhoneNumber, (PhoneNumberTypes)cmd.PhoneNumberType);

            return await _peopleDomainService.CreatePerson(person);
        }

        public async Task<FilterResponse<IEnumerable<PeopleListItem>>> Filter(FilterModel<string> f)
        {
            var searchResult = await _peopleDomainService.FastSearch(f.Filter,
                f.PageRequest.Index,
                f.PageRequest.ShowPerPage,
                f.PageRequest.SortingColumn);

            var items = searchResult.Item1.Select(i => new PeopleListItem
            {
                ID = i.ID,
                BirthDate = i.BirthDate,
                City = i.City,
                CityID = i.CityID,
                FirstName = i.FirstName,
                Gender = (int)i.Gender,
                ImageUrl = i.ImageUrl,
                LastName = i.LastName,
                PersonalNumber = i.PersonalNumber,
                PhoneNumber = i.PhoneNumber.Number.Value,
            });

            return new FilterResponse<IEnumerable<PeopleListItem>>(items, searchResult.Item2);
        }

        public async Task<FilterResponse<IEnumerable<PeopleListItem>>> FilterInDetails(FilterModel<FilterPeopleQuery> f)
        {
            var personFilter = new PeopleFilter
            {
                BirthDate = f.Filter.BirthDate,
                CityID = f.Filter.CityID,
                FirstName = f.Filter.FirstName,
                Gender = f.Filter.Gender,
                ID = f.Filter.ID,
                LastName = f.Filter.LastName,
                PersonalNumber = f.Filter.PersonalNumber,
                PhoneNumber = f.Filter.PhoneNumber,
                PhoneNumberType = f.Filter.PhoneNumberType
            };

            var searchResult = await _peopleDomainService.DeepSearch(personFilter,
                f.PageRequest.Index,
                f.PageRequest.ShowPerPage,
                f.PageRequest.SortingColumn);

            var items = searchResult.Item1.Select(i => new PeopleListItem
            {
                ID = i.ID,
                BirthDate = i.BirthDate,
                City = i.City,
                CityID = i.CityID,
                FirstName = i.FirstName,
                Gender = (int)i.Gender,
                ImageUrl = i.ImageUrl,
                LastName = i.LastName,
                PersonalNumber = i.PersonalNumber,
                PhoneNumber = i.PhoneNumber.Number.Value,
            });

            return new FilterResponse<IEnumerable<PeopleListItem>>(items, searchResult.Item2);
        }

        public async Task<Result> Update(int id, UpdatePersonCommand cmd)
        {
            try
            {
                var person = new Person(cmd.FirstName,
                                    cmd.LastName,
                                    (GenderTypes)cmd.Gender,
                                    cmd.PersonalNumber,
                                    cmd.BirthDate);

                person.ID = id;
                person.CityID = cmd.CityID;
                person.PhoneNumber =
                    new PhoneNumber(cmd.PhoneNumber, (PhoneNumberTypes)cmd.PhoneNumberType);

                return await _peopleDomainService.UpdatePerson(id, person);
            }
            catch (Exception ex)
            {
                return new Result<int>(-1, false, ex.Message, 0);
            }
        }

        public async Task<PersonDetails> Get(int id)
        {
            var person = await _peopleDomainService.GetPerson(id);

            if (person == null)
                return null;

            return new PersonDetails
            {
                ID = person.ID,
                BirthDate = person.BirthDate,
                City = person.City,
                CityID = person.CityID,
                FirstName = person.FirstName,
                Gender = (int)person.Gender,
                ImageUrl = person.ImageUrl,
                LastName = person.LastName,
                PersonalNumber = person.PersonalNumber,
                PhoneNumber = person.PhoneNumber.Number.Value,
                PhoneNumberType = (int)person.PhoneNumber.PhoneNumberType,
                RelatedPeople = person.RelatedPeople?.Select(rp => new RelatedPersonDetails
                {
                    ID = rp.ID,
                    BirthDate = rp.BirthDate,
                    City = rp.City,
                    CityID = rp.CityID,
                    FirstName = rp.FirstName,
                    Gender = (int)rp.Gender,
                    ImageUrl = rp.ImageUrl,
                    LastName = rp.LastName,
                    PersonalNumber = rp.PersonalNumber,
                    PhoneNumber = rp.PhoneNumber,
                    RelationType = rp.RelationType,
                    RelationID = rp.RelationID,
                })

            };
        }

        public async Task<Result> Delete(int id)
        {
            return await _peopleDomainService.Delete(id);
        }

        public async Task<Result> MakeRelation(int personID, int targetID, RelationTypes type)
        {
            try
            {
                var person = await _peopleDomainService.GetPerson(personID);
                var relativePerson = await _peopleDomainService.GetPerson(targetID);
                person.AddRelatedPerson(relativePerson, type);
                return await _peopleDomainService.UpdatePerson(personID, person);
            }
            catch (Exception ex)
            {
                return new Result(-1, false, ex.Message);
            }
        }

        public async Task<Result> RemoveRelation(int personID, int relationID)
        {
            try
            {
                var person = await _peopleDomainService.GetPerson(personID);
                person.RemoveRelatedPerson(relationID);
                return await _peopleDomainService.UpdatePerson(personID, person);
            }
            catch (Exception ex)
            {
                return new Result(-1, false, ex.Message);
            }
        }

        public async Task<Result<string>> SavePhoto(int id, IFormFile file)
        {
            var name = Guid.NewGuid().ToString() + "." + file.FileName.Split(".").Last();
            await _fileSystemClien.SaveImage(file, name);
            var person = await _peopleDomainService.GetPerson(id);
            person.ImageUrl = "/api/cdnfake/" + name;
            var result = await _peopleDomainService.UpdatePerson(id, person);
            if (result.IsSuccess)
                return Result<string>.GetSuccessInstance("/api/statics/" + name);
            else return new Result<string>(result.StatusCode, result.IsSuccess, result.Message, "");
        }

        public FileStream GetPhoto(string name)
        {
            return _fileSystemClien.GetImage(name);
        }

        public async Task<FilterResponse<IEnumerable<RelationsReportListItem>>> GetRelationsReport(FilterModel<string> f)
        {
            var searchResult = await _peopleDomainService.FastSearch(f.Filter,
                 f.PageRequest.Index,
                 f.PageRequest.ShowPerPage,
                 f.PageRequest.SortingColumn);

            var items = new List<RelationsReportListItem>();
            foreach (var i in searchResult.Item1)
            {
                var reportItem = new RelationsReportListItem
                {
                    ID = i.ID,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    PersonalNumber = i.PersonalNumber,
                    Relations = new List<RelationTypeRemortListItem>()
                };
                foreach (RelationTypes tp in Enum.GetValues(typeof(RelationTypes)))
                {
                    reportItem.Relations.Add(new RelationTypeRemortListItem
                    {
                        Type = tp,
                        Quantity = i.RelatedPeople.Count(p=>p.RelationType == tp)
                    }); 
                }
                items.Add(reportItem);
            } 

            return new FilterResponse<IEnumerable<RelationsReportListItem>>(items, searchResult.Item2);
        }
    }
}
