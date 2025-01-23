using Microsoft.Data.Sqlite;
using TravelRoute.Domain.Entities;

namespace TravelRoute.Infrastructure.Persistence
{
    public class PontoRepository
    {
        private readonly string _connectionString;

        public PontoRepository(string connectionString)
        {
            _connectionString = connectionString;
            CriarTabelaSeNaoExistir();
        }

        private void CriarTabelaSeNaoExistir()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
				CREATE TABLE IF NOT EXISTS Ponto (
					Id INTEGER PRIMARY KEY AUTOINCREMENT,
					Nome TEXT NOT NULL UNIQUE
				);
			";
            command.ExecuteNonQuery();
        }

        public Ponto ObterPorNome(string nome)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Nome FROM Ponto WHERE Nome = @nome;";
            command.Parameters.AddWithValue("@nome", nome);

            using var reader = command.ExecuteReader();
            return reader.Read() ? new Ponto(reader.GetString(0)) : null;
        }

        public void Salvar(Ponto ponto)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT OR IGNORE INTO Ponto (Nome)
                VALUES (@nome);
            ";
            command.Parameters.AddWithValue("@nome", ponto.Nome);
            command.ExecuteNonQuery();
        }

        public List<Ponto> ObterTodos()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Nome FROM Ponto;";
            using var reader = command.ExecuteReader();

            var pontos = new List<Ponto>();
            while (reader.Read())
            {
                pontos.Add(new Ponto(reader.GetString(0)));
            }
            return pontos;
        }
    }
}