using System.ComponentModel;

namespace Halood.Domain.Enums;

public enum SatisfactionLevel
{
    [Description("خیلی کم 😥")]
    [Route("/record_satisfaction_reply Awful")]
    [Color("#960019")]
    Awful = 1,
    
    [Description("کم 😞")]
    [Route("/record_satisfaction_reply Bad")]
    [Color("#FF0038")]
    Bad = 2,

    [Description("متوسط 😏")]
    [Route("/record_satisfaction_reply SoSo")]
    [Color("#FFE135")]
    SoSo = 3,

    [Description("زیاد 😊")]
    [Route("/record_satisfaction_reply Good")]
    [Color("#A4C739")]
    Good = 4,

    [Description("خیلی زیاد 😍")]
    [Route("/record_satisfaction_reply Perfect")]
    [Color("#008001")]
    Perfect = 5,
}
