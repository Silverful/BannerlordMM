using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Settings;
using BL.API.Core.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Regions.Commands
{
    public class AddRegionCommand : IRequest<Guid>
    {
        public string ShortName { get; set; }
        public string Name { get; set; }

        public class AddRegionCommandHandler : IRequestHandler<AddRegionCommand, Guid>
        {
            private readonly IRepository<Region> _repository;

            public AddRegionCommandHandler(IRepository<Region> repository)
            {
                _repository = repository;
            }

            public async Task<Guid> Handle(AddRegionCommand request, CancellationToken cancellationToken)
            {
                if (await _repository.GetFirstWhereAsync(x => x.ShortName == request.ShortName) != null)
                {
                    throw new AlreadyExistsException();
                }

                var region = new Region
                {
                    ShortName = request.ShortName,
                    Name = request.Name
                };

                await _repository.CreateAsync(region);

                return region.Id;
            }
        }
    }
}
