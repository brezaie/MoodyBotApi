namespace Halood.Domain;

public class ColorAttribute : Attribute
{
    public string HexCode { get; private set; }

    public ColorAttribute(string hexCode)
    {
        this.HexCode = hexCode;
    }
}
