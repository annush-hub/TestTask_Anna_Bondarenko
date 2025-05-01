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
    public class ProjectById
    {
        public record Query(Guid Id) : IRequest<ProjectDto>;

        public class Handler : IRequestHandler<Query, ProjectDto>
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

            public async Task<ProjectDto> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    _logger.LogInformation("Processing request to fetch Project with Id={ProjectId}.", request.Id);

                    var project = await _repository.GetSingleAsync(p => p.Id == request.Id);

                    if (project == null)
                    {
                        _logger.LogWarning("Project with Id='{ProjectId}' was not found.", request.Id);
                        return null!;
                    }

                    _logger.LogInformation("Project with Id='{ProjectId}' found. Mapping to ProjectDto.", request.Id);

                    return _mapper.Map<ProjectDto>(project);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while fetching Project with Id={ProjectId}.", request.Id);
                    throw; 
                }
            }
        }
    }
}
