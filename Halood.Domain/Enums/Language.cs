using System.ComponentModel;

namespace Halood.Domain.Enums;

public enum Language
{
    Unknown = 0,

    [Description("Persian")]
    Persian = 1,

    [Description("English")]
    English = 2
}
