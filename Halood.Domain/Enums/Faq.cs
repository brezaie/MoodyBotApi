using System.ComponentModel;

namespace Halood.Domain.Enums;

public enum Faq
{
    [Description("هدف از بات چیست؟")]
    [Route("/faq_reply WhatTheBotIsFor")]
    WhatTheBotIsFor = 1,

    [Description("چطور با بات کار کنم؟")]
    [Route("/faq_reply HowToWorkWithBot")]
    HowToWorkWithBot = 2,

    [Description("چطور یادآورها را تغییر دهم؟")]
    [Route("/faq_reply HowToChangeReminders")]
    HowToChangeReminders = 3,

    [Description("گزارش هفتگی چه زمانی ارسال می‌شود؟")]
    [Route("/faq_reply WhenWeeklyReportIsSent")]
    WhenWeeklyReportIsSent = 4,

    [Description("چگونه مشکلی را گزارش کنم؟")]
    [Route("/faq_reply HowToReportProblem")]
    HowToReportProblem = 5,
}
