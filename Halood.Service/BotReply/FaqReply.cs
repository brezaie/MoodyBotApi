using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Halood.Domain.Interfaces.User;
using Halood.Domain.Interfaces.UserEmotion;
using Halood.Service.BotCommand;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Halood.Service.BotReply;

public class FaqReply : IBotReply
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<NoCommand> _logger;
    private string _text = string.Empty;

    public FaqReply(ITelegramBotClient botClient, ILogger<NoCommand> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        var givenFaq = message.Text.Split(" ")[1];
        if (givenFaq == Faq.WhatTheBotIsFor.GetRoute().Split(" ")[1])
        {
            _text = $"هدف از ایجاد این بات (حالود)، کمک به شناخت بهتر خودمان با دیدن و ثبت احساس‌ها هست.\n\nدر کنار این موضوع، با ثبت میزان رضایت از زندگی، می‌توانیم روند روزانه آن را ثبت و در قالب گزارش‌های هفتگی، دریافت کنیم.\n";
        }
        else if (givenFaq == Faq.HowToWorkWithBot.GetRoute().Split(" ")[1])
        {
            _text = $"شما می‌توانید با استفاده از گزینه Menu، که در پایین آمده، گزینه مورد نظر برای ثبت رضایت از زندگی یا احساس خود را انتخاب کنید.\n\nهم‌چنین، یادآورهایی وجود دارند که به شما کمک می‌کنند این موارد را ثبت کنید؛ یادآور میزان رضایت از زندگی، هر شب، ساعت 22 (به وقت تهران) پیامی برای شما ارسال می‌کند و یادآور ثبت احساس، در ساعت‌های 11 و 19 (به وقت تهران) پیامی برای شما ارسال می‌کند.\n";
        }
        else if (givenFaq == Faq.HowToChangeReminders.GetRoute().Split(" ")[1])
        {
            _text = $"شما می‌توانید یادآور رضایت از زندگی را از طریق زیر (با استفاده از Menu) فعال/غیرفعال کنید:\nتغییر تنظیمات => یادآور رضایت از زندگی\n\nهم‌چنین برای تغییر ساعت‌های یادآور احساس و کم/زیاد کردن تعداد یادآورها می‌توانید از طریق زیر (با استفاده از Menu) اقدام کنید:\nتغییر تنظیمات => یادآور احساس‌ها\n";
        }
        else if (givenFaq == Faq.WhenWeeklyReportIsSent.GetRoute().Split(" ")[1])
        {
            _text = $"گزارش هفتگی شنبه‌ها، ساعت 8 صبح (به وقت تهران) برای کاربران ارسال می‌شود. این گزارش شامل رکوردهایی است که کاربر در طی هفته قبل در ارتباط با میزان رضایت از زندگی و احساس‌ها ثبت کرده است.";
        }
        else if (givenFaq == Faq.HowToReportProblem.GetRoute().Split(" ")[1])
        {
            _text = $"در صورتی که هنگام کار با بات به مشکلی برخوردید یا سوالی داشتید، می‌توانید آن را از طریق زیر، با ما مطرح کنید:\n@brezaie\n";
        }

        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            cancellationToken: cancellationToken
        );
    }
}
