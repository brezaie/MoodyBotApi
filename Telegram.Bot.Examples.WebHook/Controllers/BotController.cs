using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Services;
using Telegram.Bot.Types;

namespace Telegram.Bot.Controllers;

public class BotController : ControllerBase
{
    [HttpPost]
    //[ValidateTelegramBot]
    public async Task<IActionResult> Post(
        [FromBody] Update update,
        [FromServices] UpdateHandlers handleUpdateService,
        CancellationToken cancellationToken)
    {
        await handleUpdateService.HandleUpdateAsync(update, cancellationToken);

        await System.IO.File.WriteAllTextAsync("error.log", "Message Received", cancellationToken);

        return Ok();
    }
}
