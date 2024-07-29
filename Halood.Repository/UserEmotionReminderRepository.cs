using Halood.Domain.Entities;
using Halood.Domain.Interfaces.UserEmotionReminder;
using Halood.Repository.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Halood.Repository;

public class UserEmotionReminderRepository : BaseRepository<UserEmotionReminder>, IUserEmotionReminderRepository
{
    public UserEmotionReminderRepository(HaloodDbContext context) : base(context)
    {
    }

    public async Task DisableValidRemindersAsync(long userId)
    {
        var lst = await Context.UserEmotionReminders
            .Where(x => x.UserId == userId && x.ValidTo == null)
            .ToListAsync();
        foreach (var item in lst)
        {
            item.ValidTo = DateTimeOffset.Now;
        }
    }

    public async Task<List<UserEmotionReminder>> GetValidUserEmotionRemindersAsync()
    {
        return await Context.UserEmotionReminders.Where(x => x.ValidTo == null).ToListAsync();
    }
}
