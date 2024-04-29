using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Halood.Repository.EntityFramework;

public static class DataMigrationConfiguration
{
    public static void Initialize(this IServiceCollection services)
    {
        //if (!env.IsProduction()) return;

        var context = services.BuildServiceProvider().GetService<HaloodDbContext>();
        context.Database.Migrate();
    }
}
