using BL.API.Core.Abstractions.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BL.API.Core.Domain.User
{
    public class RefreshToken : IBaseEntity
    {
        public Guid Id { get; set; }
        [Key]
        public string Token { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
