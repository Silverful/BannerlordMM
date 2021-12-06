using BL.API.Core.Abstractions.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace BL.API.Core.Domain
{
    public class BaseEntity : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
    }
}
