using PM.Domain.People;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Infrastructure.EF.Entities
{
    public class PersonEntity : TEntity
    {
        public GenderTypes Gender { get; set; }
        public string PersonalNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public string PhoneNumber { get; set; }
        public PhoneNumberTypes PhoneNumberType { get; set; }
        public virtual CityEntity City { get; set; }

        public int? CityID { get; set; }

        public List<PeopleRelationEntity> Relations { get; set; }
    }
}
