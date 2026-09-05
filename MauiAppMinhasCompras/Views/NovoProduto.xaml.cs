using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();
    }

    // Ação do botão de salvar na Toolbar
    private async void ToolbarItem_Clicked(object? sender, EventArgs e)
    {
        // Validação dos campos de entrada. Se houver algum campo vazio, exibe um alerta e retorna sem prosseguir.
        if (string.IsNullOrWhiteSpace(txt_descricao.Text))
        {
            await DisplayAlertAsync("Ops", "Informe a descrição do produto", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(txt_quantidade.Text))
        {
            await DisplayAlertAsync("Ops", "Informe a quantidade do produto", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(txt_preco.Text))
        {
            await DisplayAlertAsync("Ops", "Informe o preço do produto", "OK");
            return;
        }

        try
        {
            // Instanciação do modelo com os dados inseridos.
            // Capturando a descrição, quantidade e preço do produto a partir dos campos de entrada.
            Produto p = new Produto
            {
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text)
            };

            // Persistência no banco SQLite
            await App.Db.Insert(p);

            // Feedback ao usuário e navegação de retorno
            await DisplayAlertAsync("Sucesso!", "Produto Adicionado", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }
}