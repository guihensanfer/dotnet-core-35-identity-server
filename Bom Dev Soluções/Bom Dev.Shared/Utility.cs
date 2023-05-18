using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class Utility
    {
        public const string copyright = "(c) Copyright 2021 Bom Dev. Todos os direitos reservados.";

        private static bool IsValidJson(string input)
        {
            try
            {
                JToken.Parse(input);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string TryParseTranslation(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            
            if (!value.StartsWith('{') || !value.EndsWith('}'))
                return null;

            if (!IsValidJson(value))
                return null;

            try
            {
                var cultures = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);

                if (cultures != null)
                {
                    var currentLanguage = System.Globalization.CultureInfo.CurrentCulture.Name;                    

                    foreach(var culture in cultures)
                    {
                        if(string.Equals(culture.Key, currentLanguage, StringComparison.OrdinalIgnoreCase))
                            return culture.Value;
                    }                                        
                }
            }
            catch { }          

            return null;
        }
    }    
}
