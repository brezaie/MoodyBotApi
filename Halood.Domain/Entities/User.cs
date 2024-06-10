namespace Halood.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Username { get; set; }
        public long ChatId { get; set; }
        public bool? IsBot { get; set; }
        public bool? IsPremium { get; set; }
        public string? LanguageCode { get; set; }
        public bool IsGlobalSatisfactionReminderActive { get; set; }

        public List<UserSatisfaction> UserSatisfactions { get; set; }
    }
}
