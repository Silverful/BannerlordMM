using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Settings;
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

        public class AddConfigCommandHandler : IRequestHandler<AddConfigCommand, Guid>
        {
            private readonly IRepository<Configuration> _repository;

            public AddConfigCommandHandler(IRepository<Configuration> repository)
            {
                _repository = repository;
            }


            public async Task<Guid> Handle(AddConfigCommand request, CancellationToken cancellationToken)
            {
                var configuration = new Configuration
                {
                    ConfigName = request.ConfigName,
                    Value = request.Value
                };

                await _repository.CreateAsync(configuration);

                return configuration.Id;
            }
        }
    }
}
