using PdfSharp.Fonts;

namespace Telegram.Bot.Services;

public class SahelFontResolver : IFontResolver
{
    public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
    {
        return
            //familyName.Equals("Sahel", StringComparison.CurrentCultureIgnoreCase)
            //? 
            new FontResolverInfo("Files/Sahel.ttf")
            //: null
            ;
    }

    public byte[]? GetFont(string faceName)
    {
        using (var ms = new MemoryStream())
        {
            using (var fs = File.Open(faceName, FileMode.Open))
            {
                fs.CopyTo(ms);
                ms.Position = 0;
                return ms.ToArray();
            }
        }
    }
}
