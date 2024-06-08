using Halood.Domain.Interfaces.Base;

namespace Halood.Domain.Interfaces.UserSatisfaction;

public interface IUserSatisfactionRepository : IBaseRepository<Entities.UserSatisfaction>
{
    Task<Entities.UserSatisfaction?> GetLastUserSatisfactionAsync(long userId);
}
