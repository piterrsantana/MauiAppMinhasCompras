//Aqui está a classe pública que representa um relatório de gastos por categoria com as propriedades necessárias.

namespace MauiAppMinhasCompras.Models
{
    public class RelatorioCategoria
    {
        public string Categoria { get; set; } = string.Empty;
        public double TotalGasto { get; set; }
    }
}