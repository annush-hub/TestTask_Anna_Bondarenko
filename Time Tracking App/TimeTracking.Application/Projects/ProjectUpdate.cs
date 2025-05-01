using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.DTOs;
using TimeTracking.Core.Repositories;
using TimeTracking.Core;

namespace TimeTracking.Application.Projects
{
    public class ProjectUpdate
    {
        public record Command(Guid Id, CreateProjectDto UpdatedProjectDto) : IRequest<ProjectDto>;

        public class Handler : IRequestHandler<Command, ProjectDto>
        {
            private readonly IRepository<Project> _repository;
            private readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;

            public Handler(IRepository<Project> repository, IMapper mapper, ILogger<Handler> logger)
            {
                _repository = repository;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<ProjectDto> Handle(Command request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Attempting to update project with Id={ProjectId}.", request.Id);

                var existing = await _repository.GetByIdAsync(request.Id);
                if (existing == null)
                {
                    _logger.LogWarning($"Project with id {request.Id} not found");
                    return null!;
                }

                _mapper.Map(request.UpdatedProjectDto, existing);
                await _repository.UpdateAsync(existing);

                _logger.LogInformation("Project with Id={ProjectId} updated successfully.", request.Id);

                return _mapper.Map<ProjectDto>(existing);
            }
        }
    }
}
