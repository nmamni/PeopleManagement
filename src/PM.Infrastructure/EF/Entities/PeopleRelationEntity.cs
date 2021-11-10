using PM.Domain.People;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Infrastructure.EF.Entities
{
    public class PeopleRelationEntity : TEntity
    {
        public int PersonID { get; set; }
        public virtual PersonEntity Person { get; set; }

        public int RelatedPersonID { get; set; }
        public virtual PersonEntity RelatedPerson { get; set; }

        public RelationTypes RelationType { get; set; }
    }
}
