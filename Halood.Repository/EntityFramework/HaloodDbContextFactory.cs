using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Halood.Repository.EntityFramework;

public class HaloodDbContextFactory : IDesignTimeDbContextFactory<HaloodDbContext>
{
    public HaloodDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            //"Server=tcp:tradersrating.database.windows.net,1433;Initial Catalog=tradersrating;Persist Security Info=False;User ID=tradersrating-admin;Password=15101370Hsn.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            "Server=.;Database=Halood;Trusted_Connection=True;MultipleActiveResultSets=true";
        var optionsBuilder = new DbContextOptionsBuilder<HaloodDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        return new HaloodDbContext(optionsBuilder.Options);
    }
}
