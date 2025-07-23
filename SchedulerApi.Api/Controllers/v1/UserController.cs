using Microsoft.AspNetCore.Mvc;
using SchedulerApi.Application.Attributes;
using SchedulerApi.Application.Shared;
using SchedulerApi.Application.User.Contracts;
using SchedulerApi.Application.User.DTOs;

namespace SchedulerApi.Controllers.v1;

[ApiController]
[Route("users")]
public class UserController(
    IRegisterUserService registerUserService,
    IUserUsageService userUsageService,
    IUpdateUserService updateUserService)
    : ControllerBase
{
    /// <summary>
    /// Registers a new user and returns a generated API key.
    /// </summary>
    /// <remarks>
    /// Provide user information in the body (name, surname, email, plan).
    /// This will create a user and return a new API key.
    /// </remarks>
    /// <param name="request">User registration data</param>
    /// <returns>API key</returns>
    /// <response code="200">Successfully created user and returned API key</response>
    /// <response code="400">Validation failed or input is invalid</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var result = await registerUserService.RegisterUserAsync(request);
        return Ok(result);
    }
    
    /// <summary>
    /// Updates the user's name, surname, and email.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key
    /// </remarks>
    /// <param name="request">The user update request.</param>
    /// <response code="200">User updated successfully</response>
    /// <response code="401">API key is missing or invalid</response>
    /// <response code="404">User not found</response>
    /// <response code="400">Invalid request</response>
    [HttpPatch("update")]
    [RequireApiKey]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
    {
        await updateUserService.UpdateUserAsync(request);
        return NoContent();
    }
    
    /// <summary>
    /// Get API usage for the current user.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key
    /// </remarks>
    /// <returns>Usage statistics</returns>
    /// <response code="200">Returns the current usage information</response>
    /// <response code="401">API key is missing or invalid</response>
    /// <response code="404">User not found or usage data missing</response>
    [HttpGet("usage")]
    [RequireApiKey]
    [ProducesResponseType(typeof(UsageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<ActionResult<UsageResponse>> GetUsage()
    {
        var usage = await userUsageService.GetUsageAsync();
        return Ok(usage);
    }
}