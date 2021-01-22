using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace API.Models
{    
    public class Projects
    {
        public enum Setores
        {
            [Display(Name = "Finanças & Investimentos")]
            FinancasInvestimentos = 1,

            [Display(Name = "Soluções empresariais")]
            SolucoesEmpresariais = 2
        }    

        public struct Cores
        {
            public System.Drawing.Color foreColor{get;set;}
            public System.Drawing.Color background{get;set;}
        }
        
        public class Projeto
        {
            public Projeto()
            {
                enable = true;
                colorId = new Cores{
                    foreColor = System.Drawing.Color.Black,
                    background =  System.Drawing.Color.White
                };
            }

            public string name{get;set;}
            public string description{get;set;}
            public string bio{get;set;}
            public Setores department{get;set;}
            public int compilationVersion{get;set;}
            public string linkAccess{get;set;}            
            public bool enable{get;set;}
            public Cores colorId{get;set;}
            public bool EmDesenvolvimentoInicial{
                get{
                    return compilationVersion <= 0;
                }
            }
            public string keywords{get;set;}
            public DateTime? releaseDate{get;set;}
        }

        public static List<Projeto> GetProjects() => new List<Projeto>(){
            new Projeto(){
                name = "Controle de despesas pessoais",
                description = "Organização de pendências financeiras  pessoais.",
                bio = "Organize suas pendências financeiras de forma fácil e simples. Você possui despesas trimestrais, semestrais ou anuais? Garanta o recurso financeiro para compri-lás e ainda ganhe juros da poupança com isso.",
                linkAccess = "https://www.google.com.br/",
                compilationVersion = 0,
                department = Setores.FinancasInvestimentos,
                enable = true,
                colorId = new Cores{
                    foreColor = System.Drawing.Color.FromArgb(49, 212, 117), // Verde
                    background = System.Drawing.Color.FromArgb(50, 50, 50) // Quase preto
                },
                keywords = "Finanças; Dinheiro; Controle; Economia; Poupança",
                releaseDate = new DateTime(2020, 9, 15)
            },
            new Projeto(){
                name = "Padronizei",
                description = "Base de conhecimento para organizações.",
                bio = "",
                linkAccess = "https://www.google.com.br/",
                compilationVersion = 0,
                department = Setores.SolucoesEmpresariais,
                enable = true,
                colorId = new Cores{
                    foreColor = System.Drawing.Color.Orange, // Verde
                    background = System.Drawing.Color.FromArgb(110, 72, 29) // Quase preto
                },
                keywords = "Base de conhecimento; Gestão da informação; Engajamento",
                releaseDate = null
            },
        };
    }
}