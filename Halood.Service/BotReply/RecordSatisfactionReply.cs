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
        // اگر متن وارد شده، هیچ یک از ایموجی های پیشنهادی نبود
        if (((SatisfactionLevel[])Enum.GetValues(typeof(SatisfactionLevel))).All(x =>
                x.ToString() != givenSatisfaction))
        {
            _text = $"گزینه انتخاب شده نادرست می‌باشد. لطفاً یکی از گزینه‌های پیشنهادی را انتخاب کنید.";
            await _botClient.SendTextMessageAsync(
                chatId: message.ChatId,
                text: _text,
                replyMarkup: satisfactions,
                cancellationToken: cancellationToken);
            return;
        }

        var userId = (await _userRepository.GetByAsync(message.Username)).Id;
        var lastUserSatisfaction = await _userSatisfactionRepository.GetLastUserSatisfactionAsync(userId);

        if (!CommandHandler.SpecialUserNames.Contains(message.Username) &&
            lastUserSatisfaction is not null &&
            (message.Date - lastUserSatisfaction.RegistrationDate).TotalMinutes <= 60)
        {
            _text =
                $"از آخرین دفعه که میزان رضایت خود را ثبت کرده‌اید، کم‌تر از 1 ساعت گذشته است. پس از گذشت این زمان می‌توانید مجدد رضایت خود را ثبت کنید 🙂";
            await _botClient.SendTextMessageAsync(
                chatId: message.ChatId,
                text: _text,
                cancellationToken: cancellationToken);
            return;
        }

        var satisfactionLevel = ((SatisfactionLevel[]) Enum.GetValues(typeof(SatisfactionLevel)))
            .FirstOrDefault(x => x.ToString() == givenSatisfaction);

        await _userSatisfactionRepository.SaveAsync(new UserSatisfaction
        {
            RegistrationDate = message.Date,
            SatisfactionNumber = (int)satisfactionLevel,
            UserId = userId
        });
        await _userSatisfactionRepository.CommitAsync();

        _text = $"گزینه \"{satisfactionLevel.GetDescription()}\" بعنوان میزان رضایت از زندگی امروزتان با موفقیت ثبت شد. 👍";

        foreach (var satLevel in satisfactions.InlineKeyboard)
        {
            foreach (var row in satLevel)
            {
                if(row.Text == satisfactionLevel.GetDescription())
                {
                    if (row.Text != satisfactionLevel.GetDescription()) continue;

                    row.Text = $"{row.Text} ✅";
                    break;
                }
            }
        }

        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            replyMarkup: satisfactions,
            cancellationToken: cancellationToken);
    }
}
