using MauiAppMinhasCompras.Helpers;
using System.Globalization;

namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        static SQLiteDatabaseHelper? _db;

        // Propriedade pública para acessar a instância do banco de dados
        public static SQLiteDatabaseHelper Db
        {
            get
            {
                if (_db == null)
                {
                    string path = Path.Combine(
                        FileSystem.AppDataDirectory,
                        "banco_sqlite_compras.db3");

                    _db = new SQLiteDatabaseHelper(path);
                }
                return _db;
            }
        }

        public App()
        {
            InitializeComponent();

            // Configura a cultura pt-BR para a aplicação
            var cultura = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentCulture = cultura;
            Thread.CurrentThread.CurrentUICulture = cultura;
        }

        // Sobrescreve o método CreateWindow para definir a página inicial do aplicativo
        protected override Window CreateWindow(IActivationState? activationState)
        {
            // --- CÓDIGO TEMPORÁRIO PARA ZERAR O BANCO ---
            // Task.Run(async () => await Db.ZerarEAnularTabela());
            // ------------------------------------------

            return new Window(new NavigationPage(new Views.ListaProduto()));
        }
    }
}