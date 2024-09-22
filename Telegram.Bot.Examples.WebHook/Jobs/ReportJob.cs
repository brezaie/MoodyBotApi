using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Interfaces.BotAction;
using Halood.Domain.Interfaces.User;
using Halood.Service.BotCommand;

namespace Telegram.Bot.Examples.WebHook.Jobs;

public class ReportJob : IJob
{
    private readonly IUserRepository _userRepository;
    private readonly ITelegramBotClient _botClient;
    private readonly IBotCommand _generateReportCommand;

    public ReportJob(IUserRepository userRepository, ITelegramBotClient botClient, IEnumerable<IBotCommand> botCommands)
    {
        _userRepository = userRepository;
        _botClient = botClient;
        _generateReportCommand = botCommands.FirstOrDefault(x => x.GetType() == typeof(GenerateReportCommand));
    }

    public async Task Run()
    {
        var users = await _userRepository.GetAllAsync();
        foreach (var user in users.Where(x => !x.HasBlockedBot && (x.Username == "brezaie" || x.Username == "SepidRose")))
        {
            try
            {
                await _generateReportCommand.ExecuteAsync(new BotCommandMessage
                {
                    ChatId = user.ChatId,
                    Date = DateTime.Now,
                    Username = user.Username
                }, new CancellationToken());
            }
            catch (Exception ex)
            {
                //TODO: In case of having any error, ignore it
            }
        }
    }
}
