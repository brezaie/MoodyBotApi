using Halood.Repository;
using Hangfire;
using Halood.Service;
using Hangfire.Dashboard;
using Telegram.Bot;
using Telegram.Bot.Controllers;
using Telegram.Bot.Examples.WebHook.Jobs;
using Telegram.Bot.Filters;
using Telegram.Bot.Services;

var builder = WebApplication.CreateBuilder(args);

// Setup Bot configuration
var botConfigurationSection = builder.Configuration.GetSection(BotConfiguration.Configuration);
builder.Services.Configure<BotConfiguration>(botConfigurationSection);

var botConfiguration = botConfigurationSection.Get<BotConfiguration>();

// Register named HttpClient to get benefits of IHttpClientFactory
// and consume it with ITelegramBotClient typed client.
// More read:
//  https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests#typed-clients
//  https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
builder.Services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    BotConfiguration? botConfig = sp.GetConfiguration<BotConfiguration>();
                    TelegramBotClientOptions options = new(botConfig.BotToken);
                    return new TelegramBotClient(options, httpClient);
                });

// There are several strategies for completing asynchronous tasks during startup.
// Some of them could be found in this article https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-part-1/
// We are going to use IHostedService to add and later remove Webhook
builder.Services.AddHostedService<ConfigureWebhook>();

// The Telegram.Bot library heavily depends on Newtonsoft.Json library to deserialize
// incoming webhook updates and send serialized responses back.
// Read more about adding Newtonsoft.Json to ASP.NET Core pipeline:
//   https://docs.microsoft.com/en-us/aspnet/core/web-api/advanced/formatting?view=aspnetcore-6.0#add-newtonsoftjson-based-json-format-support
builder.Services
    .AddControllers()
    .AddNewtonsoftJson();


#region Services

builder.Services.AddRepositoryDependencies(builder.Configuration);
builder.Services.AddServiceDependencies();
builder.Services.AddTransient<IJob, SatisfactionReminderJob>();
builder.Services.AddTransient<IJob, ReportJob>();
builder.Services.AddTransient<IJob, EmotionReminderJob>();
builder.Services.AddTransient<IJob, AdminStatisticsJob>();
// Dummy business-logic service
builder.Services.AddTransient<UpdateHandlers>();

#endregion

#region Hangfire

builder.Services.AddHangfire((sp, config) =>
{
    var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection");
    config.UseSqlServerStorage(connectionString);
});
builder.Services.AddHangfireServer();

#endregion

var app = builder.Build();
// Construct webhook route from the Route configuration parameter
// It is expected that BotController has single method accepting Update
app.MapBotWebhookRoute<BotController>(route: botConfiguration.Route);
app.UseHangfireDashboard("/hangfire", new DashboardOptions()
{
    Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
});
app.MapControllers();
app.Run();


public class BotConfiguration
{
    public static readonly string Configuration = "BotConfiguration";

    public string BotToken { get; init; } = default!;
    public string HostAddress { get; init; } = default!;
    public string Route { get; init; } = default!;
    public string SecretToken { get; init; } = default!;
    public string BotUserName { get; set; } = default!;
}
