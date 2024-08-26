using Halood.Domain.Entities;
using Halood.Domain.Interfaces.UserThought;
using Halood.Repository.EntityFramework;

namespace Halood.Repository;

public class UserThoughtRepository : BaseRepository<UserThought>, IUserThoughtRepository
{
    public UserThoughtRepository(HaloodDbContext context) : base(context)
    {
    }
}
