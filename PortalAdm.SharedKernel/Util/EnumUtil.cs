using System.ComponentModel;
using System.Reflection;

namespace PortalAdm.SharedKernel.Util;

public static class EnumUtil
{
    public static string GetEnumDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }
    
    public static TEnum GetEnumFromDescription<TEnum>(string description) where TEnum : Enum
    {
        var enumType = typeof(TEnum);
        var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (var field in fields)
        {
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            if (attribute != null && attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
            {
                return (TEnum)field.GetValue(null);
            }
        }

        throw new ArgumentException($"No enum value found for description '{description}'", nameof(description));
    }
}