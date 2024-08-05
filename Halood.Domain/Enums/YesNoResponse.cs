using System.ComponentModel;

namespace Halood.Domain.Enums;

public enum YesNoResponse
{
    Unknown  = 0,

    [Description("Yes")]
    [Route("/toggle_satisfaction_reminder_reply Yes")]
    Yes = 1,

    [Description("No")]
    [Route("/toggle_satisfaction_reminder_reply No")]
    No = 2
}
