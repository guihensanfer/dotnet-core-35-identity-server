using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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

        public static CultureInfo ParseToCultureOrDefault(string language)
        {            
            var culture = new CultureInfo(language);
            
            if(culture != null)
            {
                var supportedCultures = GetCultures();
                bool match = supportedCultures.Any(x => x.Equals(culture));

                if (match)
                {
                    return culture;
                }
            }

            // default
            return new CultureInfo(DefaultLanguage);
        }
    }
}
