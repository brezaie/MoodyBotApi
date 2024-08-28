using System.Drawing;
using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Microsoft.Extensions.Logging;
using MigraDoc.DocumentObjectModel;
using Halood.Domain.Interfaces.User;
using Halood.Domain.Interfaces.UserSatisfaction;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Document = MigraDoc.DocumentObjectModel.Document;
using PdfSharp.Fonts;
using Telegram.Bot.Services;
using Halood.Domain.Entities;
using Halood.Domain.Interfaces.UserEmotion;
using Stimulsoft.Base;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Export;
using Chart = MigraDoc.DocumentObjectModel.Shapes.Charts.Chart;
using ChartType = MigraDoc.DocumentObjectModel.Shapes.Charts.ChartType;
using DataLabelPosition = MigraDoc.DocumentObjectModel.Shapes.Charts.DataLabelPosition;
using DataLabelType = MigraDoc.DocumentObjectModel.Shapes.Charts.DataLabelType;
using File = System.IO.File;
using Series = MigraDoc.DocumentObjectModel.Shapes.Charts.Series;
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

            report.Dictionary.Variables.Add("EmotionsPageTitle", "احساس‌ها");
            report.Dictionary.Variables.Add("IsEmotionPageEnabled", false);
            

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
                    caption: $"گزارش رضایت از زندگی شما در هفته گذشته در فایل پیوست آمده است. می‌توانید آن را دانلود و مشاهده کنید.\r\nدر صورتی که محتویات فایل به‌درستی بارگذاری نشده، فایل را با یک اپلیکیشن دیگر باز کنید.\r\n",
                    cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            await _botClient.SendTextMessageAsync(
                chatId: message.ChatId,
                text: $"مشکلی در تولید گزارش به وجود آمده است \n {ex.Message}",
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
                    SatisfactionColor = ConvertSatisfactionToColor(satisfaction)
                };
            }).OrderByDescending(x => x.SatisfactionCount).ThenByDescending(x => x.SatisfactionNumber)
            .ToList();
    }

    private byte[] ConvertSatisfactionToBinaryColor(SatisfactionLevel satisfactionLevel)
    {
        var filePath = $"Files/{satisfactionLevel.ToString().ToLower()}_color.jpg";
        return File.ReadAllBytes(filePath);
    }

    private string ConvertSatisfactionToColor(SatisfactionLevel satisfactionLevel)
    {
        return satisfactionLevel switch
        {
            SatisfactionLevel.Perfect => "#008001",
            SatisfactionLevel.Good => "#A4C739",
            SatisfactionLevel.SoSo => "#FFE135",
            SatisfactionLevel.Bad => "#FF0038",
            SatisfactionLevel.Awful => "#960019",
            _ => "#FFFFFF"
        };
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


    void DefineStyles(Document document)
    {
        GlobalFontSettings.FontResolver = new SahelFontResolver();

        var style = document.Styles["Normal"];

        style = document.Styles["Heading1"];
        style.Font.Size = 14;
        style.Font.Bold = true;
        style.Font.Color = Colors.DarkBlue;
        style.ParagraphFormat.PageBreakBefore = true;
        style.ParagraphFormat.SpaceAfter = 6;
        style.ParagraphFormat.Alignment = ParagraphAlignment.Right;

        style = document.Styles["Heading2"];
        style.Font.Size = 12;
        style.Font.Bold = true;
        style.ParagraphFormat.PageBreakBefore = false;
        style.ParagraphFormat.SpaceBefore = 6;
        style.ParagraphFormat.SpaceAfter = 6;

        style = document.Styles["Heading3"];
        style.Font.Size = 10;
        style.Font.Bold = true;
        style.Font.Italic = true;
        style.ParagraphFormat.SpaceBefore = 6;
        style.ParagraphFormat.SpaceAfter = 3;

        style = document.Styles[StyleNames.Header];
        style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

        style = document.Styles[StyleNames.Footer];
        style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

        // Create a new style called TextBox based on style Normal
        style = document.Styles.AddStyle("TextBox", "Normal");
        style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
        style.ParagraphFormat.Borders.Width = 2.5;
        style.ParagraphFormat.Borders.Distance = "3pt";
        style.ParagraphFormat.Shading.Color = Colors.SkyBlue;

        // Create a new style called TOC based on style Normal
        style = document.Styles.AddStyle("TOC", "Normal");
        style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right, TabLeader.Dots);
        style.ParagraphFormat.Font.Color = Colors.Blue;
    }


    void DefineMyCharts(Document document, Dictionary<SatisfactionLevel, int> distribution, Dictionary<DateTime, double> trend)
    {
        Paragraph paragraph = document.LastSection.AddParagraph("گزارش هفتگی".PersianWithFontGlyphsToPfd(), "Heading1");
        paragraph.AddBookmark("Charts");

        document.LastSection.AddParagraph(
            $" {trend.Keys.Max().ToPersianDigitalDateTimeString().Substring(0, 10)} "+
            $"  تا  ".PersianWithFontGlyphsToPfd() +
            $" {trend.Keys.Min().ToPersianDigitalDateTimeString().Substring(0, 10)} " +
            $"از تاریخ ".PersianWithFontGlyphsToPfd(),
            "Heading4");



        #region Pie Chart

        var p = document.LastSection.AddParagraph();
        p.AddText("پراکندگی رضایت از زندگی".PersianWithFontGlyphsToPfd());
        p.Style = "Heading2";

        Chart piechart = new Chart(ChartType.Pie2D);
        piechart.Width = Unit.FromCentimeter(10);
        piechart.Height = Unit.FromCentimeter(10);
        Series series = piechart.SeriesCollection.AddSeries();
        series.Add(distribution.Values.Select(x => (double)x).ToArray());

        var xseries = piechart.XValues.AddXSeries();
        xseries.Add(distribution.Keys
            .Select(satisfactionLevel =>
                ConvertSatisfactionLevelToPersian(satisfactionLevel).PersianWithFontGlyphsToPfd())
            .ToArray());
        piechart.FooterArea.AddLegend();

        piechart.DataLabel.Type = DataLabelType.Percent;
        piechart.DataLabel.Position = DataLabelPosition.Center;

        document.LastSection.Add(piechart);

        #endregion



        #region Line Chart

        document.LastSection.AddParagraph("روند رضایت از زندگی".PersianWithFontGlyphsToPfd(), "Heading2");
        document.LastSection.AddParagraph("عدد 5 بیانگر بیشترین رضایت و عدد 1 بیانگر کمترین رضایت هستند".PersianWithFontGlyphsToPfd(), "Heading5");

        var lineChart = new Chart();
        lineChart.Left = 0;

        lineChart.Width = Unit.FromCentimeter(16);
        lineChart.Height = Unit.FromCentimeter(8);

        var y = lineChart.SeriesCollection.AddSeries();
        y.ChartType = ChartType.Line;
        y.Add(trend.Values.ToArray());
        y.HasDataLabel = true;

        //var y = chart.SeriesCollection.AddSeries();
        //y.ChartType = ChartType.Column2D;
        //y.Add(trend.Values.ToArray());
        //y.HasDataLabel = true;

        var x = lineChart.XValues.AddXSeries();
        x.Add(trend.Keys
            .Select(dateTime =>
                {
                    var d = dateTime
                        .ToPersianDateString();
                    var firstDigitIndex = d.IndexOfAny("0123456789".ToCharArray());
                    return d.Substring(0, firstDigitIndex)
                        .PersianWithFontGlyphsToPfd();
                }
            )
            .ToArray());

        //lineChart.XAxis.MajorTickMark = TickMarkType.Cross;
        lineChart.XAxis.Title.Caption = "روز هفته".PersianWithFontGlyphsToPfd();

        //lineChart.YAxis.MajorTickMark = TickMarkType.Cross;
        lineChart.YAxis.MajorTick = 1.0;

        document.LastSection.Add(lineChart);

        #endregion
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
