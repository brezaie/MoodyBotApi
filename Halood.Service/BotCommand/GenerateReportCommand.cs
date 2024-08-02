using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Microsoft.Extensions.Logging;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Halood.Domain.Interfaces.User;
using Halood.Domain.Interfaces.UserSatisfaction;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Document = MigraDoc.DocumentObjectModel.Document;
using PdfSharp.Fonts;
using Telegram.Bot.Services;
using Halood.Domain.Entities;
using Chart = MigraDoc.DocumentObjectModel.Shapes.Charts.Chart;
using ChartType = MigraDoc.DocumentObjectModel.Shapes.Charts.ChartType;
using DataLabelPosition = MigraDoc.DocumentObjectModel.Shapes.Charts.DataLabelPosition;
using DataLabelType = MigraDoc.DocumentObjectModel.Shapes.Charts.DataLabelType;
using Series = MigraDoc.DocumentObjectModel.Shapes.Charts.Series;

namespace Halood.Service.BotCommand;

public class GenerateReportCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<RecordEmotionCommand> _logger;
    private string _text = string.Empty;
    private readonly IUserSatisfactionRepository _userSatisfactionRepository;
    private readonly IUserRepository _userRepository;

    public GenerateReportCommand(ITelegramBotClient botClient, ILogger<RecordEmotionCommand> logger, IUserSatisfactionRepository userSatisfactionRepository, IUserRepository userRepository)
    {
        _botClient = botClient;
        _logger = logger;
        _userSatisfactionRepository = userSatisfactionRepository;
        _userRepository = userRepository;
    }

    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        CommandHandler.AddCommand(message.Username, CommandType.Report);

        await _botClient.SendChatActionAsync(
            chatId: message.ChatId,
            chatAction: ChatAction.UploadDocument,
            cancellationToken: cancellationToken);

        var document = new Document
        {
            Info =
            {
                Title = "Title",
                Subject = "Descrption",
                Author = "Behzad Rezaei"
            }
        };

        var user = await _userRepository.GetByAsync(message.Username);
        var satisfactions = await _userSatisfactionRepository.GetLastUserSatisfactionsByDaysAsync(user.Id, 7);

        if (satisfactions.Count == 0) return;

        var satisfactionsDistribution = ConvertSatisfactionsToDistribution(satisfactions);
        var satisfactionsTrend = ConvertSatisfactionsToTrend(satisfactions);

        try
        {
            DefineStyles(document);

            DefineMyCharts(document, satisfactionsDistribution, satisfactionsTrend);

            var filename = $"{user.Username}.pdf";

            var renderer = new PdfDocumentRenderer
            {
                Document = document
            };
            renderer.RenderDocument();
            renderer.PdfDocument.Save(filename);
            
            using (Stream stream = (Stream) new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                await _botClient.SendDocumentAsync(
                    chatId: message.ChatId,
                    document: InputFile.FromStream(stream, filename),
                    caption: $"گزارش رضایت از زندگی شما در هفته گذشته در فایل پیوست آمده است. می‌توانید آن را دانلود و مشاهده کنید.\n",
                    cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            await _botClient.SendTextMessageAsync(
                chatId: message.ChatId,
                text: "مشکلی در تولید گزارش به وجود آمده است",
                cancellationToken: cancellationToken);
        }
    }

    private Dictionary<SatisfactionLevel, int> ConvertSatisfactionsToDistribution(List<UserSatisfaction> satisfactions)
    {
        return satisfactions
            .GroupBy(x => x.SatisfactionNumber)
            .ToDictionary(x => (SatisfactionLevel) x.Key,
                x => x.Count());
    }


    private Dictionary<DateTime, double> ConvertSatisfactionsToTrend(List<UserSatisfaction> satisfactions)
    {
        return satisfactions
            .OrderBy(x => x.CreatedDate)
            .GroupBy(x => x.CreatedDate.Value.Date)
            .ToDictionary(x => x.Key,
                x => x.Average(y => y.SatisfactionNumber));
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
