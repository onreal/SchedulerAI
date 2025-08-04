using Microsoft.AspNetCore.Mvc;
using SchedulerApi.Application.Agents.ScheduleParser.DTOs;
using SchedulerApi.Application.Attributes;
using SchedulerApi.Application.Reminder.Contract;
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
    public async Task<IActionResult> Generate([FromBody] ReminderRequest request)
    {
        var result = await reminderService.GenerateAsync(request);
        return Ok(result);
    }
    
    [HttpPost("generate/schedule")]
    [RequireApiKey]
    [ProducesResponseType(typeof(List<ReminderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> GenerateSchedule([FromBody] ReminderRequest request)
    {
        var result = await reminderService.GenerateAsync(request);
        return Ok(result);
    }
}