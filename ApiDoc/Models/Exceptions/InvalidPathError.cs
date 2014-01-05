using System;

namespace ApiDoc.Models.Exceptions
{
    public class InvalidPathError : Exception
    {
        public InvalidPathError() { }
        public InvalidPathError(string message) : base(message){}
    }
}