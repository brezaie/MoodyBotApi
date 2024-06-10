using System.ComponentModel;

namespace Halood.Domain.Enums;

public enum YesNoResponse
{
    Unknown  = 0,

    [Description("Yes")]
    Yes = 1,

    [Description("No")]
    No = 2
}
