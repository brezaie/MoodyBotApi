using Halood.Common;
using Halood.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Halood.Service.BotAction;

public class StartCommandAction : IBotAction
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<StartCommandAction> _logger;
    private string _text =
        $"به حالود خوش آمدید! \n" +
        $"این بات به شما کمک می‌کند تا بتوانید میزان رضایت از زندگی خود، احساس‌ها و افکاری که در طول روز تجربه می‌کنید را ثبت کنید.\n" +
        $"با استفاده از این بات می‌توانید با خودتان بیش‌تر آشنا شوید.\n" +
        $"برای استفاده از بات، می‌توانید از گزینه Menu، که در پایین وجود دارد، استفاده کنید.";
    public StartCommandAction(ITelegramBotClient botClient, ILogger<StartCommandAction> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }
    
    public async Task Execute(Message message, CancellationToken cancellationToken)
    {
        CommandHandler.RemoveCommand(message.Chat.Username);

        await _botClient.SendChatActionAsync(
            chatId: message.Chat.Id,
            chatAction: ChatAction.Typing,
            cancellationToken: cancellationToken);

        // Simulate longer running task
        await Task.Delay(500, cancellationToken);

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: _text,
            cancellationToken: cancellationToken);
    }
}