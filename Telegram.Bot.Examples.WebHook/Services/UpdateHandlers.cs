using Halood.Domain.Interfaces.User;
using Halood.Domain.Dtos;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using Halood.Domain.Interfaces.BotAction;
using Halood.Service.BotCommand;

namespace Telegram.Bot.Services;

public class UpdateHandlers
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<UpdateHandlers> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IBotCommand _howDoYouFeelCommand;
    private readonly IBotCommand _howIsYourSatisfactionCommand;
    private readonly IBotCommand _noCommand;
    private readonly IBotCommand _startCommand;
    private readonly IBotCommand _toggleReminderCommand;
    private readonly IBotCommand _changeSettingsCommand;
    private readonly IBotCommand _changeLangugeCommand;

    public UpdateHandlers(ITelegramBotClient botClient, ILogger<UpdateHandlers> logger, IUserRepository userRepository,
        IEnumerable<IBotCommand> botActions)
    {
        _botClient = botClient;
        _logger = logger;
        _userRepository = userRepository;
        _howDoYouFeelCommand = botActions.FirstOrDefault(x => x.GetType() == typeof(HowDoYouFeelCommand));
        _howIsYourSatisfactionCommand =
            botActions.FirstOrDefault(x => x.GetType() == typeof(HowIsYourSatisfactionCommand));
        _noCommand = botActions.FirstOrDefault(x => x.GetType() == typeof(NoCommand));
        _startCommand = botActions.FirstOrDefault(x => x.GetType() == typeof(StartCommand));
        _toggleReminderCommand =
            botActions.FirstOrDefault(x => x.GetType() == typeof(ToggleReminderCommand));
        _changeSettingsCommand =
            botActions.FirstOrDefault(x => x.GetType() == typeof(ChangeSettingsCommand));
        _changeLangugeCommand = botActions.FirstOrDefault(x => x.GetType() == typeof(ChangeLanguageCommand));
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

        _logger.LogInformation("Receive message type: {MessageType}", message.Type);
        if (message.Text is not { } messageText)
            return;

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
            await _userRepository.SaveAsync(new Halood.Domain.Entities.User
            {
                Username = message.From.Username,
                FirstName = message.From.FirstName,
                LastName = message.From?.LastName,
                LanguageCode = message.From?.LanguageCode,
                ChatId = message.Chat.Id,
                IsBot = message.From.IsBot,
                IsPremium = message.From.IsPremium
            });
            await _userRepository.CommitAsync();
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
        var command = messageText.Split(' ')[0];
        var action = command switch
        {
            "/start" => _startCommand.Execute(botCommandMessage, cancellationToken),
            "/how_is_your_satisfaction" => _howIsYourSatisfactionCommand.Execute(botCommandMessage,
                cancellationToken),
            "/how_do_you_feel" => _howDoYouFeelCommand.Execute(botCommandMessage, cancellationToken),
            "/change_settings" => _changeSettingsCommand.Execute(botCommandMessage, cancellationToken),
            "/toggle_reminder" => _toggleReminderCommand.Execute(botCommandMessage, cancellationToken),
            "/change_language" => _changeLangugeCommand.Execute(botCommandMessage, cancellationToken),
            _ => _noCommand.Execute(botCommandMessage, cancellationToken)
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
            Username = callbackQuery.Message.Chat.Username
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
