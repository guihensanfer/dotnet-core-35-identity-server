using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Data.Models
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

            if (culture != null)
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

        public static CultureInfo ParseToCultureOrDefault(CultureInfo culture)
        {
            if (culture == null)
            {
                return null;
            }

            return ParseToCultureOrDefault(culture.Name);
        }

        /// <summary>
        /// Translate data JSON for current or parameter language.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string TryParseTranslation(string value, CultureInfo language = null)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (!value.StartsWith('{') || !value.EndsWith('}'))
                return null;

            if (!Utility.IsValidJson(value))
                return null;

            try
            {
                var cultures = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);

                if (cultures != null)
                {
                    string languageName = null;

                    if (language == null)
                    {
                        languageName = CultureInfo.CurrentCulture.Name;
                    }
                    else
                    {
                        languageName = language.Name;
                    }

                    // validation culture
                    var cultureObject = ParseToCultureOrDefault(languageName);
                    languageName = cultureObject.Name;

                    foreach (var culture in cultures)
                    {
                        if (string.Equals(culture.Key, languageName, StringComparison.OrdinalIgnoreCase))
                            return culture.Value;
                    }
                }
            }
            catch { }

            return null;
        }
    }
}
