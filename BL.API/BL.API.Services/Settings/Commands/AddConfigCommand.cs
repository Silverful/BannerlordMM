using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Settings;
using BL.API.Core.Exceptions;
using BL.API.Services.Regions.Queries;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Settings.Commands
{
    public class AddConfigCommand : IRequest<Guid>
    {
        [Required]
        public string ConfigName { get; set; }
        public string Value { get; set; }
        public string RegionShortName { get; set; }

        public class AddConfigCommandHandler : IRequestHandler<AddConfigCommand, Guid>
        {
            private readonly IRepository<Configuration> _repository;
            private readonly IMediator _mediator;

            public AddConfigCommandHandler(IRepository<Configuration> repository, IMediator mediator)
            {
                _repository = repository;
                _mediator = mediator;
            }


            public async Task<Guid> Handle(AddConfigCommand request, CancellationToken cancellationToken)
            {
                var region = await _mediator.Send(new GetRegionByShortName.Query(request.RegionShortName));

                if ((await _repository.GetFirstWhereAsync(s => s.RegionId == region.Id && s.ConfigName == request.ConfigName)) != null)
                {
                    throw new AlreadyExistsException();
                }

                var configuration = new Configuration
                {
                    ConfigName = request.ConfigName,
                    Value = request.Value,
                    RegionId = region.Id
                };


                await _repository.CreateAsync(configuration);

                return configuration.Id;
            }
        }
    }
}
