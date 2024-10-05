namespace Halood.Domain.Dtos;

public class BotCommandMessage
{
    public long ChatId { get; set; }
    public string? Username { get; set; }
    public string? Text { get; set; }
    public DateTime Date { get; set; }
    public int CommandMessageId { get; set; }
}
