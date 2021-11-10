using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PM.Application.People;

namespace PM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class cdnfakeController : ControllerBase
    {
        private readonly IPeopleApplication _peopleApplication;

        public cdnfakeController(IPeopleApplication peopleApplication)
        {
            _peopleApplication = peopleApplication;
        }

        // GET: api/People/5
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            return  File(_peopleApplication.GetPhoto(name), "image/jpeg");
        }
    }
}