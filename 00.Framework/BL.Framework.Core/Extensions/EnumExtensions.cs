using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace BL.Framework.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var displayName = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault()
                .GetCustomAttribute<DisplayAttribute>()?
                .GetName();

            if (string.IsNullOrEmpty(displayName))
            {
                displayName = enumValue.ToString();
            }

            return displayName;
        }
    }
}
