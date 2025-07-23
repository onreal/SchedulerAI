using Microsoft.AspNetCore.Mvc;
using SchedulerApi.Application.Attributes;
using SchedulerApi.Application.Schedule.Contracts;
using SchedulerApi.Application.Schedule.DTOs;
using SchedulerApi.Application.Shared;

namespace SchedulerApi.Controllers.v1;

[ApiController]
[Route("schedule")]
public class ScheduleController(
    ICreateScheduleService createService,
    IGetScheduleService getService,
    IUpdateScheduleService updateService,
    IDeleteScheduleService deleteService)
    : ControllerBase
{
    /// <summary>
    /// Creates a new schedule.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key.
    /// </remarks>
    /// <param name="request">The request payload for the new schedule.</param>
    /// <response code="201">Schedule created successfully.</response>
    /// <response code="400">Invalid request payload.</response>
    /// <response code="401">API key is missing or invalid.</response>
    [HttpPost]
    [RequireApiKey]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] CreateScheduleRequest request)
    {
        var schedule = await createService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = schedule.Id }, null);
    }

    /// <summary>
    /// Retrieves a schedule by its unique identifier.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key.
    /// </remarks>
    /// <param name="id">The GUID identifier of the schedule.</param>
    /// <response code="200">Returns the schedule details.</response>
    /// <response code="404">Schedule with specified ID not found.</response>
    /// <response code="401">API key is missing or invalid.</response>
    [HttpGet("{id:guid}")]
    [RequireApiKey]
    [ProducesResponseType(typeof(GetSchedule), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await getService.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Lists all schedules for the current authenticated user.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key.
    /// </remarks>
    /// <response code="200">Returns the list of schedules.</response>
    /// <response code="401">API key is missing or invalid.</response>
    [HttpGet]
    [RequireApiKey]
    [ProducesResponseType(typeof(IEnumerable<GetSchedule>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    public async Task<IActionResult> List()
    {
        var result = await getService.GetAllAsync();
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing schedule by its ID.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key.
    /// </remarks>
    /// <param name="id">The GUID identifier of the schedule to update.</param>
    /// <param name="request">The updated schedule data.</param>
    /// <response code="204">Schedule updated successfully.</response>
    /// <response code="400">Invalid update request.</response>
    /// <response code="404">Schedule with specified ID not found.</response>
    /// <response code="401">API key is missing or invalid.</response>
    [HttpPatch("{id:guid}")]
    [RequireApiKey]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateScheduleRequest request)
    {
        var exists = await getService.GetByIdAsync(id);
        if (exists == null) return NotFound();

        await updateService.UpdateAsync(id, request);
        return NoContent();
    }

    /// <summary>
    /// Deletes a schedule by its unique identifier.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key.
    /// </remarks>
    /// <param name="id">The GUID identifier of the schedule to delete.</param>
    /// <response code="204">Schedule deleted successfully.</response>
    /// <response code="404">Schedule with specified ID not found.</response>
    /// <response code="401">API key is missing or invalid.</response>
    [HttpDelete("{id:guid}")]
    [RequireApiKey]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var exists = await getService.GetByIdAsync(id);
        if (exists == null) return NotFound();

        await deleteService.DeleteAsync(id);
        return NoContent();
    }
}
