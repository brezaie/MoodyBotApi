using Halood.Domain.Interfaces.Base;

namespace Halood.Domain.Interfaces.User;

public interface IUserRepository : IBaseRepository<Entities.User>
{
    Task<Entities.User?> GetByAsync(string username);
}
