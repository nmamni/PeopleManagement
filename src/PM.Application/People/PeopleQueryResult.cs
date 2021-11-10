using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Application.People
{
    public class PeopleQueryResult
    {
        public int ItemsCount { get; set; }
        public IEnumerable<PeopleListItem> People { get; set; }
    }
}
