using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Models
{
    public class Version
    {
        public Version(string releaseVersion = null, DateTime? releaseDate = null, List<string> news = null)
        {
            if(releaseDate.HasValue)
                this.releaseDate = releaseDate.Value;

            if (!string.IsNullOrEmpty(releaseVersion))
                this.releaseVersion = releaseVersion;

            if (news != null)
                this.news = news;
        }

        public string releaseVersion { get; private set; } = "v1.0.0.1";

        public DateTime releaseDate { get; private set; } = new DateTime(2021, 01, 22);

        public List<string> news { get; private set; } = new List<string>()
        {
            "Modificado padrão de nomenclatura das models para Camel Case e mudado para inglês como padrão.",            
        };

        public List<Version> releaseHistory {
            get {
                return releaseHistory?
                    .OrderByDescending(x => x.releaseDate)
                    .ToList();
            }
            set
            {
                releaseHistory = new List<Version>() {
                    new Version("v1.0.0.0", new DateTime(2021, 01, 20), new List<string>()
                    {
                        "Controller Auth listando dados do usuário logado.",
                        "Adicionado nova controller para informações de versão."
                    })
                };
            }
        }         
    }
}
