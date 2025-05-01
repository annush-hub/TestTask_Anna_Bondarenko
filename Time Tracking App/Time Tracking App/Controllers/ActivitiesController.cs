using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using TimeTracking.Application.Activities;
using TimeTracking.Application.DTOs;
using TimeTracking.Application.TimeTracking;

namespace Time_Tracking_App.Controllers
{
    /// <summary>
    /// Controller for managing activities and time tracking.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly HttpClient _httpClient;

        public ActivitiesController(IMediator mediator, HttpClient httpClient)
        {
            _mediator = mediator;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Creates a new activity and sends it to a client service.
        /// </summary>
        /// <param name="activityCreate">The activity creation DTO.</param>
        /// <returns>The created activity DTO.</returns>
        [HttpPost]
        public async Task<ActionResult<ActivityDto>> Create([FromBody] CreateActivityDto activityCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var activity = await _mediator.Send(new ActivityCreate.Command(activityCreate));

            var jsonContent = JsonConvert.SerializeObject(activity);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:5001/activity", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Activity sent to client successfully.");
            }
            else
            {
                Console.WriteLine("Failed to send activity to client.");
            }

            return CreatedAtAction(nameof(GetActivityById), new { id = activity.Id }, activity);       
        }


        /// <summary>
        /// Gets an activity by its unique ID.
        /// </summary>
        /// <param name="id">The activity ID.</param>
        /// <returns>The activity DTO.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDto>> GetActivityById(Guid id)
        {
            return await _mediator.Send(new ActivityById.Query(id));
        }

        /// <summary>
        /// Gets time tracking entries for a specific person on a given date.
        /// </summary>
        /// <param name="personId">The person’s ID.</param>
        /// <param name="date">The date of the activity.</param>
        /// <returns>List of time tracking entries.</returns>
        [HttpGet("by-person")]
        public async Task<ActionResult<List<TimeTrackingEntryDto>>> 
            GetByPersonAndDate([FromQuery] Guid personId, [FromQuery] DateTime date)
        {
            return await _mediator.Send(new TimeTrackingByPersonAndDate.Query(personId, date));
        }

        /// <summary>
        /// Gets time tracking entries for a specific employee by year and ISO week number.
        /// </summary>
        /// <param name="employeeId">The employee's ID.</param>
        /// <param name="year">The year.</param>
        /// <param name="weekNumber">The ISO week number.</param>
        /// <returns>List of time tracking entries or NoContent if none found.</returns>
        [HttpGet("by-week")]
        public async Task<IActionResult> GetTimeTrackingByWeek(
            [FromQuery] Guid employeeId,
            [FromQuery] int year,
            [FromQuery] int weekNumber)
        {
                var query = new TimeTrackingByPersonAndWeek.Query(employeeId, year, weekNumber);
                var result = await _mediator.Send(query);

                if (result == null || result.Count == 0)
                {
                    return NoContent();
                }

                return Ok(result);
        }
    }
}
