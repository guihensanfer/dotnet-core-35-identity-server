namespace Data.Models
{
    public enum LoadedColumnsLevel
    {
        A, // carrega todas as colunas possiveis
        B, // carrega algumas colunas
        C // carrega poucas colunas (geralmente utilizado para selectListItem)
    }
    public class Optimization
    {
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
