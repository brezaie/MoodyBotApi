namespace Halood.Domain.Entities;

public class AppSetting : BaseEntity
{
    public string WelcomeMessage { get; set; }
    public TimeSpan? DefaultSatisfactionScheduler { get; set; }
}
