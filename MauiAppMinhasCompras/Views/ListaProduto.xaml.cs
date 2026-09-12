using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        try
        {
            await CarregarProdutos();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }

    // Método centralizado para carregar os produtos aplicando filtros de Categoria e Busca
    private async Task CarregarProdutos()
    {
        try
        {
            lista.Clear();

            string? categoriaSelecionada = pck_categoria?.SelectedItem?.ToString();
            string termoBusca = txt_search?.Text ?? string.Empty;

            List<Produto> tmp;

            if (!string.IsNullOrWhiteSpace(termoBusca))
            {
                tmp = await App.Db.Search(termoBusca);
            }
            else
            {
                tmp = await App.Db.GetAll();
            }

            if (!string.IsNullOrEmpty(categoriaSelecionada) && categoriaSelecionada != "Todas")
            {
                tmp = tmp.Where(p => p.Categoria == categoriaSelecionada).ToList();
            }

            if (tmp != null)
            {
                foreach (var item in tmp)
                {
                    lista.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }

    private async void pck_categoria_SelectedIndexChanged(object? sender, EventArgs e)
    {
        await CarregarProdutos();
    }

    private async void txt_search_TextChanged(object? sender, TextChangedEventArgs e)
    {
        await CarregarProdutos();
    }

    private async void lst_produtos_Refreshing(object? sender, EventArgs e)
    {
        try
        {
            if (txt_search != null)
            {
                txt_search.Text = string.Empty;
            }

            await CarregarProdutos();
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private async void ToolbarItem_Clicked(object? sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }

    private async void ToolbarItem_Clicked_1(object? sender, EventArgs e)
    {
        try
        {
            double soma = lista.Sum(i => i.Total);
            string msg = $"O total dos produtos filtrados é {soma:C}";

            await DisplayAlertAsync("Total dos Produtos", msg, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }

    private async void ToolbarItem_Relatorio_Clicked(object? sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new Views.RelatorioPage());
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }

    private async void MenuItem_Clicked(object? sender, EventArgs e)
    {
        try
        {
            MenuItem? selecionado = sender as MenuItem;
            if (selecionado == null) return;

            Produto? p = selecionado.BindingContext as Produto;
            if (p == null) return;

            bool confirm = await DisplayAlertAsync("Tem Certeza?", $"Remover {p.Descricao}?", "Sim", "Não");

            if (confirm)
            {
                await App.Db.Delete(p.Id);
                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }

    private async void lst_produtos_ItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            if (e.SelectedItem == null) return;

            Produto? p = e.SelectedItem as Produto;
            if (p == null) return;

            lst_produtos.SelectedItem = null;

            await Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p
            });
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }
}