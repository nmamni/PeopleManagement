using PM.Domain.BaseClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Domain.ValueObjects
{
    public class Number : BaseValueObject
    {
        public Number(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
