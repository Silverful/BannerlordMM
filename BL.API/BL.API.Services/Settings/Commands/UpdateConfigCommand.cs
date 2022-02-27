using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Settings;
using BL.API.Services.Regions.Queries;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Settings.Commands
{
    public class UpdateConfigCommand : IRequest<Task>
    {
        [Required]
        public string ConfigName { get; set; }
        public string Value { get; set; }
        public string RegionShortName { get; set; }

        public class UpdateConfigCommandHandler : IRequestHandler<UpdateConfigCommand, Task>
        {
            private readonly IRepository<Configuration> _repository;
            private readonly IMediator _mediator;

            public UpdateConfigCommandHandler(IRepository<Configuration> repository, IMediator mediator)
            {
                _repository = repository;
                _mediator = mediator;
            }

            public async Task<Task> Handle(UpdateConfigCommand request, CancellationToken cancellationToken)
            {
                var region = await _mediator.Send(new GetRegionByShortName.Query(request.RegionShortName));
                var conf = await _repository.GetFirstWhereAsync(x => x.ConfigName == request.ConfigName && x.Region.ShortName == region.ShortName, false);

                conf.Value = request.Value;

                await _repository.UpdateAsync(conf);

                return Task.CompletedTask;
            }
        }
    }
}
