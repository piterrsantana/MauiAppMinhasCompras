using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        //Variaveis privadas para validação
        string _descricao = string.Empty;
        double _quantidade;
        double _preco;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao
        {
            get => _descricao;
            set
            {
                //Validação para não permitir que a descrição seja vazia
                if (string.IsNullOrWhiteSpace(value))
                {
                    //Lançando uma exceção caso a descrição seja vazia
                    throw new Exception("Por favor, preencha a descrição");
                }
                //Atribuindo o valor à variável privada
                _descricao = value;
            }
        }

        public double Quantidade
        {
            get => _quantidade;
            set
            {
                if (value < 0)
                {
                    throw new Exception("Por favor, preencha uma quantidade válida");
                }

                _quantidade = value;
            }
        }

        public double Preco
        {
            get => _preco;
            set
            {
                if (value < 0)
                {
                    throw new Exception("Por favor, preencha um preço válido");
                }

                _preco = value;
            }
        }
        public double Total { get => Quantidade * Preco; }
    }
}