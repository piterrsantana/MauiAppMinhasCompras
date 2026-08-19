// Importa o namespace que contém a nossa classe ajudante de banco de dados SQLite
using MauiAppMinhasCompras.Helpers;

// Define o namespace principal do projeto .NET MAUI
namespace MauiAppMinhasCompras
{
    // Declara a classe parcial "App", que herda de "Application" (o núcleo que inicia o aplicativo inteiro)
    public partial class App : Application
    {
        // Declara um campo estático privado que guardará a única instância da conexão com o banco de dados (Padrão Singleton)
        static SQLiteDatabaseHelper _db;

        // Cria uma propriedade pública estática chamada "Db" para ser o ponto único de acesso ao banco no app
        public static SQLiteDatabaseHelper Db
        {
            get // Executado sempre que o código chama "App.Db"
            {
                // Verifica se a conexão com o banco ainda não foi criada na memória
                if (_db == null)
                {
                    // Combina o caminho da pasta segura de dados locais do celular...
                    string path = Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData),

                        // ...com o nome específico do arquivo do banco de dados SQLite[cite: 2]
                        "banco_sqlite_compras.db3");

                    // Cria a instância do ajudante de banco de dados passando o caminho do arquivo encontrado[cite: 2]
                    _db = new SQLiteDatabaseHelper(path);
                }

                // Retorna a conexão existente (ou a recém-criada) para ser usada nas telas
                return _db;
            }
        }

        // O Construtor da classe App: é a primeira coisa executada quando o aplicativo abre no celular
        public App()
        {
            // Inicializa todos os componentes visuais definidos nos arquivos XAML
            InitializeComponent();

            // (Comentado) A configuração antiga usando o AppShell padrão do projeto
            //MainPage = new AppShell();

            // Define a página inicial do aplicativo como uma página de navegação empacotando a tela de lista de produtos
            MainPage = new NavigationPage(new Views.ListaProduto());
        }
    }
}