namespace Halood.Domain.Entities;

public class UserChat : BaseEntity
{
    public long UserId { get; set; }

    public string Text { get; set; }
    public string JsonChat { get; set; }
    public DateTimeOffset? RegistrationDate { get; set; }
}
