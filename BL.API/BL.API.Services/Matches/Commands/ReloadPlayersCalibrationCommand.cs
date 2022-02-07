using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Matches.Commands
{
    public class ReloadPlayersCalibrationCommand
    {
        public record Query(Guid PlayerId) : IRequest<Task>;

        public class ReloadPlayersCalibrationCommandHandler : IRequestHandler<Query, Task>
        {
            public Task<Task> Handle(Query request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
