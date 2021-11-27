using System;

namespace BL.API.Core.Abstractions.Model
{
    public interface IBaseEntity
    {   
        public Guid Id { get; set; }
    }
}
