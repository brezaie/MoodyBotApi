using Halood.Domain.Dtos;

namespace Halood.Domain.Interfaces.BotAction;

public interface IBotReply
{
    Task Execute(BotCommandMessage message, CancellationToken cancellationToken);
}
