namespace BL.Framework.Core.Constants
{
	public static class ValidationConstants 
	{
        public const string MobileRegex = "^(09)[0-9]{2}[0-9]{7}$";

        //public const string PersianCharactersRegex = "^[\u0621-\u0628\u062A-\u063A\u0641-\u0642\u0644-\u0648\u064E-\u0651\u0655\u067E\u0686\u0698\u06A9\u06AF\u06BE\u06CC\u0629\u0643\u0649-\u064B\u064D\u06D5 ]+$";
        //public const string PersianWithMarksCharactersRegex = "^[\u0621-\u0628\u062A-\u063A\u0641-\u0642\u0644-\u0648\u064E-\u0651\u0655\u067E\u0686\u0698\u06A9\u06AF\u06BE\u06CC\u0629\u0643\u0649-\u064B\u064D\u06D5\u060C\u061B\u061F\u0640\u066A\u066B\u066C ]+$";
        //public const string PersianWithMarksAndNumericCharactersRegex = "^[\u0621-\u0628\u062A-\u063A\u0641-\u0642\u0644-\u0648\u064E-\u0651\u0655\u067E\u0686\u0698\u06A9\u06AF\u06BE\u06CC\u0629\u0643\u0649-\u064B\u064D\u06D5\u060C\u061B\u061F\u0640\u066A\u066B\u066C\u06F0-\u06F9\u0660-\u0669 0-9\\(\\)-]+$";

        public const string AlphaWithMarksAndNumericCharactersRegex = @"^[\u0621-\u0628\u062A-\u063A\u0641-\u0642\u0644-\u0648\u064E-\u0651\u0655\u067E\u0686\u0698\u06A9\u06AF\u06BE\u06CC\u0629\u0643\u0649-\u064B\u064D\u06D5\u060C\u061B\u061F\u0640\u066A\u066B\u066C\u06F0-\u06F9\u0660-\u0669 a-zA-Z0-9\(\)\:\-\.\/]+$";
        public const string AlphaCharactersRegex = @"^[\u0621-\u0628\u062A-\u063A\u0641-\u0642\u0644-\u0648\u064E-\u0651\u0655\u067E\u0686\u0698\u06A9\u06AF\u06BE\u06CC\u0629\u0643\u0649-\u064B\u064D\u06D5\u060C\u061B\u061F\u0640\u066A\u066B\u066C\u0660-\u0669 a-zA-Z]+$";
        public const string AlphaWithSpecialMarksAndNumericCharactersRegex = @"^[\u0621-\u0628\u062A-\u063A\u0641-\u0642\u0644-\u0648\u064E-\u0651\u0655\u067E\u0686\u0698\u06A9\u06AF\u06BE\u06CC\u0629\u0643\u0649-\u064B\u064D\u06D5\u060C\u061B\u061F\u0640\u066A\u066B\u066C\u06F0-\u06F9\u0660-\u0669 a-zA-Z0-9_\'\""\(\)\:\*\-\!\?\?\%\/\.\+]+$";

        public const string NumericCharactersRegex = @"^[0-9\u06F0-\u06F9\u0660-\u0669]+$";

        public const string WebsiteUrl = @"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$";

        public const string HexadecimalColor = @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$";

        public static string[] LogoSupportedFormats = new string[] { "image/jpg", "image/jpeg", "image/png" };

        public static string[] CustomerRequestSupportedFormats = new string[] { "application/pdf", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/x-zip-compressed" };
    }
}
