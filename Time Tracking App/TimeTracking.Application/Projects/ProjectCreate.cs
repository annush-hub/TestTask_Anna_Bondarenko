using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.DTOs;
using TimeTracking.Core.Repositories;
using TimeTracking.Core;
using Microsoft.Extensions.Logging;

namespace TimeTracking.Application.Projects
{
    public class ProjectCreate
    {
        public record Command(CreateProjectDto ProjectDto) : IRequest<ProjectDto>;

        public class Handler : IRequestHandler<Command, ProjectDto>
        {
            private readonly IRepository<Project> _projectRepository;
            private readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;

            public Handler(IRepository<Project> projectRepository, IMapper mapper, ILogger<Handler> logger)
            {
                _projectRepository = projectRepository;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<ProjectDto> Handle(Command request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Starting to create a new project.");

                var project = _mapper.Map<Project>(request.ProjectDto);
                var createdProject = await _projectRepository.AddAsync(project);

                _logger.LogInformation("Project with Id={ProjectId} created successfully.", createdProject.Id);

                return _mapper.Map<ProjectDto>(createdProject);
            }
        }
    }
}
