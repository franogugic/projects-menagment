using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using projects_menagment.Application.Interfaces.Repositories;
using projects_menagment.Application.Interfaces.Security;
using projects_menagment.Infrastructure.Persistence;
using projects_menagment.Infrastructure.Repositories;
using projects_menagment.Infrastructure.Security;

namespace projects_menagment.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IPlanRepository, PlanRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddSingleton<IAuthTokenService, JwtTokenService>();

        return services;
    }
}
