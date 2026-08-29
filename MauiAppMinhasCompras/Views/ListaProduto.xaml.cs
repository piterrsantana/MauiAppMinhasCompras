using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>(); //reage na interface automaticamente. Mostra tudo que tem na lista se nada for digitado

    public ListaProduto()
    {
        InitializeComponent();

        lst_produtos.ItemsSource = lista; //liga o 'lst_produtos' da tela XAML com a 'lista' do C# e remove direto da ObservableCollection
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // Limpa a lista existente para não duplicar os itens ao voltar de outra tela
            lista.Clear(); //Limpa a ObservableCollection para não duplicar itens na tela

            List<Produto> tmp = await App.Db.GetAll(); // Busca todos os produtos gravados no SQLite
            int numero = 1;

            foreach (var item in tmp) // PASSO B: Adiciona item por item na ObservableCollection para exibir na tela
            {
                item.Numero = numero;
                lista.Add(item);

                numero++; //Inserido  instrução para iniciar com o nº1 e ir adicioando  de 1 em 1
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        { //Comando para navegar do botão Adicionar para a tela NovoProduto
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlertAsync("Ops", ex.Message, "Ok");
        }
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            var menuItem = sender as MenuItem;
            var produto = menuItem?.BindingContext as Produto;

            if (produto != null)
            {
                bool confirm = await DisplayAlertAsync(
                    "Confirmação",
                    $"Deseja remover '{produto.Descricao}'?",
                    "Sim",
                    "Não");

                if (confirm)
                {
                    // Exclui pelo ID verdadeiro do banco
                    await App.Db.Delete(produto.ID);

                    // Remove da tela direto da ObservableCollection
                    lista.Remove(produto);

                    // Reorganiza apenas a numeração visual
                    int numero = 1;

                    foreach (var item in lista)
                    {
                        item.Numero = numero;
                        numero++;
                    }

                    await DisplayAlertAsync(
                        "Sucesso",
                        "Produto removido com sucesso!",
                        "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        string q = e.NewTextValue; 

        lista.Clear(); //Limpa o que está aparecendo na tela atualmente

        List<Produto> tmp = await App.Db.Search(q); // Busca no banco SQLite apenas o que combina com o texto digitado

        tmp.ForEach (i => lista.Add(i)); // Alimenta a ObservableCollection com os itens filtrados
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    { // se a lista estiver filtrada, ele soma apenas o que está filtrado)

        double soma = lista.Sum(i => i.Total); // Usa o LINQ (.Sum) sobre a ObservableCollection 'lista'

        string msg = $"O total é {soma:C}";

        DisplayAlertAsync("Total dos Produtos", msg, "OK");
    }
}