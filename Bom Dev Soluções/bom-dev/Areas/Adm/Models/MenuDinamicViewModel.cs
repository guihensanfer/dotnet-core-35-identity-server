using System;
using System.Collections.Generic;
using System.Linq;

namespace Bom_Dev.Areas.Adm.Models
{
    public class MenuDinamicViewModel
    {
        public const string ParamSearchMenu = "search";

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool Enabled { get; set; } = true;        

        public List<MenuDinamicViewModel> Chieldres { get; set; }

        public static MenuDinamicViewModel GetMenuFromUrl(string url, List<MenuDinamicViewModel> menuDinamicViewModels) =>
            menuDinamicViewModels
            .Where(x => url.Contains(x.Url, System.StringComparison.OrdinalIgnoreCase))
            .FirstOrDefault();

        public static List<MenuDinamicViewModel> SearchMenuChieldByName(string name, List<MenuDinamicViewModel> menuDinamicViewModels)
        {
            List<MenuDinamicViewModel> result = new List<MenuDinamicViewModel>();

            menuDinamicViewModels.ForEach(x => result.AddRange(x.Chieldres));
            result.RemoveAll(x => !x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

            return result?.OrderBy(x => x.Name).ToList();
        }

        public static List<MenuDinamicViewModel> GetAllMenus()
        {
            return new List<MenuDinamicViewModel>()
            {
                new MenuDinamicViewModel()
                {
                    Name = "CATEGORIAS",
                    Url="/adm/category",
                    Chieldres = new List<MenuDinamicViewModel>()
                    {
                        new MenuDinamicViewModel()
                        {
                            Name = "Gerenciar categorias",
                            Url="/adm/category"
                        },
                        new MenuDinamicViewModel()
                        {
                            Name = "Adicionar categoria",
                            Url="/adm/category/create"
                        }                        
                    }
                }
            }
            .Where(x => x.Enabled)            
            .ToList();
        }
    }
}
