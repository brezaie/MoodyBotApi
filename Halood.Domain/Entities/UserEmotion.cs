namespace Halood.Domain.Entities;

public class UserEmotion : BaseEntity
{
    public long UserId { get; set; }
    public User User { get; set; }

    public string EmotionText { get; set; }
    public DateTimeOffset RegistrationDate { get; set; }
}
