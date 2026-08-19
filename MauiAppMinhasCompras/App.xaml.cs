using Microsoft.Extensions.DependencyInjection;

namespace MauiAppMinhasCompras
{
    // Aqui estamos abrindo a "fábrica principal" do nosso aplicativo móvel.
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage (new Views.ListaProduto());
        }

     
    }
}