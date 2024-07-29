namespace Halood.Domain.Entities;

public class UserEmotionReminder : BaseEntity
{
    public long UserId { get; set; }
    public int Hour { get; set; }
    public DateTimeOffset? ValidTo { get; set; }
}
