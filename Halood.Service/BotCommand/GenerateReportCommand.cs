using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Microsoft.Extensions.Logging;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using MigraDoc.Rendering;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Document = MigraDoc.DocumentObjectModel.Document;

namespace Halood.Service.BotCommand;

public class GenerateReportCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<HowDoYouFeelCommand> _logger;
    private string _text = string.Empty;

    public GenerateReportCommand(ITelegramBotClient botClient, ILogger<HowDoYouFeelCommand> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    public async Task Execute(BotCommandMessage message, CancellationToken cancellationToken)
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

        DefineMyCharts(document);

        var filename = "Behzad.pdf";

        var renderer = new PdfDocumentRenderer
        {
            Document = document
        };
        renderer.RenderDocument();
        renderer.PdfDocument.Save(filename);

        try
        {
            using (Stream stream = (Stream) new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                await _botClient.SendDocumentAsync(
                    chatId: message.ChatId,
                    document: InputFile.FromStream(stream, filename),
                    caption: "",
                    cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            await _botClient.SendTextMessageAsync(
                chatId: message.ChatId,
                text: "Problem in generating the report",
                cancellationToken: cancellationToken);
        }
    }


    void DefineMyCharts(Document document)
    {
        Paragraph paragraph = document.LastSection.AddParagraph("Chart Overview", "Heading1");
        paragraph.AddBookmark("Charts");

        document.LastSection.AddParagraph("Sample Chart", "Heading2");

        Chart chart = new Chart();
        chart.Left = 0;

        chart.Width = Unit.FromCentimeter(16);
        chart.Height = Unit.FromCentimeter(12);

        var y = chart.SeriesCollection.AddSeries();
        y.ChartType = ChartType.Column2D;
        y.Add(10.0, 30, 20, 15, 25);

        XSeries x = chart.XValues.AddXSeries();
        x.Add("5", "4", "3", "2", "1");

        chart.XAxis.MajorTickMark = TickMarkType.Cross;
        chart.XAxis.Title.Caption = "X-Axis";


        chart.YAxis.MajorTickMark = TickMarkType.Outside;
        chart.YAxis.HasMajorGridlines = true;

        document.LastSection.Add(chart);


        //Series series = chart.SeriesCollection.AddSeries();
        //series.ChartType = ChartType.Column2D;
        //series.Add(new double[] { 1, 17, 45, 5, 3, 20, 11, 23, 8, 19 });
        //series.HasDataLabel = true;

        //series = chart.SeriesCollection.AddSeries();
        //series.ChartType = ChartType.Line;
        //series.Add(new double[] { 41, 7, 5, 45, 13, 10, 21, 13, 18, 9 });

        //XSeries xseries = chart.XValues.AddXSeries();
        //xseries.Add("A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N");

        //chart.XAxis.MajorTickMark = TickMarkType.Outside;
        //chart.XAxis.Title.Caption = "X-Axis";

        //chart.YAxis.MajorTickMark = TickMarkType.Outside;
        //chart.YAxis.HasMajorGridlines = true;

        //chart.PlotArea.LineFormat.Color = Colors.DarkGray;
        //chart.PlotArea.LineFormat.Width = 1;

        //document.LastSection.Add(chart);
    }

}
