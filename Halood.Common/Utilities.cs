using System.ComponentModel;
using System.Reflection;
using Halood.Domain;

namespace Halood.Common;

public static class Utilities
{
    public static string GetDescription(this Enum value)
    {
        Type type = value.GetType();
        string name = Enum.GetName(type, value);
        if (name != null)
        {
            FieldInfo field = type.GetField(name);
            if (field != null)
            {
                DescriptionAttribute attr =
                    Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attr != null)
                {
                    return attr.Description;
                }
            }
        }
        return value.ToString();
    }

    public static string GetRoute(this Enum value)
    {
        Type type = value.GetType();
        string name = Enum.GetName(type, value);
        if (name != null)
        {
            FieldInfo field = type.GetField(name);
            if (field != null)
            {
                RouteAttribute attr =
                    Attribute.GetCustomAttribute(field, typeof(RouteAttribute)) as RouteAttribute;
                if (attr != null)
                {
                    return attr.Path;
                }
            }
        }
        return value.ToString();
    }
}
