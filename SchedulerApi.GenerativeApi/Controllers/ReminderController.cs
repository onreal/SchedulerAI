using Microsoft.AspNetCore.Mvc;
using SchedulerApi.Application.Attributes;
using SchedulerApi.Application.Reminder.Contract;
using SchedulerApi.Application.Reminder.DTOs;
using SchedulerApi.Application.Shared;
using SchedulerApi.Application.Shared.Contracts;

namespace SchedulerApi.GenerativeApi.Controllers;

/// <summary>
/// Generate and execute reminders through prompts
/// </summary>
[ApiController]
[Route("reminders")]
public class ReminderController(IReminderService reminderService) : ControllerBase
{
    [HttpPost("generate")]
    [RequireApiKey]
    [ProducesResponseType(typeof(List<ReminderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> Register([FromBody] ReminderRequest request)
    {
        var result = await reminderService.GenerateAsync(request);
        return Ok(result);
    }
}