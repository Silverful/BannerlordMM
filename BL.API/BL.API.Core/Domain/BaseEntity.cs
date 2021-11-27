using BL.API.Core.Abstractions.Model;
using System;

namespace BL.API.Core.Domain
{
    public class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
    }
}
