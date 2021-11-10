using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PM.Application.Cities;
using PM.Common.CommonModels;

namespace PM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICitiesApplication _citiesApplication;
        public CitiesController(ICitiesApplication citiesApplication)
        {
            _citiesApplication = citiesApplication;
        }

        // GET: api/Cities
        [HttpGet]
        public async Task<FilterResponse<IEnumerable<CityListItem>>> Get([FromQuery]string searchWord,
                                                    [FromQuery]int index = 0,
                                                    [FromQuery]int nitems = 10,
                                                    [FromQuery] string ordering = "ID")
        {
            var filter = new FilterModel<string>
            {
                Filter = searchWord,
                PageRequest =
                {
                    Index = index,
                    ShowPerPage = nitems,
                    SortingColumn = ordering
                }
            };

            return await _citiesApplication.Filter(filter);
        }


        // POST: api/Cities
        [HttpPost]
        public async Task<Result<int>> Post([FromBody] CreateCityCommand createCityCommand)
        {
            return await _citiesApplication.Create(createCityCommand);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<Result> Delete(int id)
        {
            return await _citiesApplication.Delete(id);
        }
    }
}
