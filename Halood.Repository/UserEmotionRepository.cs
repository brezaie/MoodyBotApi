using Halood.Domain.Entities;
using Halood.Domain.Interfaces.UserEmotion;
using Halood.Repository.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Halood.Repository;

public class UserEmotionRepository : BaseRepository<UserEmotion>, IUserEmotionRepository
{
    public UserEmotionRepository(HaloodDbContext context) : base(context)
    {
    }

    public async Task<UserEmotion?> GetLastUserEmotionAsync(long userId)
    {
        return await Context.UserEmotions
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.RegistrationDate)
            .FirstOrDefaultAsync();
    }
}
