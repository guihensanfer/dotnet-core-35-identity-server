using System;
using System.Collections.Generic;

namespace API
{
    public class Version
    {
        public string ReleaseVersion { get; } = "v1.0.0.0";
        public DateTime ReleaseDate { get; } = new DateTime(2021, 01, 20);
        public List<string> News { get; } = new List<string>()
        {
            "Controller Auth listando dados do usuário logado.",
            "Adicionado nova controller para informações de versão."
        };
        public List<Version> ReleaseHistory { get; } = new List<Version>() { 
        
        };
    }
}
