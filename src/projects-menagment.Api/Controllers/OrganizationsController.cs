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

    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<UserOrganizationResponseBodyDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByUserId(Guid userId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing organizations fetch for user {UserId}", userId);

        var organizations = await organizationService.GetByUserIdAsync(userId, cancellationToken);
        var response = organizations
            .Select(item => new UserOrganizationResponseBodyDto(
                item.OrganizationId,
                item.Name,
                item.PlanId,
                item.CreatedByUserId,
                item.CreatedAt,
                item.Role))
            .ToList();

        return Ok(response);
    }

    [HttpPost("{organizationId:guid}/members/invite")]
    [ProducesResponseType(typeof(InviteOrganizationMemberResponseBodyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> InviteMember(
        Guid organizationId,
        [FromBody] InviteOrganizationMemberRequestBodyDto? request,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ValidationException("Request body is required.");
        }

        var result = await organizationService.InviteMemberAsync(
            new InviteOrganizationMemberRequestDto(
                organizationId,
                request.InvitedByUserId,
                request.Email ?? string.Empty,
                request.Role),
            cancellationToken);

        return Ok(new InviteOrganizationMemberResponseBodyDto(
            result.InvitationId,
            result.Email,
            result.Role,
            result.ExpiresAt,
            result.InvitationLink));
    }

    [HttpPost("member-invitations/accept")]
    [ProducesResponseType(typeof(AcceptOrganizationInvitationResponseBodyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AcceptInvitation(
        [FromBody] AcceptOrganizationInvitationRequestBodyDto? request,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ValidationException("Request body is required.");
        }

        var result = await organizationService.AcceptInvitationAsync(
            new AcceptOrganizationInvitationRequestDto(request.Token ?? string.Empty),
            cancellationToken);

        return Ok(new AcceptOrganizationInvitationResponseBodyDto(
            result.OrganizationId,
            result.UserId,
            result.Role,
            result.Message));
    }
}
