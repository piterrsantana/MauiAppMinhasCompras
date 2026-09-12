using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txt_descricao.Text))
        {
            await DisplayAlertAsync("Ops", "Informe a descrição do produto", "OK");
            return;
        }

        if (pck_categoria.SelectedItem == null)
        {
            await DisplayAlertAsync("Ops", "Selecione a categoria do produto", "OK");
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
            Produto p = new Produto
            {
                Descricao = txt_descricao.Text,
                Categoria = pck_categoria.SelectedItem?.ToString() ?? string.Empty,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text)
            };

            await App.Db.Insert(p);

            await DisplayAlertAsync("Sucesso!", "Produto Adicionado", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }
}