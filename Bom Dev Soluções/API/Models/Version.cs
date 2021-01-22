using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Models
{
    public class Version
    {        
        public Version(string releaseVersion, DateTime releaseDate, List<string> news)
        {
            this.releaseDate = releaseDate;            
            this.releaseVersion = releaseVersion;            
            this.news = news;                    
        }

        public string releaseVersion { get; private set; } = "v1.0.0.1";

        public DateTime releaseDate { get; private set; } = new DateTime(2021, 01, 22);

        public List<string> news { get; private set; } = new List<string>()
        {
            "Modificado padrão de nomenclatura das models para Camel Case e mudado para inglês como padrão.",            
        };
        
        public static List<Version> GetVersions() => new List<Version>() { 
            new Version("1.0.0.0", new DateTime(2021, 01, 01), new List<string>(){ 
                "Implementado funcionamento do controle de versão desta API."
            }),
            new Version("1.0.0.1", new DateTime(2021, 01, 22), new List<string>(){
                "Alterado para inglês as model e padrão camel case nas propriedades."
            }),
        }.OrderByDescending(x => x.releaseDate)
        .ToList();
    }
}
