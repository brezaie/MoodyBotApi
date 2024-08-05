using Halood.Domain.Dtos;

namespace Halood.Domain.Interfaces.BotAction;

public interface IBotReply
{
    Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken);
}
