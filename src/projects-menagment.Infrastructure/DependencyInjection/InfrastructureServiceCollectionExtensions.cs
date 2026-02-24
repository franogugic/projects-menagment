using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using projects_menagment.Application.Interfaces.Repositories;
using projects_menagment.Application.Interfaces.Security;
using projects_menagment.Infrastructure.Persistence;
using projects_menagment.Infrastructure.Repositories;
using projects_menagment.Infrastructure.Security;

namespace projects_menagment.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();

        return services;
    }
}
