using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PM.Application.People;
using PM.Domain.People;

namespace PM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelationsController : ControllerBase
    {
        IPeopleApplication _peopleApplication;

        public RelationsController(IPeopleApplication peopleApplication)
        {
            _peopleApplication = peopleApplication;
        }


        // GET: api/Relations/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Relations
        [HttpPost("{id}/{targetID}")]
        public async Task Post(int id, int targetID, RelationTypes relationType)
        {
            await _peopleApplication.MakeRelation(id, targetID, relationType);
        }


        // DELETE: api/Relations
        [HttpDelete("{id}/{relationID}")]
        public async Task Delete(int id, int relationID)
        {
            await _peopleApplication.RemoveRelation(id, relationID);
        }
    }
}
