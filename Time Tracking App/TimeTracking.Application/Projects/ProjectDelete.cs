using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Core.Repositories;
using TimeTracking.Core;

namespace TimeTracking.Application.Projects
{
    public class ProjectDelete
    {
        public record Command(Guid Id) : IRequest<bool>;

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IRepository<Project> _repository;
            private readonly ILogger<Handler> _logger;

            public Handler(IRepository<Project> repository, ILogger<Handler> logger)
            {
                _repository = repository;
                _logger = logger;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var project = await _repository.GetByIdAsync(request.Id);
                if (project == null)
                {
                    _logger.LogWarning($"Project with id {request.Id} not found");
                    return false;
                }

                await _repository.DeleteAsync(project);

                _logger.LogInformation("Project with Id={ProjectId} deleted successfully.", request.Id);
                return true;
            }
        }
    }
}
