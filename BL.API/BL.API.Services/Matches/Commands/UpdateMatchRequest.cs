using System;
using System.ComponentModel.DataAnnotations;

namespace BL.API.Services.Matches.Commands
{
    public class UpdateMatchRequest : UploadMatchRequest
    {
        [Required]
        public Guid MatchId { get; set; }
    }
}
