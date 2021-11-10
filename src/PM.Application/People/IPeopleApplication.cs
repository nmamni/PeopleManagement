using Microsoft.AspNetCore.Http;
using PM.Common.CommonModels;
using PM.Domain.People;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PM.Application.People
{
    public interface IPeopleApplication
    {
        Task<Result<int>> CreatePerson(CreatePersonCommand command);
        Task<FilterResponse<IEnumerable<PeopleListItem>>> Filter(FilterModel<string> f);
        Task<FilterResponse<IEnumerable<RelationsReportListItem>>> GetRelationsReport(FilterModel<string> f);
        Task<FilterResponse<IEnumerable<PeopleListItem>>> FilterInDetails(FilterModel<FilterPeopleQuery> f);
        Task<Result> Update(int id, UpdatePersonCommand cmd);
        Task<PersonDetails> Get(int id);

        Task<Result> Delete(int id);

        Task<Result> MakeRelation(int personID, int targetID, RelationTypes type);

        Task<Result> RemoveRelation(int personID, int targetID);
        Task<Result<string>> SavePhoto(int id, IFormFile file);
        FileStream GetPhoto(string name);
    }
}
