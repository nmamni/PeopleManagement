using PM.Domain.People;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Application.People
{
    public class RelationsReportListItem
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalNumber { get; set; }

        public List<RelationTypeRemortListItem> Relations { get; set; }
    }

    public class RelationTypeRemortListItem
    {
        public RelationTypes Type { get; set; }
        public int Quantity { get; set; }
    }
}
