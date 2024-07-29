using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Halood.Repository.EntityFramework;

public class HaloodDbContextFactory : IDesignTimeDbContextFactory<HaloodDbContext>
{
    public HaloodDbContext CreateDbContext(string[] args)
    {
        var connectionString =
        //@"Server=148.251.235.23;Database=haloodli_tracker;User Id=haloodli_admin;Password=k(kD7}iOwW_i0)0i;Integrated Security=False;Trusted_Connection=False;MultipleActiveResultSets=true";
        "Server=.;Database=Halood;Trusted_Connection=True;MultipleActiveResultSets=true";
        var optionsBuilder = new DbContextOptionsBuilder<HaloodDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        return new HaloodDbContext(optionsBuilder.Options);
    }
}
