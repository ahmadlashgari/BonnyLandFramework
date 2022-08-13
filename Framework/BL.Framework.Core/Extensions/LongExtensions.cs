using System;
using System.Globalization;

namespace BL.Framework.Core.Extensions
{
	public static class LongExtensions
	{
        public static string ConvertBytesToSize(this long bytes)
        {
	        var sizes = new [] { "Bytes", "KB", "MB", "GB", "TB" };

	        if (bytes == 0)
	        {
		        return "0 Byte";
	        }

	        var i = int.Parse((Math.Floor(Math.Log(bytes) / Math.Log(1024))).ToString(CultureInfo.InvariantCulture));

	        return $"{Math.Round(bytes / Math.Pow(1024, i))} {sizes[i]}";
        }
    }
}
