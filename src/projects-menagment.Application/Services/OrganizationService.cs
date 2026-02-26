using Microsoft.Extensions.Logging;
using projects_menagment.Application.Dtos.Organizations;
using projects_menagment.Application.Exceptions;
using projects_menagment.Application.Interfaces.Repositories;
using projects_menagment.Application.Interfaces.Services;
using projects_menagment.Domain.Entities;

namespace projects_menagment.Application.Services;

public sealed class OrganizationService(
    IUserRepository userRepository,
    IPlanRepository planRepository,
    IOrganizationRepository organizationRepository,
    ILogger<OrganizationService> logger) : IOrganizationService
{
    public async Task<CreateOrganizationResponseDto> CreateAsync(CreateOrganizationRequestDto request, CancellationToken cancellationToken)
    {
        var name = request.Name?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Organization name is required.");
        }

        if (name.Length > 150)
        {
            throw new ValidationException("Organization name must not exceed 150 characters.");
        }

        if (request.PlanId == Guid.Empty)
        {
            throw new ValidationException("Plan id is required.");
        }

        if (request.CreatedByUserId == Guid.Empty)
        {
            throw new ValidationException("Created by user id is required.");
        }

        var user = await userRepository.GetByIdAsync(request.CreatedByUserId, cancellationToken);
        if (user is null)
        {
            logger.LogWarning("Organization creation failed because creator user {UserId} was not found", request.CreatedByUserId);
            throw new NotFoundException("Creator user was not found.");
        }

        if (!user.IsActive)
        {
            logger.LogWarning("Organization creation denied because user {UserId} is inactive", user.Id);
            throw new ForbiddenException("User account is inactive.");
        }

        var plan = await planRepository.GetByIdAsync(request.PlanId, cancellationToken);
        if (plan is null)
        {
            logger.LogWarning("Organization creation failed because plan {PlanId} was not found", request.PlanId);
            throw new NotFoundException("Plan was not found.");
        }

        if (!plan.IsActive)
        {
            logger.LogWarning("Organization creation denied because plan {PlanId} is inactive", plan.Id);
            throw new ForbiddenException("Selected plan is inactive.");
        }

        var organization = Organization.Create(name, request.PlanId, request.CreatedByUserId);
        await organizationRepository.AddAsync(organization, cancellationToken);

        logger.LogInformation(
            "Organization {OrganizationId} created by user {UserId} with plan {PlanId}",
            organization.Id,
            organization.CreatedByUserId,
            organization.PlanId);

        return new CreateOrganizationResponseDto(
            organization.Id,
            organization.Name,
            organization.PlanId,
            organization.CreatedByUserId,
            organization.CreatedAt);
    }
}
