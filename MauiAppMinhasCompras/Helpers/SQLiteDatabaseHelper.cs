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

        // Atualizar Produto (Com a coluna Categoria)
        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=?, Categoria=? WHERE Id=?";

            return _conn.QueryAsync<Produto>(
                sql, p.Descricao, p.Quantidade, p.Preco, p.Categoria, p.Id
            );
        }

        // Apagar um produto pelo ID
        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        // Retornar todos os produtos salvos
        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        // Buscar produtos filtrados por Categoria
        public Task<List<Produto>> GetByCategoria(string categoria)
        {
            return _conn.Table<Produto>()
                        .Where(p => p.Categoria == categoria)
                        .ToListAsync();
        }

        // Pesquisar por Descrição (Ignorando diferença entre maiúsculas e minúsculas)
        public Task<List<Produto>> Search(string q)
        {
            string termo = q?.ToLower() ?? string.Empty;
            string sql = "SELECT * FROM Produto WHERE LOWER(Descricao) LIKE '%" + termo + "%'";

            return _conn.QueryAsync<Produto>(sql);
        }

        // Zera a tabela quando ativado (Manter comentado para uso em testes)
        /*
        public async Task ZerarEAnularTabela()
        {
            await _conn.DropTableAsync<Produto>();
            await _conn.CreateTableAsync<Produto>();
        }
        */
    }
}