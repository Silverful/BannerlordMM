using System;

namespace BL.API.Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Entity not found")
        {

        }
    }
}
