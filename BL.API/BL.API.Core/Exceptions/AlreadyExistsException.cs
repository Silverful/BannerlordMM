using System;

namespace BL.API.Core.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException() : base("Already exists")
        {

        }
    }
}
