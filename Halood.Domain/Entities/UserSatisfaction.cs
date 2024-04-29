namespace Halood.Domain.Entities;

public class UserSatisfaction : BaseEntity
{
    public long UserId { get; set; }
    public User User { get; set; }

    public int SatisfactionNumber { get; set; }
    public DateTimeOffset RegistrationDate { get; set; }
}
