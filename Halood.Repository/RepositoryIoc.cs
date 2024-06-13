using Halood.Domain.Interfaces.User;
using Halood.Domain.Interfaces.UserEmotion;
using Halood.Domain.Interfaces.UserSatisfaction;
using Halood.Repository.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Halood.Repository;

public static class RepositoryIoc
{
    public static IServiceCollection AddRepositoryDependencies(this IServiceCollection services,
            IConfiguration configuration)
    {

        services.AddDbContext<HaloodDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IUserSatisfactionRepository, UserSatisfactionRepository>();
        services.AddTransient<IUserEmotionRepository, UserEmotionRepository>();

        //services.Initialize();

        return services;
    }
}
