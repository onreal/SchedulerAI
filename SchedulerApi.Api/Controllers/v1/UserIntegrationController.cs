using Microsoft.AspNetCore.Mvc;
using SchedulerApi.Application.Attributes;
using SchedulerApi.Application.Shared;
using SchedulerApi.Application.UserIntegration.Contracts;
using SchedulerApi.Application.UserIntegration.DTOs;

namespace SchedulerApi.Controllers.v1;

[ApiController]
[Route("userIntegrations")]
public class UserIntegrationController(
    ICreateUserIntegrationService createUserIntegrationService,
    IGetUserIntegrationService getUserIntegrationService,
    IDeleteUserIntegrationService deleteUserIntegrationService,
    IUpdateUserIntegrationService updateUserIntegrationService)
    : ControllerBase
{
    /// <summary>
    /// Creates a new user integration.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key.
    /// </remarks>
    /// <param name="request">The request payload containing integration type and settings.</param>
    /// <response code="201">User integration created successfully.</response>
    /// <response code="400">Invalid request payload.</response>
    /// <response code="401">API key is missing or invalid.</response>
    [HttpPost]
    [RequireApiKey]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] CreateUserIntegrationRequest request)
    {
        var template = await createUserIntegrationService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = template.Id }, null); // Note: Consider returning the real integration ID here
    }

    /// <summary>
    /// Retrieves a user integration by its unique identifier.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key.
    /// </remarks>
    /// <param name="id">The GUID identifier of the user integration.</param>
    /// <response code="200">Returns the user integration details.</response>
    /// <response code="404">User integration with specified ID not found.</response>
    /// <response code="401">API key is missing or invalid.</response>
    [HttpGet("{id:guid}")]
    [RequireApiKey]
    [ProducesResponseType(typeof(GetUserIntegration), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await getUserIntegrationService.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Retrieves all user integrations for the current authenticated user.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key.
    /// </remarks>
    /// <response code="200">Returns the list of user integrations.</response>
    /// <response code="401">API key is missing or invalid.</response>
    [HttpGet]
    [RequireApiKey]
    [ProducesResponseType(typeof(IEnumerable<GetUserIntegration>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    public async Task<IActionResult> List()
    {
        var result = await getUserIntegrationService.GetAllAsync();
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing user integration by its ID.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key.
    /// </remarks>
    /// <param name="id">The GUID identifier of the user integration to update.</param>
    /// <param name="request">The updated integration settings.</param>
    /// <response code="204">User integration updated successfully.</response>
    /// <response code="400">Invalid update request payload.</response>
    /// <response code="404">User integration with specified ID not found.</response>
    /// <response code="401">API key is missing or invalid.</response>
    [HttpPatch("{id:guid}")]
    [RequireApiKey]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserIntegrationRequest request)
    {
        var exists = await getUserIntegrationService.GetByIdAsync(id);
        if (exists == null) return NotFound();

        await updateUserIntegrationService.UpdateAsync(id, request);
        return NoContent();
    }

    /// <summary>
    /// Deletes a user integration by its unique identifier.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key.
    /// </remarks>
    /// <param name="id">The GUID identifier of the user integration to delete.</param>
    /// <response code="204">User integration deleted successfully.</response>
    /// <response code="404">User integration with specified ID not found.</response>
    /// <response code="401">API key is missing or invalid.</response>
    [HttpDelete("{id:guid}")]
    [RequireApiKey]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var exists = await getUserIntegrationService.GetByIdAsync(id);
        if (exists == null) return NotFound();

        await deleteUserIntegrationService.DeleteAsync(id);
        return NoContent();
    }
}
