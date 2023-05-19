namespace Data.Models
{    
    public class Optimization
    {
        public const int SizePerPageDefault = 2;
        public enum LoadedColumnsLevel
        {
            A, // carrega todas as colunas possiveis
            B, // carrega algumas colunas
            C // carrega poucas colunas (geralmente utilizado para selectListItem)
        }

        public Optimization() { }
        public Optimization(LoadedColumnsLevel loadedColumnsLevel, int page, int sizePerPage = SizePerPageDefault) {
            LoadedColumns = loadedColumnsLevel;
            Page = page;
            SizePerPage = sizePerPage;
        }

        /// <summary>
        /// Nota de resultados carregados possiveis.
        /// </summary>
        public LoadedColumnsLevel LoadedColumns { get; set; } = LoadedColumnsLevel.A;

        /// <summary>
        /// Total de itens por resultado.
        /// </summary>
        public int SizePerPage { get; set; } = SizePerPageDefault;

        /// <summary>
        /// Pagina que se quer ler.
        /// </summary>
        public int Page { get; set; } = 1;
    }
}
