using System.ComponentModel;

namespace Halood.Domain.Enums;

public enum SatisfactionLevel
{
    [Description("خیلی کم 😥")]
    [Route("/record_satisfaction_reply Awful")]
    Awful = 1,
    
    [Description("کم 😞")]
    [Route("/record_satisfaction_reply Bad")]
    Bad = 2,

    [Description("متوسط 😏")]
    [Route("/record_satisfaction_reply SoSo")]
    SoSo = 3,

    [Description("زیاد 😊")]
    [Route("/record_satisfaction_reply Good")]
    Good = 4,

    [Description("خیلی زیاد 😍")]
    [Route("/record_satisfaction_reply Perfect")]
    Perfect = 5,
}
