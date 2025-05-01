using MediatR;
using Microsoft.AspNetCore.Mvc;
using TimeTracking.Application.Activities;
using TimeTracking.Application.DTOs;
using TimeTracking.Application.Projects;

namespace Time_Tracking_App.Controllers
{
    /// <summary>
    /// Controller for managing project-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="projectDto">Project creation data.</param>
        /// <returns>The created project.</returns>
        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create([FromBody] CreateProjectDto projectDto)
        {
            var created = await _mediator.Send(new ProjectCreate.Command(projectDto));
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Gets a project by its ID.
        /// </summary>
        /// <param name="id">Project ID.</param>
        /// <returns>The project with the given ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetById(Guid id)
        {
            return await _mediator.Send(new ProjectById.Query(id));
        }

        /// <summary>
        /// Gets a list of all projects.
        /// </summary>
        /// <returns>List of project DTOs.</returns>
        [HttpGet]
        public async Task<ActionResult<List<ProjectDto>>> GetProjects()
        {
           return await _mediator.Send(new ProjectList.Query());
        }

        /// <summary>
        /// Updates an existing project.
        /// </summary>
        /// <param name="id">Project ID.</param>
        /// <param name="updateDto">Updated project data.</param>
        /// <returns>The updated project or NotFound if it doesn’t exist.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectDto>> Update(Guid id, [FromBody] CreateProjectDto updateDto)
        {
            var updated = await _mediator.Send(new ProjectUpdate.Command(id, updateDto));
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        /// <summary>
        /// Deletes a project by its ID.
        /// </summary>
        /// <param name="id">Project ID.</param>
        /// <returns>NoContent if deleted successfully; otherwise NotFound.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _mediator.Send(new ProjectDelete.Command(id));
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
