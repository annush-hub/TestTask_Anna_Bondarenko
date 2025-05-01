using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TimeTracking.Application.DTOs;
using TimeTracking.Core;
using TimeTracking.Core.Repositories;

namespace TimeTracking.Application.Activities
{
    public class ActivityById
    {
        public record Query(Guid Id) : IRequest<ActivityDto>;

        public class Handler : IRequestHandler<Query, ActivityDto>
        {
            private readonly IRepository<Activity> _repository;
            private readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;

            public Handler(IRepository<Activity> repository, IMapper mapper, ILogger<Handler> logger)
            {
                _mapper = mapper;
                _logger = logger;
                _repository = repository;
            }

            public async Task<ActivityDto> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var activity = await _repository.GetSingleAsync(a => a.Id == request.Id, includes: [a => a.Employee, a => a.Project]);

                    if (activity == null)
                    {
                        _logger.LogError("Activity with Id='{ActivityId}' was not found.", request.Id);
                        throw new Exception($"Activity with Id '{request.Id}' not found.");
                    }

                    return _mapper.Map<ActivityDto>(activity);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while retrieving activity with Id='{ActivityId}'", request.Id);
                    throw; 
                }
            }
        }
    }
}
