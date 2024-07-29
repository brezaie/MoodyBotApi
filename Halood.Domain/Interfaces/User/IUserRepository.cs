using Halood.Domain.Interfaces.Base;

namespace Halood.Domain.Interfaces.User;

public interface IUserRepository : IBaseRepository<Entities.User>
{
    Task<Entities.User?> GetByAsync(string username, bool asNoTracking = true);
    Task UpdateAsync(Entities.User user);
    Task<List<Entities.User>> GetSatisfactionRemindersAsync();
}
