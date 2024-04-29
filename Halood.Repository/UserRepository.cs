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

    public async Task<User?> GetByAsync(string username)
    {
        return await Context.Users.FirstOrDefaultAsync(x => x.Username == username);
    }
}
