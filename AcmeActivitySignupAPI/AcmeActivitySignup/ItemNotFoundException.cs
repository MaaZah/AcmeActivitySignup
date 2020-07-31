using System;
using System.Globalization;

namespace AcmeActivitySignup
{
    /// <summary>
    /// Custom Exception type for when an object is not found in the datastore
    /// </summary>
    public class ItemNotFoundException : ApplicationException
    {
        public ItemNotFoundException() : base() { }

        public ItemNotFoundException(string message) : base(message) { }

        public ItemNotFoundException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args)) { }

        public ItemNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}