using System;

namespace BL.API.Core.Exceptions
{
    public class NotAuthorized : Exception
    {
        public NotAuthorized() : base("User not authorized")
        {
        }
    }
}
