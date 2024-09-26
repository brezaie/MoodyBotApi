using System.Drawing;
using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Microsoft.Extensions.Logging;
using Halood.Domain.Interfaces.User;
using Halood.Domain.Interfaces.UserSatisfaction;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Halood.Domain.Entities;
using Halood.Domain.Interfaces.UserEmotion;
using Stimulsoft.Base;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Export;
using File = System.IO.File;
using User = Halood.Domain.Entities.User;

namespace Halood.Service.BotCommand;

public class GenerateReportCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<RecordEmotionCommand> _logger;
    private string _text = string.Empty;
    private readonly IUserSatisfactionRepository _userSatisfactionRepository;
    private readonly IUserEmotionRepository _userEmotionRepository;
    private readonly IUserRepository _userRepository;

    public GenerateReportCommand(ITelegramBotClient botClient, ILogger<RecordEmotionCommand> logger,
        IUserSatisfactionRepository userSatisfactionRepository, IUserRepository userRepository,
        IUserEmotionRepository userEmotionRepository)
    {
        _botClient = botClient;
        _logger = logger;
        _userSatisfactionRepository = userSatisfactionRepository;
        _userRepository = userRepository;
        _userEmotionRepository = userEmotionRepository;
    }

    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        await _botClient.SendChatActionAsync(
            chatId: message.ChatId,
            chatAction: ChatAction.UploadDocument,
            cancellationToken: cancellationToken);

        var previousDays = 7;
        var user = await _userRepository.GetByAsync(message.Username);
        var satisfactions = await _userSatisfactionRepository.GetLastUserSatisfactionsByDaysAsync(user.Id, previousDays);
        var emotions = await _userEmotionRepository.GetLastUserEmotionsByDaysAsync(user.Id, previousDays);

        if (satisfactions.Count == 0 && emotions.Count == 0) return;

        try
        {
            var report = new StiReport();
            report.Load(@"Files\report.mrt");

            StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHkO46nMQvol4ASeg91in+mGJLnn2KMIpg3eSXQSgaFOm15+0l" +
                             "hekKip+wRGMwXsKpHAkTvorOFqnpF9rchcYoxHXtjNDLiDHZGTIWq6D/2q4k/eiJm9fV6FdaJIUbWGS3whFWRLPHWC" +
                             "BsWnalqTdZlP9knjaWclfjmUKf2Ksc5btMD6pmR7ZHQfHXfdgYK7tLR1rqtxYxBzOPq3LIBvd3spkQhKb07LTZQoyQ" +
                             "3vmRSMALmJSS6ovIS59XPS+oSm8wgvuRFqE1im111GROa7Ww3tNJTA45lkbXX+SocdwXvEZyaaq61Uc1dBg+4uFRxv" +
                             "yRWvX5WDmJz1X0VLIbHpcIjdEDJUvVAN7Z+FW5xKsV5ySPs8aegsY9ndn4DmoZ1kWvzUaz+E1mxMbOd3tyaNnmVhPZ" +
                             "eIBILmKJGN0BwnnI5fu6JHMM/9QR2tMO1Z4pIwae4P92gKBrt0MqhvnU1Q6kIaPPuG2XBIvAWykVeH2a9EP6064e11" +
                             "PFCBX4gEpJ3XFD0peE5+ddZh+h495qUc1H2B";

            StiFontCollection.AddFontFile(@"Files\IRANSansWebFaNum.ttf", "IRANSans", FontStyle.Regular);

            report.Dictionary.Variables.Add("Username", GetFullName(user));
            report.Dictionary.Variables.Add("FromDate", DateTime.Now.AddDays(-previousDays).ToPersianDigitalDateTimeString().Substring(0, 10));
            report.Dictionary.Variables.Add("ToDate", DateTime.Now.AddDays(-1).ToPersianDigitalDateTimeString().Substring(0, 10));
            report.Dictionary.Variables.Add("CurrentDate", DateTime.Now.ToPersianDigitalDateTimeString().Substring(0, 10));


            #region Satisfaction Page
            
            report.Dictionary.Variables.Add("IsSatisfactionPageEnabled", satisfactions.Count != 0);

            report.RegBusinessObject("Satisfaction", "SatisfactionPieChart", ConvertToSatisfactionDistribution(satisfactions));
            report.RegBusinessObject("Satisfaction", "SatisfactionTrend", ConvertToSatisfactionTrend(satisfactions));

            #endregion
            
            #region Emotions Page

            report.Dictionary.Variables.Add("IsEmotionPageEnabled", true);
            report.RegBusinessObject("Emotion", "EmotionPieChart", ConvertToEmotionDistribution(emotions));


            #endregion

            #region Change Font to IRANSans

            var fileContent = await File.ReadAllBytesAsync(@"Files\IRANSansWebFaNum.ttf", cancellationToken);
            var resource = new StiResource("IRANSans", "IRANSans", false, StiResourceType.FontTtf, fileContent, false);
            report.Dictionary.Resources.Add(resource);
            StiFontCollection.AddResourceFont(resource.Name, resource.Content, "ttf", resource.Alias);

            var pages = report.Pages;
            foreach (StiPage page in pages)
            {
                foreach (StiComponent component in page.GetComponents())
                {
                    if (component is StiText textComponent)
                    {
                        textComponent.Font = StiFontCollection.CreateFont("IRANSans", textComponent.Font.Size, textComponent.Font.Style);
                    }
                }
            }

            #endregion


            await report.CompileAsync();
            await report.RenderAsync();
            var filename = $"{user.Username}.pdf";

            await report.ExportDocumentAsync(StiExportFormat.Pdf, filename,
                new StiPdfExportSettings
                {
                    ImageResolution = 400,
                    ImageQuality = 1000,
                    StandardPdfFonts = true,
                    EmbeddedFonts = true,
                    AllowEditable = StiPdfAllowEditable.No,
                    UseUnicode = true,
                    UserAccessPrivileges = StiUserAccessPrivileges.All
                }
            );

            using (Stream stream = (Stream)new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                await _botClient.SendDocumentAsync(
                    chatId: message.ChatId,
                    document: InputFile.FromStream(stream, filename),
                    caption: $"گزارش عمل‌کرد شما در هفته گذشته در فایل پیوست آمده است. می‌توانید آن را دانلود و مشاهده کنید.\r\nدر صورتی که محتویات فایل به‌درستی بارگذاری نشده، فایل را با یک اپلیکیشن دیگر باز کنید.\r\n",
                    cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            await _botClient.SendTextMessageAsync(
                chatId: CommandHandler.SpecialUserNames.FirstOrDefault().Value,
                text: $"مشکلی در تولید گزارش برای {message.Username} به وجود آمده است \n {ex.Message}",
                cancellationToken: cancellationToken);
        }
    }

    private string GetFullName(User user)
    {
        return string.IsNullOrEmpty(user.FirstName) switch
        {
            false when !string.IsNullOrEmpty(user.LastName) =>
                $"{user.FirstName} {user.LastName} | {user.Username}",
            false when string.IsNullOrEmpty(user.LastName) => $"{user.FirstName} | {user.Username}",
            _ => string.IsNullOrEmpty(user.LastName) ? $"{user.LastName} | {user.Username}" : $"{user.Username}"
        };
    }

    private List<SatisfactionDistribution> ConvertToSatisfactionDistribution(List<UserSatisfaction> satisfactions)
    {
        var satisfactionsCount = satisfactions.Count;
        return satisfactions.GroupBy(x => x.SatisfactionNumber)
            .Select(x =>
            {
                var satisfaction = (SatisfactionLevel) x.Key;
                return new SatisfactionDistribution
                {
                    SatisfactionName = ConvertSatisfactionToName(satisfaction),
                    SatisfactionNumber = x.Key,
                    SatisfactionCount = x.Count(),
                    SatisfactionPercentage = (float) ((float) 100 * (float) x.Count() / (float) satisfactionsCount),
                    SatisfactionBinaryColor = ConvertSatisfactionToBinaryColor(satisfaction),
                    SatisfactionColor = satisfaction.GetColor()
                };
            }).OrderByDescending(x => x.SatisfactionCount).ThenByDescending(x => x.SatisfactionNumber)
            .ToList();
    }

    private List<EmotionDistribution> ConvertToEmotionDistribution(List<UserEmotion> userEmotions)
    {
        var emotionsCount = userEmotions.Count;
        var res = userEmotions.GroupBy(x => x.EmotionText)
            .Select(x =>
            {
                var emotion = (Emotion)Enum.Parse(typeof(Emotion), x.Key, true);
                return new EmotionDistribution
                {
                    EmotionName = emotion.GetDescription(),
                    EmotionCount = x.Count(),
                    EmotionPercentage = (float)((float)100 * (float)x.Count() / (float)emotionsCount),
                    EmotionBinaryColor = ConvertEmotionToBinaryColor(emotion),
                    EmotionColor = emotion.GetColor()
                };
            }).OrderByDescending(x => x.EmotionCount).ThenByDescending(x => x.EmotionName)
            .ToList();

        return res;
    }


    private byte[] ConvertSatisfactionToBinaryColor(SatisfactionLevel satisfactionLevel)
    {
        var filePath = $"Files/{satisfactionLevel.ToString().ToLower()}_color.jpg";
        return File.ReadAllBytes(filePath);
    }

    private byte[] ConvertEmotionToBinaryColor(Emotion emotion)
    {
        var filePath = $"Files/{emotion.ToString().ToLower()}_color.jpg";
        return File.ReadAllBytes(filePath);
    }
    
    private string ConvertSatisfactionToName(SatisfactionLevel satisfactionLevel)
    {
        return satisfactionLevel switch
        {
            SatisfactionLevel.Perfect => "خیلی زیاد",
            SatisfactionLevel.Good => "زیاد",
            SatisfactionLevel.SoSo => "متوسط",
            SatisfactionLevel.Bad => "کم",
            SatisfactionLevel.Awful => "خیلی کم",
            _ => "#FFFFFF"
        };
    }

    
    private List<SatisfactionTrend> ConvertToSatisfactionTrend(List<UserSatisfaction> satisfactions)
    {
        return satisfactions.GroupBy(x => x.CreatedDate.Value.Date)
            .OrderBy(x => x.Key)
            .Select(x =>
            {
                var d = x.Key.ToPersianDateString();
                var firstDigitIndex = d.IndexOfAny("0123456789".ToCharArray());
                
                return new SatisfactionTrend
                {
                    SatisfactionLevel = (float)x.Average(y => y.SatisfactionNumber),
                    Date = $"{d}"
                };
            }).ToList();
    }

    private string ConvertSatisfactionLevelToPersian(SatisfactionLevel satisfactionLevel)
    {
        return satisfactionLevel switch
        {
            SatisfactionLevel.Perfect => "عالی",
            SatisfactionLevel.Good => "خوب",
            SatisfactionLevel.SoSo => "متوسط",
            SatisfactionLevel.Bad => "بد",
            SatisfactionLevel.Awful => "افتضاح",
            _ => string.Empty
        };
    }

}
