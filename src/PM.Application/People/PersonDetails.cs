using PM.Domain.People;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Application.People
{
    public class PersonDetails
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Gender { get; set; }
        public string PersonalNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public int? CityID { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public int PhoneNumberType { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<RelatedPersonDetails> RelatedPeople { get; set; }
    }

    public class RelatedPersonDetails
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Gender { get; set; }
        public string PersonalNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public int? CityID { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public int PhoneNumberType { get; set; }
        public string ImageUrl { get; set; }
        public RelationTypes RelationType { get; set; }

        public int RelationID { get; set; }
    }
}
