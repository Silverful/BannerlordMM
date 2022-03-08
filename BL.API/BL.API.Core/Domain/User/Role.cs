using Microsoft.AspNetCore.Identity;
using System;

namespace BL.API.Core.Domain.User
{
    public class Role : IdentityRole<Guid>
    {
        public Role() : base()
        {

        }

        public Role(string Role) : base(Role)
        {

        }
    }
}
