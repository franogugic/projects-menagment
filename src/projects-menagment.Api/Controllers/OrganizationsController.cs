using Microsoft.AspNetCore.Mvc;
using projects_menagment.Api.Dtos.Common;
using projects_menagment.Api.Dtos.Organizations;
using projects_menagment.Application.Dtos.Organizations;
using projects_menagment.Application.Exceptions;
using projects_menagment.Application.Interfaces.Services;

namespace projects_menagment.Api.Controllers;

[ApiController]
[Route("api/organizations")]
public sealed class OrganizationsController(
    IOrganizationService organizationService,
    ILogger<OrganizationsController> logger) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateOrganizationResponseBodyDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateOrganizationRequestBodyDto? request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ValidationException("Request body is required.");
        }

        logger.LogInformation(
            "Processing organization create request for name {OrganizationName} by user {UserId}",
            request.Name,
            request.CreatedByUserId);

        var response = await organizationService.CreateAsync(
            new CreateOrganizationRequestDto(
                request.Name ?? string.Empty,
                request.PlanId,
                request.CreatedByUserId),
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            new CreateOrganizationResponseBodyDto(
                response.Id,
                response.Name,
                response.PlanId,
                response.CreatedByUserId,
                response.CreatedAt));
    }
}
