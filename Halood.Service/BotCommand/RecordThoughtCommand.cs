using Halood.Domain.Dtos;
using Halood.Domain.Interfaces.BotAction;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Halood.Service.BotCommand;

public class RecordThoughtCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private string _text =
        $"چه موضوعی ذهن شما را درگیر کرده؟ آن را یادداشت کنید.";

    public RecordThoughtCommand(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }


    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            replyMarkup: new InlineKeyboardMarkup(
                new List<InlineKeyboardButton>
                {
                    
                }

                ),
            
            cancellationToken: cancellationToken);
    }
}
