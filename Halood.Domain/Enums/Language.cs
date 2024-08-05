using System.ComponentModel;

namespace Halood.Domain.Enums;

public enum Language
{
    Unknown = 0,

    [Description("Persian")]
    [Route("/change_language_reply Persian")]
    Persian = 1,

    [Description("English")]
    [Route("/change_language_reply English")]
    English = 2
}
