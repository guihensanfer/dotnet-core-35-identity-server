using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bom_Dev.Models
{    
    public class Projetos
    {
        public enum Setores
        {
            [Display(Name = "Finanças & Investimentos")]
            Financas_Investimentos = 1,

            [Display(Name = "Vendas na internet")]
            VendasPelaInternet = 2
        }    

        public struct Cores
        {
            public System.Drawing.Color Texto{get;set;}
            public System.Drawing.Color Fundo{get;set;}
        }
        
        public class Projeto
        {
            public Projeto()
            {
                Ativo = true;
                CorIdentidade = new Cores{
                    Texto = System.Drawing.Color.Black,
                    Fundo =  System.Drawing.Color.White
                };
            }

            public string Nome{get;set;}
            public string DescricaoBreve{get;set;}
            public string DescricaoDetalhada{get;set;}
            public Setores Setor{get;set;}
            public int VersaoCompilacao{get;set;}
            public string LinkAcesso{get;set;}
            public bool Ativo{get;set;}
            public Cores CorIdentidade{get;set;}
            public bool EmDesenvolvimentoInicial{
                get{
                    return VersaoCompilacao <= 0;
                }
            }
            public string PalavrasChaves{get;set;}
        }

        public static List<Projeto> InstanciarProjetos() => new List<Projeto>(){
            new Projeto(){
                Nome = "Controle de despesas pessoais",
                DescricaoBreve = "Organização de pendências financeiras  pessoais.",
                DescricaoDetalhada = "Organize suas pendências financeiras de forma fácil e simples. Você possui despesas trimestrais, semestrais ou anuais? Garanta o recurso financeiro para compri-lás e ainda ganhe juros da poupança com isso.",
                LinkAcesso = "https://www.google.com.br/",
                VersaoCompilacao = 0,
                Setor = Setores.Financas_Investimentos,
                Ativo = true,
                CorIdentidade = new Cores{
                    Texto = System.Drawing.Color.FromArgb(49, 212, 117), // Verde
                    Fundo = System.Drawing.Color.FromArgb(50, 50, 50) // Quase preto
                },
                PalavrasChaves = "Finanças; Dinheiro; Controle; Economia; Poupança"                
            }
        };
    }
}