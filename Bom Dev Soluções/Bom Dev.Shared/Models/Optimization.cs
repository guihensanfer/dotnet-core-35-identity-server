namespace Data.Models
{    
    public class Optimization
    {
        public enum LoadedColumnsLevel
        {
            A, // carrega todas as colunas possiveis
            B, // carrega algumas colunas
            C // carrega poucas colunas (geralmente utilizado para selectListItem)
        }

        public Optimization() { }
        public Optimization(LoadedColumnsLevel loadedColumnsLevel) {
            this.LoadedColumns = loadedColumnsLevel;
        }

        /// <summary>
        /// Nota de resultados carregados possiveis.
        /// </summary>
        public LoadedColumnsLevel LoadedColumns { get; set; } = LoadedColumnsLevel.A;        

        /// <summary>
        /// Total de itens por resultado.
        /// </summary>
        public short? Take { get; set; }
    }
}
