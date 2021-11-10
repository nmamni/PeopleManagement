using PM.Domain.BaseClasses;
using PM.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Domain.People
{
    public class PhoneNumber : BaseEntity
    {
        public PhoneNumber(string number, PhoneNumberTypes numberType)
        {
            Number = new Number(number);
            PhoneNumberType = numberType;
        }

        public Number Number { get; set; }

        public PhoneNumberTypes PhoneNumberType { get; set; }
    }
}
