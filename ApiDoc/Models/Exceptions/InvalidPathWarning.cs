using System;

namespace ApiDoc.Models.Exceptions
{
    public class InvalidPathWarning : Exception
    {
        public InvalidPathWarning() { }
        public InvalidPathWarning(string message) : base(message) { }
    }
}