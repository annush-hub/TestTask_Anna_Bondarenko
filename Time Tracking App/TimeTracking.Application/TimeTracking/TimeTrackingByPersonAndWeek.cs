using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.DTOs;
using TimeTracking.Core;
using TimeTracking.Core.Repositories;
using TimeTracking.Core.Specifications;

namespace TimeTracking.Application.TimeTracking
{
    public class TimeTrackingByPersonAndWeek
    {
        public record Query(Guid EmployeeId, int Year, int WeekNumber) : IRequest<List<TimeTrackingEntryDto>>;

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
                _logger.LogInformation("Fetching time tracking entries for EmployeeId={EmployeeId} for Year={Year}, WeekNumber={WeekNumber}.",
                                        request.EmployeeId, request.Year, request.WeekNumber);
                var spec = new ActivityByEmployeeAndYearSpecification(request.EmployeeId, request.Year);
                var activities = await _repository.GetAsync(spec);

                _logger.LogInformation("Retrieved {ActivityCount} activities for EmployeeId={EmployeeId} for Year={Year}.", 
                                        activities.Count, request.EmployeeId, request.Year);

                var filteredActivities = activities
                    .Where(a => ActivityByEmployeeAndWeekSpecification.GetWeekNumber(a.Date) == request.WeekNumber)
                    .ToList();

                _logger.LogInformation("Filtered {FilteredActivityCount} activities for WeekNumber={WeekNumber}.", 
                                        filteredActivities.Count, request.WeekNumber);


                var timeTrackingEntries = _mapper.Map<List<TimeTrackingEntryDto>>(filteredActivities);

                return timeTrackingEntries;
            }

        }
    }

}
