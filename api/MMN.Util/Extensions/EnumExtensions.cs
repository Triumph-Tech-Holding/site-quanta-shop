using PagarMe.Base;
using System;
using System.ComponentModel;
using System.Reflection;

namespace MMN.Util.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T enumValue)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                return null;

            var description = enumValue.ToString();
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            if (fieldInfo != null)
            {
                var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    description = ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return description;
        }

        public static string GetEnumValue<T>(this T enumValue)
        {
            FieldInfo field = enumValue.GetType().GetField(enumValue.ToString());
            EnumValueAttribute attribute = field.GetCustomAttribute<EnumValueAttribute>();

            return attribute == null ? enumValue.ToString() : attribute.Value;
        }

        //public static string GetDisplayName(this Enum enumValue)
        //{
        //    return enumValue.GetType()
        //                    .GetMember(enumValue.ToString())
        //                    .First()
        //                    .GetCustomAttribute<DisplayAttribute>()
        //                    .GetName();
        //}
    }
}
