using Halood.Domain.Entities;
using Halood.Domain.Interfaces.User;
using Halood.Domain.Interfaces.UserSatisfaction;
using Halood.Repository.EntityFramework;

namespace Halood.Repository;

public class UserSatisfactionRepository : BaseRepository<UserSatisfaction>, IUserSatisfactionRepository
{
    public UserSatisfactionRepository(HaloodDbContext context) : base(context)
    {
    }
}
