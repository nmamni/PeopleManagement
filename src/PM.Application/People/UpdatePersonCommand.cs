using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Application.People
{
    public class UpdatePersonCommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Gender { get; set; }
        public string PersonalNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public int? CityID { get; set; }
        public string PhoneNumber { get; set; }
        public int PhoneNumberType { get; set; }
    }
}
