using Halood.Domain.Entities;
using Halood.Domain.Interfaces.User;
using Halood.Repository.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Halood.Repository;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(HaloodDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByAsync(string username, bool asNoTracking = true)
    {
        var query = Context.Users;
        if(asNoTracking)
            query.AsNoTracking();

        return await query.FirstOrDefaultAsync(x => x.Username == username);
    }

    public async Task UpdateAsync(User user)
    {
        var entity = await Context.Users
            .FirstOrDefaultAsync(x => x.Username == user.Username);

        if (entity == null)
            return;

        entity.IsGlobalSatisfactionReminderActive = user.IsGlobalSatisfactionReminderActive;
        entity.HasBlockedBot = user.HasBlockedBot;
    }

    public async Task<List<User>> GetSatisfactionRemindersAsync()
    {
        return await Context.Users.AsNoTracking().Where(x => x.IsGlobalSatisfactionReminderActive && x.ChatId > 0 && !x.HasBlockedBot)
            .ToListAsync();
    }
}
