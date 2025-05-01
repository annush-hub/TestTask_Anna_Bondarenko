using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TimeTracking.Application.DTOs;
using TimeTracking.Core;
using TimeTracking.Core.Repositories;
using TimeTracking.Core.Specifications;

namespace TimeTracking.Application.TimeTracking
{
    public class TimeTrackingByPersonAndDate
    {
        public record Query(Guid EmployeeId, DateTime Date) : IRequest<List<TimeTrackingEntryDto>>;

        public class Handler : IRequestHandler<Query, List<TimeTrackingEntryDto>>
        {
            private readonly IRepository<Activity> _repository;
            private readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;

            public Handler(IRepository<Activity> repository, IMapper mapper, ILogger<Handler> logger)
            {
                _repository = repository;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<List<TimeTrackingEntryDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Fetching activities for EmployeeId={EmployeeId} on {Date}.", 
                                      request.EmployeeId, request.Date.ToShortDateString());

                var specification = new ActivityByEmployeeAndDateSpecification(request.EmployeeId, request.Date);

                var activities = await _repository.GetAsync(specification);

                if (activities == null || !activities.Any())
                {
                    _logger.LogWarning($"No activities found for EmployeeId={request.EmployeeId} on {request.Date.ToShortDateString()}.");
                    return new List<TimeTrackingEntryDto>();
                }

                _logger.LogInformation("Found {ActivityCount} activities for EmployeeId={EmployeeId} on {Date}.",
                                      activities.Count, request.EmployeeId, request.Date.ToShortDateString());

                return activities.Select(a => new TimeTrackingEntryDto
                {
                    Date = a.Date,
                    Role = a.Role.ToString(),
                    Project = a.Project.Name,
                    ActivityType = a.ActivityType.ToString(),
                    Hours = a.Hours
                }).ToList();
            }
        }
    }
}
