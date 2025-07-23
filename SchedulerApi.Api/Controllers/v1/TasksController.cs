using Microsoft.AspNetCore.Mvc;
using SchedulerApi.Application.Attributes;
using SchedulerApi.Application.Tasks.Contracts;

namespace SchedulerApi.Controllers.v1;

/// <summary>
/// Execute Scheduler Integrations.
/// </summary>
[ApiController]
[Route("tasks")]
public class TasksController(ITaskService taskService) : ControllerBase
{
    /// <summary>
    /// Runs a scheduler task by providing the schedule ID.
    /// </summary>
    /// <param name="scheduleId">The unique identifier of the schedule to run.</param>
    /// <returns>Returns no content if the task is executed successfully, or not found if the schedule does not exist.</returns>
    /// <response code="204">Task executed successfully.</response>
    /// <response code="404">Schedule does not exist.</response>
    [HttpGet("{scheduleId:guid}/run")]
    [RequireApiKey]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RunTask(Guid scheduleId)
    {
        await taskService.RunAsync(scheduleId);

        return NoContent();
    }
}