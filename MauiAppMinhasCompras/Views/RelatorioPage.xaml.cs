
/*Essa pagina tem o objetivo de exibir um relatório de gastos por categoria.*/
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class RelatorioPage : ContentPage
{
    public RelatorioPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // 1. Busca todos os produtos gravados no SQLite
            List<Produto> listaProdutos = await App.Db.GetAll();

            // 2. Agrupa por categoria e calcula o total gasto
            var relatorio = listaProdutos
                .GroupBy(p => string.IsNullOrWhiteSpace(p.Categoria) ? "Sem Categoria" : p.Categoria)
                .Select(g => new RelatorioCategoria
                {
                    Categoria = g.Key,
                    TotalGasto = g.Sum(p => p.Total) // Usa a propriedade Total existente na sua model
                })
                .OrderByDescending(r => r.TotalGasto)
                .ToList();

            // 3. Associa o resultado à lista da tela
            lst_relatorio.ItemsSource = relatorio;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }
}