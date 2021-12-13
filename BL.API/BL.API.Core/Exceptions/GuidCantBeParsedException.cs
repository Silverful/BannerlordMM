using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.API.Core.Exceptions
{
    public class GuidCantBeParsedException : Exception
    {
        public GuidCantBeParsedException() : base("Guid can not be parsed")
        {

        }
    }
}
