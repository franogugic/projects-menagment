using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using projects_menagment.Application.Interfaces.Repositories;
using projects_menagment.Domain.Entities;
using projects_menagment.Infrastructure.Persistence;

namespace projects_menagment.Infrastructure.Repositories;

public sealed class PlanRepository(
    AppDbContext dbContext,
    ILogger<PlanRepository> logger) : IPlanRepository
{
    public async Task<Plan?> GetByIdAsync(Guid planId, CancellationToken cancellationToken)
    {
        logger.LogDebug("Fetching plan by id {PlanId}", planId);

        return await dbContext.Plans
            .AsNoTracking()
            .FirstOrDefaultAsync(plan => plan.Id == planId, cancellationToken);
    }
}
