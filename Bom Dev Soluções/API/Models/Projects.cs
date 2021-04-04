using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace API.Models
{    
    public class Projects
    {
        public enum Setores
        {
            [Display(Name = "Finanças pessoais & Investimentos")]
            FinancasEInvestimentos = 0,
        }    

        public struct Cores
        {
            public System.Drawing.Color foreColor{get;set;}
            public System.Drawing.Color background{get;set;}
        }
        
        public class Project
        {
            public Project()
            {
                enable = true;
                colorId = new Cores{
                    foreColor = System.Drawing.Color.Black,
                    background =  System.Drawing.Color.White
                };
            }

            public string name{get;set;}
            public string description{get;set;}

            [StringLength(100)]
            public string bio{get;set;}
            public Setores department{get;set;}
            public int compilationVersion{get;set;}
            public string linkAccess{get;set;}            
            public bool enable{get;set;}
            public Cores colorId{get;set;}
            public bool hasRelease{
                get{
                    return compilationVersion <= 0;
                }
            }
            public string keywords{get;set;}
            public DateTime? releaseDate{get;set;}
        }

        public static List<Project> GetProjects() => new List<Project>(){
            new Project(){
                name = "Potes de Ouro",
                description = "Solução para pendências financeiras pessoais.",
                bio = "Organize suas pendências financeiras de forma fácil e simples.",
                linkAccess = "https://www.google.com.br/",
                compilationVersion = 0,
                department = Setores.FinancasEInvestimentos,
                enable = true,
                colorId = new Cores{
                    foreColor = System.Drawing.Color.Orange,
                    background = System.Drawing.Color.FromArgb(110, 72, 29)
                },
                keywords = "Finanças; Dinheiro; Controle; Economia; Poupança",
                releaseDate = new DateTime(2020, 9, 15)
            },
            new Project(){
                name = "Bom Invest",
                description = "Plataforma de compartilhamento de carteiras de investimentos.",
                bio = "Torne sua carteira de investimentos pública e compartilhe sua performance com outros investidores.",
                linkAccess = "https://www.google.com.br/",
                compilationVersion = 0,
                department = Setores.FinancasEInvestimentos,
                enable = true,
                colorId = new Cores{
                    foreColor = System.Drawing.Color.FromArgb(49, 212, 117), // Verde
                    background = System.Drawing.Color.FromArgb(50, 50, 50) // Quase preto                    
                },
                keywords = "Bolsa de valores; Investimentos; Ranking; Top;",
                releaseDate = null
            },
        };
    }
}