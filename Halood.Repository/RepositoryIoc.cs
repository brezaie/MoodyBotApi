using Halood.Domain.Interfaces.User;
using Halood.Domain.Interfaces.UserEmotion;
using Halood.Domain.Interfaces.UserEmotionReminder;
using Halood.Domain.Interfaces.UserSatisfaction;
using Halood.Domain.Interfaces.UserThought;
using Halood.Repository.EntityFramework;
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
        services.AddTransient<IUserEmotionReminderRepository, UserEmotionReminderRepository>();
        services.AddTransient<IUserThoughtRepository, UserThoughtRepository>();

        //services.Initialize();

        return services;
    }
}
