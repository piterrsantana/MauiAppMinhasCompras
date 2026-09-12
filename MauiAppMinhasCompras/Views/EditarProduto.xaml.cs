using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    public EditarProduto()
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
            Produto? produto_anexado = BindingContext as Produto;

            if (produto_anexado == null)
            {
                await DisplayAlertAsync("Ops", "Nenhum produto foi selecionado.", "OK");
                return;
            }

            Produto p = new Produto
            {
                Id = produto_anexado.Id,
                Descricao = txt_descricao.Text ?? string.Empty,
                Categoria = pck_categoria.SelectedItem?.ToString() ?? string.Empty,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text)
            };

            await App.Db.Update(p);
            await DisplayAlertAsync("Sucesso!", "Produto Atualizado", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }
}