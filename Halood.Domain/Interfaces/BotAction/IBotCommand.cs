using Halood.Domain.Dtos;

namespace Halood.Domain.Interfaces.BotAction;

public interface IBotCommand
{
    Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken);
}
