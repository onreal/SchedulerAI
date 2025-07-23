using Microsoft.AspNetCore.Mvc;
using SchedulerApi.Application.Attributes;
using SchedulerApi.Domain.ValueObjects;

namespace SchedulerApi.Controllers.v1;

/// <summary>
/// Provides metadata about available integrations.
/// </summary>
[ApiController]
[Route("integrations")]
public class IntegrationController : ControllerBase
{
    /// <summary>
    /// Gets all available integration types.
    /// </summary>
    /// <remarks>
    /// Returns a list of supported integration types, such as Input, Generative, and Transactional.
    /// </remarks>
    /// <returns>A list of integration type names.</returns>
    /// <response code="200">Returns the list of integration types</response>
    [HttpGet]
    [RequireApiKey]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public ActionResult<Dictionary<string, List<string>>> GetGroupedIntegrations()
    {
        var grouped = Integration.All
            .GroupBy(i => i.Type)
            .ToDictionary(
                g => g.Key.ToString(), // Key: "Input", "Generative", etc.
                g => g.OrderBy(i => i.Order)
                    .Select(i => i.Name)
                    .ToList()
            );

        return Ok(grouped);
    }
}