using Microsoft.EntityFrameworkCore;

namespace projects_menagment.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
}
