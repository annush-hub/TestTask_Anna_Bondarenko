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
    public class ProjectList
    {
        public record Query() : IRequest<List<ProjectDto>>;

        public class Handler : IRequestHandler<Query, List<ProjectDto>>
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

            public async Task<List<ProjectDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Fetching all projects.");

                var projects = await _repository.GetAllAsync();
                _logger.LogInformation("Successfully fetched {ProjectCount} projects.", projects.Count);

                return _mapper.Map<List<ProjectDto>>(projects);
            }
        }
    }
}
