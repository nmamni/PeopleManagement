using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PM.Application.People;
using PM.Common.CommonModels;

namespace PM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IPeopleApplication _peopleApplication;

        public ReportsController(IPeopleApplication peopleApplication)
        {
            this._peopleApplication = peopleApplication;
        }

        [HttpGet]
        public async Task<FilterResponse<IEnumerable<RelationsReportListItem>>> Relations([FromQuery]string searchWord,
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

            return await _peopleApplication.GetRelationsReport(filter);
        }
    }
}