namespace Halood.Domain.Dtos;

public class BotActionMessage
{
    public long ChatId { get; set; }
    public string? Username { get; set; }
    public string? Text { get; set; }
    public DateTime Date { get; set; }
}
