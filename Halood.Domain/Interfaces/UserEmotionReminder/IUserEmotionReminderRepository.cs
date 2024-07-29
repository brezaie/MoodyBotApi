using Halood.Domain.Interfaces.Base;

namespace Halood.Domain.Interfaces.UserEmotionReminder;

public interface IUserEmotionReminderRepository : IBaseRepository<Entities.UserEmotionReminder>
{
    Task DisableValidRemindersAsync(long userId);
    Task<List<Entities.UserEmotionReminder>> GetValidUserEmotionRemindersAsync();
}
