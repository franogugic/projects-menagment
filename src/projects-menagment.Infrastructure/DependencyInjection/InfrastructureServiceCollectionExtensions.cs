using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using projects_menagment.Infrastructure.Persistence;

namespace projects_menagment.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}
