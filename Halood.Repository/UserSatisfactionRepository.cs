using Halood.Domain.Entities;
using Halood.Domain.Interfaces.User;
using Halood.Domain.Interfaces.UserSatisfaction;
using Halood.Repository.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Halood.Repository;

public class UserSatisfactionRepository : BaseRepository<UserSatisfaction>, IUserSatisfactionRepository
{
    public UserSatisfactionRepository(HaloodDbContext context) : base(context)
    {
    }

    public Task<UserSatisfaction?> GetLastUserSatisfactionAsync(long userId) =>
        Context.UserSatisfactions
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.RegistrationDate)
            .FirstOrDefaultAsync();
}
