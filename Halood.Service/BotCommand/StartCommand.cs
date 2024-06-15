using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Interfaces.BotAction;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Halood.Service.BotCommand;

public class StartCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<StartCommand> _logger;
    private string _text =
        $"به حالود خوش آمدید! \n\n" +
        $"این بات به شما کمک می‌کند تا بتوانید میزان رضایت از زندگی خود، احساس‌ها و افکاری که در طول روز تجربه می‌کنید را ثبت کنید.\n\n" +
        $"با استفاده از این بات می‌توانید با خودتان بیش‌تر آشنا شوید.\n\n" +
        $"برای استفاده از بات، می‌توانید از گزینه Menu، که در پایین وجود دارد، استفاده کنید.";
    public StartCommand(ITelegramBotClient botClient, ILogger<StartCommand> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }
    
    public async Task Execute(BotCommandMessage message, CancellationToken cancellationToken)
    {
        CommandHandler.RemoveCommand(message.Username);

        await _botClient.SendChatActionAsync(
            chatId: message.ChatId,
            chatAction: ChatAction.Typing,
            cancellationToken: cancellationToken);

        // Simulate longer running task
        await Task.Delay(1000, cancellationToken);

        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            cancellationToken: cancellationToken);
    }

    public Task Execute(CallbackQuery callBackQuery, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
