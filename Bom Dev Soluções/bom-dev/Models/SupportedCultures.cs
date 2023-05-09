using System.Collections.Generic;
using System.Globalization;

namespace Bom_Dev.Models
{
    public class SupportedCultures
    {
        public const string DefaultLanguage = "pt-BR";
        public const string ResourcesFolder = "Resources";
        public const string ChangeURLParameter = "alter-language";
        public static List<CultureInfo> GetCultures()
        {
            return new List<CultureInfo>() {
                new CultureInfo(DefaultLanguage),
                new CultureInfo("en-US")
            };
        }
    }
}
