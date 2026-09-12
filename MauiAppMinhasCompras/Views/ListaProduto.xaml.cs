using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    // Lista observável que notifica a interface gráfica automaticamente ao adicionar ou remover itens
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();

        // Vincula a lista observável como fonte de dados do ListView
        lst_produtos.ItemsSource = lista;
    }

    // Executado sempre que a página é exibida na tela
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

    // Método auxiliar para recarregar a lista respeitando o filtro de categoria selecionado
    private async Task CarregarProdutos()
    {
        lista.Clear();

        string? categoriaSelecionada = pck_categoria.SelectedItem?.ToString();

        List<Produto> tmp;

        // Se não selecionou nada ou escolheu "Todas", busca tudo no banco
        if (string.IsNullOrEmpty(categoriaSelecionada) || categoriaSelecionada == "Todas")
        {
            tmp = await App.Db.GetAll();
        }
        else
        {
            // Busca filtrada por categoria no SQLite
            tmp = await App.Db.GetByCategoria(categoriaSelecionada);
        }

        tmp.ForEach(i => lista.Add(i));
    }

    // NOVO: Evento disparado ao selecionar uma categoria no Picker
    private async void pck_categoria_SelectedIndexChanged(object? sender, EventArgs e)
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

    // Ação do botão "Adicionar" na barra de ferramentas (Toolbar)
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

    // Evento disparado a cada caractere digitado na barra de pesquisa
    private async void txt_search_TextChanged(object? sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;
            lista.Clear();

            // Realiza a busca filtrada no banco de dados por texto
            List<Produto> tmp = await App.Db.Search(q);

            // Opcional: Se houver uma categoria selecionada no Picker que não seja "Todas", filtra em memória
            string? categoria = pck_categoria.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(categoria) && categoria != "Todas")
            {
                tmp = tmp.Where(p => p.Categoria == categoria).ToList();
            }

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }

    // Ação do botão "Somar" na barra de ferramentas
    private async void ToolbarItem_Clicked_1(object? sender, EventArgs e)
    {
        try
        {
            // Calcula o total acumulado dos itens que estão atualmente visíveis na tela
            double soma = lista.Sum(i => i.Total);

            string msg = $"O total dos produtos filtrados é {soma:C}";

            await DisplayAlertAsync("Total dos Produtos", msg, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }

    // Ação do menu contextual "Remover" (ViewCell ContextActions)
    private async void MenuItem_Clicked(object? sender, EventArgs e)
    {
        try
        {
            MenuItem? selecionado = sender as MenuItem;

            if (selecionado == null)
                return;

            Produto? p = selecionado.BindingContext as Produto;

            if (p == null)
                return;

            bool confirm = await DisplayAlertAsync(
                "Tem Certeza?", $"Remover {p.Descricao}?", "Sim", "Não");

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

    // Evento disparado ao tocar em um item da lista
    private async void lst_produtos_ItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            if (e.SelectedItem == null)
                return;

            Produto? p = e.SelectedItem as Produto;

            if (p == null)
                return;

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

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {
            await CarregarProdutos();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
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

}