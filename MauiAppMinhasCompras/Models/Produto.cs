using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        // Variáveis privadas para validação
        string _descricao = string.Empty;
        double _quantidade;
        double _preco;
        string _categoria = string.Empty;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Descricao
        {
            get => _descricao;
            set
            {
                if (value != null)
                {
                    _descricao = value;
                }
                else
                {
                    throw new Exception("Por favor, preencha a descrição");
                }
            }
        }

        public double Quantidade
        {
            get => _quantidade;
            set
            {
                if (value <= 0)
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

        // Propriedade Categoria adicionada para o Desafio 1
        public string Categoria
        {
            get => _categoria;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Por favor, selecione uma categoria para o produto");
                }

                _categoria = value;
            }
        }

        public double Total { get => Quantidade * Preco; }
    }
}