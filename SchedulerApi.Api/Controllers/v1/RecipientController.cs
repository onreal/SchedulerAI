using Microsoft.AspNetCore.Mvc;
using SchedulerApi.Application.Attributes;
using SchedulerApi.Application.Recipient.Contracts;
using SchedulerApi.Application.Recipient.DTOs;
using SchedulerApi.Application.Shared;

namespace SchedulerApi.Controllers.v1;

[ApiController]
[Route("recipients")]
public class RecipientController(
    ICreateRecipientService createService,
    IUpdateRecipientService updateService,
    IDeleteRecipientService deleteService,
    IGetRecipientService getService)
    : ControllerBase
{
    /// <summary>
    /// Creates a new recipient.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key
    /// </remarks>
    /// <param name="request">The recipient creation request containing Name, Surname, Phone, and Email.</param>
    /// <response code="201">Recipient created successfully.</response>
    /// <response code="400">Invalid request payload.</response>
    /// <response code="401">API key is missing or invalid.</response>
    [HttpPost]
    [RequireApiKey]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] CreateRecipientRequest request)
    {
        var recipient = await createService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = recipient.Id }, null);
    }

    /// <summary>
    /// Retrieves a recipient by its unique identifier.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key
    /// </remarks>
    /// <param name="id">The GUID identifier of the recipient.</param>
    /// <response code="200">Returns the recipient details.</response>
    /// <response code="404">Recipient with specified ID not found.</response>
    /// <response code="401">API key is missing or invalid.</response>
    [HttpGet("{id:guid}")]
    [RequireApiKey]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var recipient = await getService.GetByIdAsync(id);
        if (recipient == null) return NotFound();
        return Ok(recipient);
    }

    /// <summary>
    /// Retrieves the list of all recipients for the current authenticated user.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key
    /// </remarks>
    /// <response code="200">Returns the list of recipients.</response>
    /// <response code="401">API key is missing or invalid.</response>
    [HttpGet]
    [RequireApiKey]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll()
    {
        var recipients = await getService.GetAllAsync();
        return Ok(recipients);
    }

    /// <summary>
    /// Updates the details of a recipient identified by the given ID.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key
    /// </remarks>
    /// <param name="id">The GUID identifier of the recipient to update.</param>
    /// <param name="request">The update request with recipient details to change.</param>
    /// <response code="204">Recipient updated successfully, no content returned.</response>
    /// <response code="400">Invalid update request.</response>
    /// <response code="401">API key is missing or invalid.</response>
    /// <response code="404">Recipient with specified ID not found.</response>
    [HttpPatch("{id:guid}")]
    [RequireApiKey]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRecipientRequest request)
    {
        await updateService.UpdateAsync(id, request);
        return NoContent();
    }

    /// <summary>
    /// Deletes a recipient by its unique identifier.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key
    /// </remarks>
    /// <param name="id">The GUID identifier of the recipient to delete.</param>
    /// <response code="204">Recipient deleted successfully, no content returned.</response>
    /// <response code="401">API key is missing or invalid.</response>
    /// <response code="404">Recipient with specified ID not found.</response>
    [HttpDelete("{id:guid}")]
    [RequireApiKey]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await deleteService.DeleteAsync(id);
        return NoContent();
    }
}
