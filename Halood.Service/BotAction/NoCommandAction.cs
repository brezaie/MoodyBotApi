using Halood.Common;
using Halood.Domain.Entities;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces;
using Halood.Domain.Interfaces.User;
using Halood.Domain.Interfaces.UserSatisfaction;
using Microsoft.Extensions.Logging;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Halood.Service.BotAction;

public class NoCommandAction : IBotAction
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<NoCommandAction> _logger;
    private string _text = string.Empty;
    private readonly IUserRepository _userRepository;
    private readonly IUserSatisfactionRepository _userSatisfactionRepository;

    public NoCommandAction(ITelegramBotClient botClient, ILogger<NoCommandAction> logger, string text,
        IUserRepository userRepository, IUserSatisfactionRepository userSatisfactionRepository)
    {
        _botClient = botClient;
        _logger = logger;
        _text = text;
        _userRepository = userRepository;
        _userSatisfactionRepository = userSatisfactionRepository;
    }

    public async Task Execute(Message message, CancellationToken cancellationToken)
    {
        var previousCommand = CommandHandler.GetCommand(message.Chat.Username);
        switch (previousCommand)
        {
            case CommandType.Unknown:
                await ExecuteUnknownCommand(message, cancellationToken);
                break;
            case CommandType.Satisfaction:
                await ExecuteSatisfactionCommandReply(message, cancellationToken);
                break;
            case CommandType.Feeling:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task ExecuteUnknownCommand(Message message, CancellationToken cancellationToken)
    {
        _text = "یکی از گزینه های زیر رو انتخاب کن:\n" +
                "/start - شروع بات\n" +
                "/how_is_your_satisfaction - چقدر از امروز راضی بودی تا الان؟\n" +
                "/how_do_you_feel - الان چه احساسی داری؟";

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: _text,
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken);
    }

    private async Task ExecuteSatisfactionCommandReply(Message message, CancellationToken cancellationToken)
    {
        // اگر متن وارد شده، هیچ یک از ایموجی های پیشنهادی نبود
        if (((SatisfactionLevel[])Enum.GetValues(typeof(SatisfactionLevel))).All(x =>
                x.GetDescription() != message.Text))
        {
            _text = $"ایموجی انتخاب شده نادرست می‌باشد. لطفاً یکی از ایموجی‌های پیشنهادی را انتخاب کنید.";
            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: _text,
                replyMarkup: CommandHandler.SatisfactionLevelReplyKeyboardMarkup,
                cancellationToken: cancellationToken);
        }

        var userId = (await _userRepository.GetByAsync(message.Chat.Username)).Id;
        var lastUserSatisfaction = await _userSatisfactionRepository.GetLastUserSatisfactionAsync(userId);

        if (!CommandHandler.SpecialUserNames.Contains(message.Chat.Username) &&
            lastUserSatisfaction is not null &&
            (message.Date - lastUserSatisfaction.RegistrationDate).Minutes <= 60)
        {
            _text =
                $"از آخرین دفعه که میزان رضایت خود را ثبت کرده‌اید، کم‌تر از 1 ساعت گذشته است. پس از گذشت این زمان می‌توانید مجدد رضایت خود را ثبت کنید 🙂";
            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: _text,
                cancellationToken: cancellationToken);
        }

        await _userSatisfactionRepository.SaveAsync(new UserSatisfaction
        {
            RegistrationDate = message.Date,
            SatisfactionNumber = (int)((SatisfactionLevel[])Enum.GetValues(typeof(SatisfactionLevel)))
                .FirstOrDefault(x => x.GetDescription() == message.Text),
            UserId = userId
        });
        await _userSatisfactionRepository.CommitAsync();

        CommandHandler.RemoveCommand(message.Chat.Username);

        _text = "رضایت از زندگی این لحظه‌ات رو با موفقیت ثبت کردی  👍";
        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: _text,
            cancellationToken: cancellationToken);
    }
}
