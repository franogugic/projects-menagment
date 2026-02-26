using projects_menagment.Domain.Entities;

namespace projects_menagment.Application.Interfaces.Repositories;

public interface IPlanRepository
{
    Task<Plan?> GetByIdAsync(Guid planId, CancellationToken cancellationToken);
}
