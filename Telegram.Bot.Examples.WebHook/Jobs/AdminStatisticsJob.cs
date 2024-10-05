using Halood.Common;
using Halood.Domain.Interfaces.User;
using Halood.Domain.Interfaces.UserEmotion;
using Halood.Domain.Interfaces.UserSatisfaction;

namespace Telegram.Bot.Examples.WebHook.Jobs;

public class AdminStatisticsJob : IJob
{
    private readonly IUserRepository _userRepository;
    private readonly IUserSatisfactionRepository _userSatisfactionRepository;
    private readonly IUserEmotionRepository _userEmotionRepository;
    private readonly ITelegramBotClient _botClient;

    public AdminStatisticsJob(IUserRepository userRepository, ITelegramBotClient botClient, IUserSatisfactionRepository userSatisfactionRepository, IUserEmotionRepository userEmotionRepository)
    {
        _userRepository = userRepository;
        _botClient = botClient;
        _userSatisfactionRepository = userSatisfactionRepository;
        _userEmotionRepository = userEmotionRepository;
    }

    public async Task Run()
    {
        try
        {
            var newUsers = await _userRepository.GetTodayNewUsersAsync();
            var newSatisfactions = await _userSatisfactionRepository.GetTodayNumberOfSatisfactions();
            var newEmotions = await _userEmotionRepository.GetTodayNumberOfEmotions();

            var newUsersString = string.Join("\n", newUsers.Select(x => $"{x.FirstName} {x.LastName} | {x.Username}"));

            await _botClient.SendTextMessageAsync(
                chatId: CommandHandler.SpecialUserNames.FirstOrDefault().Value,
                text: $"New Users: \n {newUsersString}\n#Satisfactions: {newSatisfactions}\n#Emotions: {newEmotions}");
        }
        catch (Exception ex)
        {
            await _botClient.SendTextMessageAsync(
                chatId: CommandHandler.SpecialUserNames.FirstOrDefault().Value,
                text: $"Error in sending admin statistics: {ex.Message}");
        }

    }
}
