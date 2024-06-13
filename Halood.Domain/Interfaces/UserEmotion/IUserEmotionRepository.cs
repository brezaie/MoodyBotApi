using Halood.Domain.Interfaces.Base;

namespace Halood.Domain.Interfaces.UserEmotion;

public interface IUserEmotionRepository : IBaseRepository<Entities.UserEmotion>
{
    Task<Entities.UserEmotion?> GetLastUserEmotionAsync(long userId);
}
