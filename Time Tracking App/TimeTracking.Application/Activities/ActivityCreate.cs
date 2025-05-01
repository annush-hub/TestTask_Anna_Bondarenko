using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TimeTracking.Application.DTOs;
using TimeTracking.Core;
using TimeTracking.Core.Repositories;

namespace TimeTracking.Application.Activities
{
    public class ActivityCreate
    {
        public record Command(CreateActivityDto ActivityCreateDto) : IRequest<ActivityDto>;

        public class Handler : IRequestHandler<Command, ActivityDto>
        {
            private readonly IRepository<Activity> _activityRepository;
            private readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;

            public Handler(IRepository<Activity> activityRepository, IMapper mapper, ILogger<Handler> logger)
            {
                _activityRepository = activityRepository;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<ActivityDto> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    _logger.LogInformation("Processing activity creation for Employee: {EmployeeFullName}, Project: {ProjectName}",
                        request.ActivityCreateDto.EmployeeId, request.ActivityCreateDto.ProjectId);

                    var activity = _mapper.Map<Activity>(request.ActivityCreateDto);

                    _logger.LogInformation("Mapped CreateActivityDto to Activity. Starting database insertion.");

                    var createdActivity = await _activityRepository.AddAsync(activity);

                    _logger.LogInformation("Activity with Id: {ActivityId} successfully created.", createdActivity.Id);

                    var fullActivity = await _activityRepository.GetSingleAsync(
                        a => a.Id == createdActivity.Id,
                        includes: [a => a.Employee, a => a.Project]
                    );

                    _logger.LogInformation("Successfully retrieved full Activity details from database.");

                    return _mapper.Map<ActivityDto>(fullActivity);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while creating the activity.");
                    throw;
                }
            }
        }
    }
}
