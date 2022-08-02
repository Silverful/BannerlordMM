using System;

namespace BL.API.Services.Clans.Commands
{
    public class JoinRequestClanResponse
    {
        public Guid? RequestId { get; set; }
        public string Message { get; set; }
    }
}
