using System;
using System.Collections.Generic;

namespace API
{
    public class Version
    {
        public static string ReleaseVersion { get; } = "v1.0.0.0";
        public static DateTime ReleaseDate { get; } = new DateTime(2020, 11, 01);
        public static List<string> News { get; } = new List<string>()
        {
            "Controller Auth listando dados do usuário logado.",
            "Adicionado nova controller para informações de versão."
        };
        public static List<Version> ReleaseHistory { get; } = new List<Version>() { 
        
        };
    }
}
