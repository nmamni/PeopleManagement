using PM.Common.Exceptions;
using PM.Domain.BaseClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Domain.Cities
{
    public class City : BaseEntity, IAggregateRoot
    {
        public City(string name)
        {
            Name = name;
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set {
                if(string.IsNullOrEmpty(value))
                    throw new LocalizableException("CITY_NAME_IS_REQUIRED", "CITY_NAME_IS_REQUIRED");
                _name = value; }
        }

    }
}
