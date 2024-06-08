using Halood.Domain.Entities;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.User;
using Halood.Domain.Interfaces.UserSatisfaction;
using Newtonsoft.Json;
using System;
using Telegram.Bot.Examples.WebHook;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Services;

public class UpdateHandlers
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<UpdateHandlers> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IUserSatisfactionRepository _userSatisfactionRepository;

    public static Dictionary<string, CommandType> Commands = new();

    public UpdateHandlers(ITelegramBotClient botClient, ILogger<UpdateHandlers> logger, IUserRepository userRepository,
        IUserSatisfactionRepository userSatisfactionRepository)
    {
        _botClient = botClient;
        _logger = logger;
        _userRepository = userRepository;
        _userSatisfactionRepository = userSatisfactionRepository;
    }

#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable RCS1163 // Unused parameter.
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


        _logger.LogInformation("Receive message type: {MessageType}", message.Type);
        if (message.Text is not { } messageText)
            return;

        var command = messageText.Split(' ')[0];

        var action = command switch
        {
            "/start" => StartKeyboard(_botClient, message, cancellationToken),
            "/how_is_your_satisfaction" => HowIsYourSatisfactionKeyboard(_botClient, message, cancellationToken),
            "/how_do_you_feel" => HowDoYouFeelKeyboard(_botClient, message, cancellationToken),
            //"/photo" => SendFile(_botClient, message, cancellationToken),
            //"/request" => RequestContactAndLocation(_botClient, message, cancellationToken),
            //"/inline_mode" => StartInlineQuery(_botClient, message, cancellationToken),
            _ => Usage(_botClient, message, cancellationToken)
        };
        Message sentMessage = await action;
        _logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);

        // Send inline keyboard
        // You can process responses in BotOnCallbackQueryReceived handler
        static async Task<Message> StartKeyboard(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {

            var t = JsonConvert.SerializeObject(message);
            Commands.Remove(message.Chat.Username);

            await botClient.SendChatActionAsync(
                chatId: message.Chat.Id,
                chatAction: ChatAction.Typing,
                cancellationToken: cancellationToken);

            // Simulate longer running task
            await Task.Delay(500, cancellationToken);

            //InlineKeyboardMarkup inlineKeyboard = new(
            //    new[]
            //    {
            //        // first row
            //        new []
            //        {
            //            InlineKeyboardButton.WithCallbackData("1.1", "11"),
            //            InlineKeyboardButton.WithCallbackData("1.2", "12"),
            //        },
            //        // second row
            //        new []
            //        {
            //            InlineKeyboardButton.WithCallbackData("2.1", "21"),
            //            InlineKeyboardButton.WithCallbackData("2.2", "22"),
            //        },
            //    });

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"به بات ثبت احساس ها و افکار خوش اومدید. امیدواریم بتونیم میزبان ناب ترین احساسات شما باشیم. برای ثبت احساس، فکر و رضایت از زندگی، میتونید از گزینه Menu در پایین، سمت چپ استفاده کنید. با تشکر",
                //replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }

        static async Task<Message> HowIsYourSatisfactionKeyboard(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(
                new[]
                {
                    new KeyboardButton[]
                    {
                        SatisfactionLevel.Awful.GetDescription(),
                        SatisfactionLevel.Bad.GetDescription(),
                        SatisfactionLevel.SoSo.GetDescription(),
                        SatisfactionLevel.Good.GetDescription(),
                        SatisfactionLevel.Perfect.GetDescription()
                    }
                })
            {
                ResizeKeyboard = true
            };

            var doesCommandExist = Commands.FirstOrDefault(x => x.Key == message.Chat.Username);
            if (doesCommandExist.Value != CommandType.Satisfaction)
            {
                Commands.Add(message.Chat.Username, CommandType.Satisfaction);
            }

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "چقدر از امروزت راضی بودی؟",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken);
        }

        static async Task<Message> HowDoYouFeelKeyboard(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "این قسمت بعدا تکمیل میشه",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
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

        async Task<Message> Usage(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            var usage = string.Empty;
            var previousCommand = Commands.FirstOrDefault(x => x.Key == message.Chat.Username);
            if (string.IsNullOrEmpty(previousCommand.Key))
            {
                usage = "یکی از گزینه های زیر رو انتخاب کن:\n" +
                        "/start - شروع بات\n" +
                        "/how_is_your_satisfaction - چقدر از امروز راضی بودی تا الان؟\n" +
                        "/how_do_you_feel - الان چه احساسی داری؟";
            }
            else if (previousCommand.Value == CommandType.Satisfaction)
            {
                if (((SatisfactionLevel[]) Enum.GetValues(typeof(SatisfactionLevel))).All(x =>
                        x.GetDescription() != message.Text))
                {
                    usage = $"مقداری که وارد کردی، معتبر نیست. لطفاً یکی از گزینه های زیر رو انتخاب کن";
                    ReplyKeyboardMarkup replyKeyboardMarkup = new(
                        new[]
                        {
                            new KeyboardButton[]
                            {
                                SatisfactionLevel.Awful.GetDescription(),
                                SatisfactionLevel.Bad.GetDescription(),
                                SatisfactionLevel.SoSo.GetDescription(),
                                SatisfactionLevel.Good.GetDescription(),
                                SatisfactionLevel.Perfect.GetDescription()
                            }
                        })
                    {
                        ResizeKeyboard = true
                    };

                    return await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: usage,
                        replyMarkup: replyKeyboardMarkup,
                        cancellationToken: cancellationToken);
                }

                var userId = (await _userRepository.GetByAsync(message.Chat.Username)).Id;

                var lastUserSatisfaction = await _userSatisfactionRepository.GetLastUserSatisfactionAsync(userId);

                if (message.Chat.Username != "brezaie" && lastUserSatisfaction is not null &&
                    (message.Date - lastUserSatisfaction.RegistrationDate).Minutes <= 60)
                {
                    usage =
                        $"از آخرین دفعه که میزان رضایت خود را ثبت کرده‌اید، کم‌تر از 1 ساعت گذشته است. پس از گذشت این زمان می‌توانید مجدد رضایت خود را ثبت کنید 🙂";
                    return await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: usage,
                        cancellationToken: cancellationToken);
                }

                await _userSatisfactionRepository.SaveAsync(new UserSatisfaction
                {
                    RegistrationDate = message.Date,
                    SatisfactionNumber = (int) ((SatisfactionLevel[]) Enum.GetValues(typeof(SatisfactionLevel)))
                        .FirstOrDefault(x => x.GetDescription() == message.Text),
                    UserId = userId
                });
                await _userSatisfactionRepository.CommitAsync();

                usage = "ممنون که رضایت از زندگی امروزت رو ثبت کردی 👍";
                Commands.Remove(message.Chat.Username);
            }

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: usage,
                replyMarkup: new ReplyKeyboardRemove(),
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
    }

    // Process Inline Keyboard callback data
    private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received inline keyboard callback from: {CallbackQueryId}", callbackQuery.Id);

        await _botClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: $"Received {callbackQuery.Data}",
            cancellationToken: cancellationToken);

        await _botClient.SendTextMessageAsync(
            chatId: callbackQuery.Message!.Chat.Id,
            text: $"Received {callbackQuery.Data}",
            cancellationToken: cancellationToken);
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
