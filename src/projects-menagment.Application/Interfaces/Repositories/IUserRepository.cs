using projects_menagment.Domain.Entities;

namespace projects_menagment.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
}
