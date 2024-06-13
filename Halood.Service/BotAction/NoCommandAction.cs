using Halood.Common;
using Halood.Domain.Entities;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.User;
using Halood.Domain.Interfaces.UserSatisfaction;
using Microsoft.Extensions.Logging;
using Halood.Domain.Dtos;
using Telegram.Bot;
using Halood.Domain.Interfaces.BotAction;
using Halood.Domain.Interfaces.UserEmotion;

namespace Halood.Service.BotAction;

public class NoCommandAction : IBotAction
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<NoCommandAction> _logger;
    private string _text = string.Empty;
    private readonly IUserRepository _userRepository;
    private readonly IUserSatisfactionRepository _userSatisfactionRepository;
    private readonly IUserEmotionRepository _userEmotionRepository;

    delegate Task DoAction(BotActionMessage message, CancellationToken cancellationToken);

    private Dictionary<CommandType, DoAction> replyActions = new ();

    public NoCommandAction(ITelegramBotClient botClient, ILogger<NoCommandAction> logger,
        IUserRepository userRepository, IUserSatisfactionRepository userSatisfactionRepository,
        IUserEmotionRepository userEmotionRepository)
    {
        _botClient = botClient;
        _logger = logger;
        _userRepository = userRepository;
        _userSatisfactionRepository = userSatisfactionRepository;
        _userEmotionRepository = userEmotionRepository;

        replyActions.Add(CommandType.Unknown, ExecuteUnknownCommand);
        replyActions.Add(CommandType.Satisfaction, ExecuteSatisfactionCommandReply);
        replyActions.Add(CommandType.Emotion, ExecuteEmotionCommandReply);
        replyActions.Add(CommandType.ToggleReminder, ExecuteToggleReminderCommandReply);
    }

    public async Task Execute(BotActionMessage message, CancellationToken cancellationToken)
    {
        var previousCommand = CommandHandler.GetCommand(message.Username);
        await replyActions[previousCommand].Invoke(message, cancellationToken);
    }
    
    private async Task ExecuteUnknownCommand(BotActionMessage message, CancellationToken cancellationToken)
    {
        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: "دستوری برای اجرا انتخاب نشده است. لطفاً یکی از گزینه‌های زیر را انتخاب کنید.",
            replyMarkup: CommandHandler.MenuInlineKeyboardMarkup,
            cancellationToken: cancellationToken);
    }

    private async Task ExecuteSatisfactionCommandReply(BotActionMessage message, CancellationToken cancellationToken)
    {
        // اگر متن وارد شده، هیچ یک از ایموجی های پیشنهادی نبود
        if (((SatisfactionLevel[])Enum.GetValues(typeof(SatisfactionLevel))).All(x =>
                x.GetDescription() != message.Text))
        {
            _text = $"ایموجی انتخاب شده نادرست می‌باشد. لطفاً یکی از ایموجی‌های پیشنهادی را انتخاب کنید.";
            await _botClient.SendTextMessageAsync(
                chatId: message.ChatId,
                text: _text,
                replyMarkup: CommandHandler.SatisfactionLevelReplyKeyboardMarkup,
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

        await _userSatisfactionRepository.SaveAsync(new UserSatisfaction
        {
            RegistrationDate = message.Date,
            SatisfactionNumber = (int)((SatisfactionLevel[])Enum.GetValues(typeof(SatisfactionLevel)))
                .FirstOrDefault(x => x.GetDescription() == message.Text),
            UserId = userId
        });
        await _userSatisfactionRepository.CommitAsync();

        CommandHandler.RemoveCommand(message.Username);

        _text = "رضایت از زندگی این لحظه‌تان را با موفقیت ثبت کردید  👍";
        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            cancellationToken: cancellationToken);
    }

    private async Task ExecuteToggleReminderCommandReply(BotActionMessage message, CancellationToken cancellationToken)
    {
        // اگر متن وارد شده، هیچ یک از گزینه های پیشنهادی نبود
        if (((YesNoResponse[])Enum.GetValues(typeof(YesNoResponse))).All(x =>
                x.GetDescription() != message.Text))
        {
            _text = $"گزینه انتخاب شده نادرست می‌باشد. لطفاً یکی از گزینه‌های پیشنهادی را انتخاب کنید.";
            await _botClient.SendTextMessageAsync(
                chatId: message.ChatId,
                text: _text,
                replyMarkup: CommandHandler.ReminderToggleInlineKeyboardMarkup,
                cancellationToken: cancellationToken);
            return;
        }

        if (message.Text == YesNoResponse.No.GetDescription())
        {
            _text = $"دستور مورد نظر لغو گردید.";
            await _botClient.SendTextMessageAsync(
                chatId: message.ChatId,
                text: _text,
                cancellationToken: cancellationToken);

            CommandHandler.RemoveCommand(message.Username);

            return;
        }

        var user = await _userRepository.GetByAsync(message.Username);
        user.IsGlobalSatisfactionReminderActive = !user.IsGlobalSatisfactionReminderActive;
        await _userRepository.UpdateAsync(user);
        await _userRepository.CommitAsync();

        CommandHandler.RemoveCommand(message.Username);

        _text = $"{(user.IsGlobalSatisfactionReminderActive ? "فعال‌سازی" : "غیرفعال‌سازی")} با موفقت انجام شد.👍";
        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            cancellationToken: cancellationToken);
    }

    private async Task ExecuteEmotionCommandReply(BotActionMessage message, CancellationToken cancellationToken)
    {
        // اگر متن وارد شده، هیچ یک از احساس های پیشنهادی نبود
        if (((Emotion[])Enum.GetValues(typeof(Emotion))).All(x =>
                x.GetDescription() != message.Text))
        {
            _text = $"احساس انتخاب شده نادرست می‌باشد. لطفاً یکی از احساس‌های پیشنهادی را انتخاب کنید.";
            await _botClient.SendTextMessageAsync(
                chatId: message.ChatId,
                text: _text,
                replyMarkup: CommandHandler.EmotionReplyKeyboardMarkup,
                cancellationToken: cancellationToken);
            return;
        }

        var userId = (await _userRepository.GetByAsync(message.Username)).Id;
        var lastUserEmotion = await _userEmotionRepository.GetLastUserEmotionAsync(userId);

        if (!CommandHandler.SpecialUserNames.Contains(message.Username) &&
            lastUserEmotion is not null &&
            (message.Date - lastUserEmotion.RegistrationDate).TotalMinutes <= 60)
        {
            _text =
                $"از آخرین دفعه که احساس خود را ثبت کرده‌اید، کم‌تر از 1 ساعت گذشته است. پس از گذشت این زمان می‌توانید مجدد احساس خود را ثبت کنید 🙂";
            await _botClient.SendTextMessageAsync(
                chatId: message.ChatId,
                text: _text,
                cancellationToken: cancellationToken);
            return;
        }

        await _userEmotionRepository.SaveAsync(new UserEmotion
        {
            UserId = userId,
            RegistrationDate = message.Date,
            EmotionText = ((Emotion[])Enum.GetValues(typeof(Emotion)))
                .FirstOrDefault(x =>
                    x.GetDescription() == message.Text).ToString()

        });

        await _userEmotionRepository.CommitAsync();

        CommandHandler.RemoveCommand(message.Username);

        _text = "احساس این لحظه‌تان را با موفقیت ثبت کردید  👍";
        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            cancellationToken: cancellationToken);
    }
}
