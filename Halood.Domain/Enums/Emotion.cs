using System.ComponentModel;

namespace Halood.Domain.Enums;

public enum Emotion
{
    Unknown = 0,

    [Description("آرامش")]
    Serenity = 1,

    [Description("شادی")]
    Joy = 2,

    [Description("وجد")]
    Ecstacy = 3,

    [Description("عشق")]
    Love = 4,

    [Description("پذیرش")]
    Acceptance = 5,

    [Description("اعتماد")]
    Trust = 6,

    [Description("تحسین")]
    Admiration = 7,

    [Description("سلطه‌پذیری")]
    Submission = 8,

    [Description("دلهره")]
    Apprehension = 9,

    [Description("ترس")]
    Fear = 10,

    [Description("وحشت")]
    Terror = 11,

    [Description("حیرت")]
    Awe = 12,

    [Description("حواس‌پرتی")]
    Distraction = 13,

    [Description("تعجب")]
    Surprise = 14,

    [Description("شگفتی")]
    Amazement = 15,

    [Description("مخالفت")]
    Disapproval = 16,

    [Description("پکری")]
    Pensiveness = 17,

    [Description("ناراحتی")]
    Sadness = 18,

    [Description("سوگ")]
    Grief = 19,

    [Description("پشیمانی")]
    Remorse = 20,

    [Description("ملال")]
    Boredom = 21,

    [Description("بیزاری")]
    Disgust = 22,

    [Description("انزجار")]
    Loathing = 23,

    [Description("تحقیر")]
    Contempt = 24,

    [Description("رنج")]
    Annoyance = 25,

    [Description("خشم")]
    Anger = 26,

    [Description("جنون")]
    Rage = 27,

    [Description("پرخاش‌گری")]
    Aggressiveness = 28,

    [Description("دل‌بستگی")]
    Interest = 29,

    [Description("انتظار")]
    Anticipation = 30,

    [Description("هشیاری")]
    Vigilance = 31,

    [Description("خوش‌بینی")]
    Optimism = 32
}
