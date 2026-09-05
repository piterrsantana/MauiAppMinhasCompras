using MauiAppMinhasCompras.Helpers;

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
                // Se a instância do banco de dados ainda não foi criada, cria uma nova
                if (_db == null)
                {
                    // FileSystem.AppDataDirectory garante o caminho correto em qualquer plataforma
                    string path = Path.Combine(
                        FileSystem.AppDataDirectory,
                        "banco_sqlite_compras.db3");

                    _db = new SQLiteDatabaseHelper(path);
                }
                // Se ja existe uma instância, retorna a instância existente
                return _db;
            }
        }
        public App()
        {
            InitializeComponent();
        }
        // Sobrescreve o método CreateWindow para definir a página inicial do aplicativo
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(new Views.ListaProduto()));
        }
    }
}