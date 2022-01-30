using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Settings;
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

        public class UpdateConfigCommandHandler : IRequestHandler<UpdateConfigCommand, Task>
        {
            private readonly IRepository<Configuration> _repository;

            public UpdateConfigCommandHandler(IRepository<Configuration> repository)
            {
                _repository = repository;
            }

            public async Task<Task> Handle(UpdateConfigCommand request, CancellationToken cancellationToken)
            {
                var conf = await _repository.GetFirstWhereAsync(x => x.ConfigName == request.ConfigName);

                conf.Value = request.Value;

                await _repository.UpdateAsync(conf);

                return Task.CompletedTask;
            }
        }
    }
}
