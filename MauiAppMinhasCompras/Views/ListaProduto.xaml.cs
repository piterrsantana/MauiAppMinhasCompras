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
            lista.Clear();

            // Busca todos os produtos salvos no banco de dados
            List<Produto> tmp = await App.Db.GetAll();

            // Adiciona cada produto encontrado na lista observável
            tmp.ForEach(i => lista.Add(i));
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
            // Navega para a página de inclusão de novo produto
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

            // Realiza a busca filtrada no banco de dados
            List<Produto> tmp = await App.Db.Search(q);

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
            // Calcula o total acumulado de todos os itens atualmente na lista
            double soma = lista.Sum(i => i.Total);

            string msg = $"O total é {soma:C}";

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

            // Solicita confirmação do usuário antes de deletar
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

            // Limpa a seleção para permitir selecionar o mesmo item novamente
            lst_produtos.SelectedItem = null;

            // Abre a tela de edição passando o produto selecionado no BindingContext
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