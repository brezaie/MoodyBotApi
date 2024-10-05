using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Entities;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Halood.Domain.Interfaces.User;
using Halood.Domain.Interfaces.UserSatisfaction;
using Halood.Service.BotCommand;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Halood.Service.BotReply;

public class RecordSatisfactionReply : IBotReply
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<NoCommand> _logger;
    private string _text = string.Empty;
    private readonly IUserRepository _userRepository;
    private readonly IUserSatisfactionRepository _userSatisfactionRepository;

    public RecordSatisfactionReply(ITelegramBotClient botClient, ILogger<NoCommand> logger,
        IUserRepository userRepository, IUserSatisfactionRepository userSatisfactionRepository)
    {
        _botClient = botClient;
        _logger = logger;
        _userRepository = userRepository;
        _userSatisfactionRepository = userSatisfactionRepository;
    }

    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        var satisfactions = CommandHandler.GetSatisfactionLevelInlineKeyboardMarkup();

        var givenSatisfaction = message.Text.Split(" ")[1];
        var satisfactionLevel = ((SatisfactionLevel[]) Enum.GetValues(typeof(SatisfactionLevel)))
            .FirstOrDefault(x => x.ToString() == givenSatisfaction);

        var userId = (await _userRepository.GetByAsync(message.Username)).Id;
        await _userSatisfactionRepository.SaveAsync(new UserSatisfaction
        {
            RegistrationDate = message.Date,
            SatisfactionNumber = (int)satisfactionLevel,
            UserId = userId
        });
        await _userSatisfactionRepository.CommitAsync();

        _text = $"گزینه \"{satisfactionLevel.GetDescription()}\" بعنوان میزان رضایت از زندگی امروزتان با موفقیت ثبت شد. 👍";

        InlineKeyboardButton reply = null;

        foreach (var satLevel in satisfactions.InlineKeyboard)
        {
            foreach (var row in satLevel)
            {
                if(row.Text == satisfactionLevel.GetDescription())
                {
                    if (row.Text != satisfactionLevel.GetDescription()) continue;

                    row.Text = $"{row.Text} ✅";
                    reply = row;
                    break;
                }
            }
        }

        await _botClient.EditMessageTextAsync(message.ChatId, message.CommandMessageId,
            text: _text,
            cancellationToken: cancellationToken
        );
    }
}
