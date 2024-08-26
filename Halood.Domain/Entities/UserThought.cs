namespace Halood.Domain.Entities;

public class UserThought : BaseEntity
{
    public long UserId { get; set; }
    public User User { get; set; }

    public string Text { get; set; }
    public DateTimeOffset RegistrationDate { get; set; }
}
