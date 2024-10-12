using Halood.Common;
using Halood.Domain.Interfaces.User;
using Halood.Domain.Dtos;
using Halood.Domain.Entities;
using Halood.Domain.Enums;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using Halood.Domain.Interfaces.BotAction;
using Halood.Domain.Interfaces.UserEmotionReminder;
using Halood.Service.BotCommand;
using Halood.Service.BotReply;

namespace Telegram.Bot.Services;

public class UpdateHandlers
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<UpdateHandlers> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IUserEmotionReminderRepository _userEmotionReminderRepository;
    private readonly IBotCommand _howDoYouFeelCommand;
    private readonly IBotCommand _howIsYourSatisfactionCommand;
    private readonly IBotCommand _noCommand;
    private readonly IBotCommand _startCommand;
    private readonly IBotCommand _toggleSatisfactionReminderCommand;
    private readonly IBotCommand _changeSettingsCommand;
    private readonly IBotCommand _changeLanguageCommand;
    private readonly IBotCommand _generateReportCommand;
    private readonly IBotCommand _changeEmotionReminder;
    private readonly IBotCommand _recordThoughtCommand;
    private readonly IBotCommand _sendEmergencyMessageCommand;
    private readonly IBotCommand _faqCommand;

    private readonly IBotReply _recordSatisfactionReply;
    private readonly IBotReply _recordEmotionReply;
    private readonly IBotReply _changeLanguageReply;
    private readonly IBotReply _change_emotion_reminder_reply;
    private readonly IBotReply _toggleSatisfactionReminderReply;
    private readonly IBotReply _recordThoughtReply;
    private readonly IBotReply _faqReply;

    public UpdateHandlers(ITelegramBotClient botClient, ILogger<UpdateHandlers> logger, IUserRepository userRepository,
        IEnumerable<IBotCommand> botActions, IEnumerable<IBotReply> botReplies,
        IUserEmotionReminderRepository userEmotionReminderRepository)
    {
        _botClient = botClient;
        _logger = logger;
        _userRepository = userRepository;
        _userEmotionReminderRepository = userEmotionReminderRepository;
        _howDoYouFeelCommand = botActions.FirstOrDefault(x => x.GetType() == typeof(RecordEmotionCommand));
        _howIsYourSatisfactionCommand =
            botActions.FirstOrDefault(x => x.GetType() == typeof(RecordSatisfactionCommand));
        _noCommand = botActions.FirstOrDefault(x => x.GetType() == typeof(NoCommand));
        _startCommand = botActions.FirstOrDefault(x => x.GetType() == typeof(StartCommand));
        _toggleSatisfactionReminderCommand =
            botActions.FirstOrDefault(x => x.GetType() == typeof(ToggleSatisfactionReminderCommand));
        _changeSettingsCommand =
            botActions.FirstOrDefault(x => x.GetType() == typeof(ChangeSettingsCommand));
        _changeLanguageCommand = botActions.FirstOrDefault(x => x.GetType() == typeof(ChangeLanguageCommand));
        _generateReportCommand = botActions.FirstOrDefault(x => x.GetType() == typeof(GenerateReportCommand));
        _changeEmotionReminder = botActions.FirstOrDefault(x => x.GetType() == typeof(ChangeEmotionReminderCommand));
        _recordThoughtCommand = botActions.FirstOrDefault(x => x.GetType() == typeof(RecordThoughtCommand));
        _sendEmergencyMessageCommand =
            botActions.FirstOrDefault(x => x.GetType() == typeof(SendEmergencyMessageCommand));
        _faqCommand = botActions.FirstOrDefault(x => x.GetType() == typeof(FaqCommand));


        _recordSatisfactionReply = botReplies.FirstOrDefault(x => x.GetType() == typeof(RecordSatisfactionReply));
        _recordEmotionReply = botReplies.FirstOrDefault(x => x.GetType() == typeof(RecordEmotionReply));
        _changeLanguageReply = botReplies.FirstOrDefault(x => x.GetType() == typeof(ChangeLanguageReply));
        _change_emotion_reminder_reply =
            botReplies.FirstOrDefault(x => x.GetType() == typeof(ChangeEmotionReminderReply));
        _toggleSatisfactionReminderReply =
            botReplies.FirstOrDefault(x => x.GetType() == typeof(ToggleSatisfactionReminderReply));
        _recordThoughtReply = botReplies.FirstOrDefault(x => x.GetType() == typeof(RecordThoughtReply));
        _faqReply = botReplies.FirstOrDefault(x => x.GetType() == typeof(FaqReply));
    }

    public Task HandleErrorAsync(Exception exception, CancellationToken cancellationToken)
#pragma warning restore RCS1163 // Unused parameter.
#pragma warning restore IDE0060 // Remove unused parameter
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _                                       => exception.ToString()
        };

        _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            // UpdateType.Unknown:
            // UpdateType.ChannelPost:
            // UpdateType.EditedChannelPost:
            // UpdateType.ShippingQuery:
            // UpdateType.PreCheckoutQuery:
            // UpdateType.Poll:
            { Message: { } message }                       => BotOnMessageReceived(message, cancellationToken),
            { EditedMessage: { } message }                 => BotOnMessageReceived(message, cancellationToken),
            { CallbackQuery: { } callbackQuery }           => BotOnCallbackQueryReceived(callbackQuery, cancellationToken),
            { InlineQuery: { } inlineQuery }               => BotOnInlineQueryReceived(inlineQuery, cancellationToken),
            { ChosenInlineResult: { } chosenInlineResult } => BotOnChosenInlineResultReceived(chosenInlineResult, cancellationToken),
            _                                              => UnknownUpdateHandlerAsync(update, cancellationToken)
        };

        await handler;
    }

    private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        await SaveUser(message);

        if (message.Text is not { } messageText)
            messageText = string.Empty;

        var botActionMessage = ConvertToBotActionMessage(message);
        await RunCommand(messageText, botActionMessage, cancellationToken);
    }

    static async Task<Message> SendFile(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await botClient.SendChatActionAsync(
            message.Chat.Id,
            ChatAction.UploadPhoto,
            cancellationToken: cancellationToken);

        const string filePath = "Files/tux.png";
        await using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();

        return await botClient.SendPhotoAsync(
            chatId: message.Chat.Id,
            photo: new InputFileStream(fileStream, fileName),
            caption: "Nice Picture",
            cancellationToken: cancellationToken);
    }

    private async Task SaveUser(Message message)
    {
        var user = await _userRepository.GetByAsync(message.From.Username);
        if (user is null)
        {
            var savedUser = await _userRepository.SaveAsync(new Halood.Domain.Entities.User
            {
                Username = message.From.Username,
                FirstName = message.From.FirstName,
                LastName = message.From?.LastName,
                LanguageCode = message.From?.LanguageCode,
                ChatId = message.Chat.Id,
                IsBot = message.From.IsBot,
                IsPremium = message.From.IsPremium,
                IsGlobalSatisfactionReminderActive = true,
                HasBlockedBot = false
            });
            await _userRepository.CommitAsync();

            await _userEmotionReminderRepository.SaveRangeAsync(new List<UserEmotionReminder>
            {
                new()
                {
                    Hour = 11,
                    UserId = savedUser.Id
                },
                new()
                {
                    Hour = 19,
                    UserId = savedUser.Id
                }
            });
            await _userEmotionReminderRepository.CommitAsync();
        }
    }

    // Process Inline Keyboard callback data
    private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var botActionMessage = ConvertToBotActionMessage(callbackQuery);

        await RunCommand(callbackQuery.Data, botActionMessage, cancellationToken);
    }

    private async Task RunCommand(string messageText, BotCommandMessage botCommandMessage,
        CancellationToken cancellationToken)
    {
        var commandText = messageText.Split(' ')[0];
        var command = ConvertCommandTextToAction(commandText);
        var action = command switch
        {
            CommandType.Start => _startCommand.ExecuteAsync(botCommandMessage, cancellationToken),
            CommandType.Satisfaction => _howIsYourSatisfactionCommand.ExecuteAsync(botCommandMessage,
                cancellationToken),
            CommandType.Emotion => _howDoYouFeelCommand.ExecuteAsync(botCommandMessage, cancellationToken),
            CommandType.Settings => _changeSettingsCommand.ExecuteAsync(botCommandMessage, cancellationToken),
            CommandType.SatisfactionReminder => _toggleSatisfactionReminderCommand.ExecuteAsync(botCommandMessage, cancellationToken),
            CommandType.Language => _changeLanguageCommand.ExecuteAsync(botCommandMessage, cancellationToken),
            CommandType.Report => _generateReportCommand.ExecuteAsync(botCommandMessage, cancellationToken),
            CommandType.EmotionReminder => _changeEmotionReminder.ExecuteAsync(botCommandMessage, cancellationToken),
            CommandType.SatisfactionReply => _recordSatisfactionReply.ExecuteAsync(botCommandMessage, cancellationToken),
            CommandType.EmotionReply => _recordEmotionReply.ExecuteAsync(botCommandMessage, cancellationToken),
            CommandType.LanguageReply => _changeLanguageReply.ExecuteAsync(botCommandMessage, cancellationToken),
            CommandType.EmotionReminderReply => _change_emotion_reminder_reply.ExecuteAsync(botCommandMessage, cancellationToken),
            CommandType.SatisfactionReminderReply => _toggleSatisfactionReminderReply.ExecuteAsync(botCommandMessage, cancellationToken),
            CommandType.RecordThoughtCommand => _recordThoughtCommand.ExecuteAsync(botCommandMessage, cancellationToken),
            CommandType.RecordThoughtReply => _recordThoughtReply.ExecuteAsync(botCommandMessage, cancellationToken),
            CommandType.SendEmergencyMessage => _sendEmergencyMessageCommand.ExecuteAsync(botCommandMessage, cancellationToken),
            CommandType.Faq => _faqCommand.ExecuteAsync(botCommandMessage, cancellationToken),
            CommandType.FaqReply => _faqReply.ExecuteAsync(botCommandMessage, cancellationToken),
            _ => _noCommand.ExecuteAsync(botCommandMessage, cancellationToken)
        };

        await action;
    }

    static async Task<Message> RequestContactAndLocation(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        ReplyKeyboardMarkup RequestReplyKeyboard = new(
            new[]
            {
                KeyboardButton.WithRequestLocation("Location"),
                KeyboardButton.WithRequestContact("Contact"),
            });

        return await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Who or Where are you?",
            replyMarkup: RequestReplyKeyboard,
            cancellationToken: cancellationToken);
    }

    static async Task<Message> StartInlineQuery(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        InlineKeyboardMarkup inlineKeyboard = new(
            InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Inline Mode"));

        return await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Press the button to start Inline Query",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }

    private CommandType ConvertCommandTextToAction(string actionText)
    {
        return Enum.GetValues<CommandType>().FirstOrDefault(command => command.GetRoute() == actionText);
    }

    private BotCommandMessage ConvertToBotActionMessage(Message message)
    {
        return new BotCommandMessage
        {
            Text = message.Text,
            ChatId = message.Chat.Id,
            Date = message.Date,
            Username = message.Chat.Username
        };
    }

    private BotCommandMessage ConvertToBotActionMessage(CallbackQuery callbackQuery)
    {
        return new BotCommandMessage
        {
            Text = callbackQuery.Data,
            ChatId = callbackQuery.Message.Chat.Id,
            Date = callbackQuery.Message.Date,
            Username = callbackQuery.Message.Chat.Username,
            CommandMessageId = callbackQuery.Message.MessageId
        };
    }


    #region Inline Mode

    private async Task BotOnInlineQueryReceived(InlineQuery inlineQuery, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received inline query from: {InlineQueryFromId}", inlineQuery.From.Id);

        InlineQueryResult[] results = {
            // displayed result
            new InlineQueryResultArticle(
                id: "1",
                title: "TgBots",
                inputMessageContent: new InputTextMessageContent("hello"))
        };

        await _botClient.AnswerInlineQueryAsync(
            inlineQueryId: inlineQuery.Id,
            results: results,
            cacheTime: 0,
            isPersonal: true,
            cancellationToken: cancellationToken);
    }

    private async Task BotOnChosenInlineResultReceived(ChosenInlineResult chosenInlineResult, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received inline result: {ChosenInlineResultId}", chosenInlineResult.ResultId);

        await _botClient.SendTextMessageAsync(
            chatId: chosenInlineResult.From.Id,
            text: $"You chose result with Id: {chosenInlineResult.ResultId}",
            cancellationToken: cancellationToken);
    }

    #endregion

#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable RCS1163 // Unused parameter.
    private Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
#pragma warning restore RCS1163 // Unused parameter.
#pragma warning restore IDE0060 // Remove unused parameter
    {
        _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }
}
