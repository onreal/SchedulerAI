using Microsoft.AspNetCore.Mvc;
using SchedulerApi.Application.Attributes;
using SchedulerApi.Application.Shared;
using SchedulerApi.Application.Template.Contracts;
using SchedulerApi.Application.Template.DTOs;

namespace SchedulerApi.Controllers.v1;

[ApiController]
[Route("templates")]
public class TemplateController(
    ICreateTemplateService createService,
    IUpdateTemplateService updateService,
    IDeleteTemplateService deleteService,
    IGetTemplateService getService)
    : ControllerBase
{
    /// <summary>
    /// Creates a new template.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key.
    /// </remarks>
    /// <param name="request">The request payload containing template information.</param>
    /// <response code="201">Template created successfully.</response>
    /// <response code="400">Invalid request payload.</response>
    /// <response code="401">API key is missing or invalid.</response>
    [HttpPost]
    [RequireApiKey]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] CreateTemplateRequest request)
    {
        await createService.CreateAsync(request);
        return Created(string.Empty, null);
    }

    /// <summary>
    /// Retrieves a template by its unique identifier.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key.
    /// </remarks>
    /// <param name="id">The GUID identifier of the template.</param>
    /// <response code="200">Returns the template details.</response>
    /// <response code="404">Template with specified ID not found.</response>
    /// <response code="401">API key is missing or invalid.</response>
    [HttpGet("{id:guid}")]
    [RequireApiKey]
    [ProducesResponseType(typeof(GetTemplate), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var template = await getService.GetByIdAsync(id);
        return template == null ? NotFound() : Ok(template);
    }

    /// <summary>
    /// Retrieves all templates for the current authenticated user.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key.
    /// </remarks>
    /// <response code="200">Returns the list of templates.</response>
    /// <response code="401">API key is missing or invalid.</response>
    [HttpGet]
    [RequireApiKey]
    [ProducesResponseType(typeof(IEnumerable<GetTemplate>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    public async Task<IActionResult> List()
    {
        var templates = await getService.GetAllAsync();
        return Ok(templates);
    }

    /// <summary>
    /// Updates an existing template by its ID.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key.
    /// </remarks>
    /// <param name="id">The GUID identifier of the template to update.</param>
    /// <param name="request">The updated template payload.</param>
    /// <response code="204">Template updated successfully.</response>
    /// <response code="400">Invalid update request payload.</response>
    /// <response code="404">Template with specified ID not found.</response>
    /// <response code="401">API key is missing or invalid.</response>
    [HttpPatch("{id:guid}")]
    [RequireApiKey]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTemplateRequest request)
    {
        var existing = await getService.GetByIdAsync(id);
        if (existing == null) return NotFound();

        await updateService.UpdateAsync(id, request);
        return NoContent();
    }

    /// <summary>
    /// Deletes a template by its unique identifier.
    /// </summary>
    /// <remarks>
    /// Requires a valid API key in the header: X-Api-Key.
    /// </remarks>
    /// <param name="id">The GUID identifier of the template to delete.</param>
    /// <response code="204">Template deleted successfully.</response>
    /// <response code="404">Template with specified ID not found.</response>
    /// <response code="401">API key is missing or invalid.</response>
    [HttpDelete("{id:guid}")]
    [RequireApiKey]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existing = await getService.GetByIdAsync(id);
        if (existing == null) return NotFound();

        await deleteService.DeleteAsync(id);
        return NoContent();
    }
}
