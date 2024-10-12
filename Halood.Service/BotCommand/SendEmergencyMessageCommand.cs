using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Halood.Domain.Interfaces.User;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Halood.Service.BotCommand;

public class SendEmergencyMessageCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<NoCommand> _logger;
    private string _text = string.Empty;
    private readonly IUserRepository _userRepository;

    public SendEmergencyMessageCommand(ITelegramBotClient botClient, ILogger<NoCommand> logger, IUserRepository userRepository)
    {
        _botClient = botClient;
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        if (!CommandHandler.SpecialUserNames.ContainsKey(message.Username))
            return;

        var givenText = message.Text.Substring(CommandType.SendEmergencyMessage.GetRoute().Length);
        var users = await _userRepository.GetAllAsync();
        foreach (var user in users.Where(x => !x.HasBlockedBot))
        {
            try
            {
                await _botClient.SendTextMessageAsync(
                    chatId: user.ChatId,
                    text: givenText,
                    cancellationToken: cancellationToken
                );
            }
            catch (Exception ex)
            {
                await _botClient.SendTextMessageAsync(
                    chatId: CommandHandler.SpecialUserNames.FirstOrDefault().Value,
                    text: $"مشکلی در ارسال پیام فوری برای {message.Username} به وجود آمده است \n {ex.Message}",
                    cancellationToken: cancellationToken);
            }
        }
    }
}
