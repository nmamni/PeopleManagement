using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Common.Exceptions
{
    public class LocalizableException : Exception
    {
        public string MessageKey { get; private set; }
        public LocalizableException(string messageKey, string errorMessage) : base(errorMessage)
        {
            MessageKey = messageKey;
        }
    }
}
