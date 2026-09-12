using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        // Adicionar Novo Produto
        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }

        // Atualizar (Incluído o campo Categoria no UPDATE)
        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=?, Categoria=? WHERE Id=?";

            return _conn.QueryAsync<Produto>(
                sql, p.Descricao, p.Quantidade, p.Preco, p.Categoria, p.Id
            );
        }

        // Apagar um produto
        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        // Retornar todos os produtos
        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        // NOVO: Buscar produtos filtrados por Categoria
        public Task<List<Produto>> GetByCategoria(string categoria)
        {
            return _conn.Table<Produto>()
                        .Where(p => p.Categoria == categoria)
                        .ToListAsync();
        }

        // Pesquisar por Descrição
        public Task<List<Produto>> Search(string q)
        {
            string sql = "SELECT * FROM Produto WHERE descricao LIKE '%" + q + "%'";

            return _conn.QueryAsync<Produto>(sql);
        }

        public async Task ZerarEAnularTabela()
        {
            // Apaga a tabela e recria zerada
            await _conn.DropTableAsync<Produto>();
            await _conn.CreateTableAsync<Produto>();
        }
    }
}