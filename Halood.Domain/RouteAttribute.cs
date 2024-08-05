namespace Halood.Domain;

public class RouteAttribute : Attribute
{
    public string Path { get; private set; }

    public RouteAttribute(string path)
    {
        this.Path = path;
    }
}
