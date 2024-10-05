using Halood.Domain.Entities;
using Halood.Domain.Interfaces.UserSatisfaction;
using Halood.Repository.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Halood.Repository;

public class UserSatisfactionRepository : BaseRepository<UserSatisfaction>, IUserSatisfactionRepository
{
    public UserSatisfactionRepository(HaloodDbContext context) : base(context)
    {
    }

    public async Task<UserSatisfaction?> GetLastUserSatisfactionAsync(long userId) =>
        await Context.UserSatisfactions
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.RegistrationDate)
            .FirstOrDefaultAsync();

    public async Task<List<UserSatisfaction>> GetLastUserSatisfactionsByDaysAsync(long userId, int days)
    {
        return await Context.UserSatisfactions
            .Where(x => x.UserId == userId
                        && x.CreatedDate.Value.Date >= DateTime.Now.Date.AddDays(-days)
                        && x.CreatedDate.Value.Date < DateTime.Now.Date)
            .ToListAsync();
    }

    public async Task<int> GetTodayNumberOfSatisfactions()
    {
        return await Context.UserSatisfactions
            .Where(x => x.CreatedDate.Value.Date == DateTime.Now.Date)
            .CountAsync();
    }
}
