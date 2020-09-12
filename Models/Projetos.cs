using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bom_Dev.Models
{    
    public class Projetos
    {
        public enum Setores
        {
            [Display(Name = "Finanças & Investimentos")]
            Financas_Investimentos = 1
        }    
        
        public class Projeto
        {
            public string Nome{get;set;}
            public string DescricaoBreve{get;set;}
            public string DescricaoDetalhada{get;set;}
            public int VersaoCompilacao{get;set;}
            public string LinkAcesso{get;set;}
        }

        public static List<Projeto> InstanciarProjetos() => new List<Projeto>(){
            new Projeto(){
                Nome = "Controle de despesas pessoais",
                DescricaoBreve = "Organização de pendências financeiras  pessoais.",
                DescricaoDetalhada = "Organize suas pendências financeiras de forma fácil e simples. Você possui despesas trimestrais, semestrais ou anuais? Garanta o recurso financeiro para compri-lás e ainda ganhe juros da poupança com isso.",
                LinkAcesso = "#",
                VersaoCompilacao = 1
            }
        };
    }
}