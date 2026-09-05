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
        if(string.IsNullOrWhiteSpace(txt_descricao.Text))
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
            // Tenta obter o produto anexado ao BindingContext da página
            Produto? produto_anexado = BindingContext as Produto;

            // Se o BindingContext não for um Produto, interrompe a execução com segurança
            if (produto_anexado == null)
            {
                await DisplayAlertAsync("Ops", "Nenhum produto foi selecionado.", "OK");
                return;
            }

            // Cria o objeto atualizado com os dados informados nos campos de texto
            Produto p = new Produto
            {
                Id = produto_anexado.Id,
                Descricao = txt_descricao.Text ?? "", //O ?? garante que a descrição não seja nula
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text)
            };

            // Atualiza o produto no banco de dados SQLite
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