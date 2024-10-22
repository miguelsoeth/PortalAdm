using System.ComponentModel;
using System.Reflection;

namespace PortalAdm.SharedKernel;

public static class EnumExtensions
{
    public static T GetEnumValueFromDescription<T>(string description) where T : Enum
    {
        foreach (var field in typeof(T).GetFields())
        {
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            if (attribute != null && attribute.Description == description)
            {
                return (T)field.GetValue(null);
            }
        }

        throw new ArgumentException($"No enum with description '{description}' found.", nameof(description));
    }
}