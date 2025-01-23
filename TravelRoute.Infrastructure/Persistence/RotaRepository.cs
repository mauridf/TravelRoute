using Microsoft.Data.Sqlite;
using TravelRoute.Domain.Entities;

namespace TravelRoute.Infrastructure.Persistence
{
    public class RotaRepository
    {
        private readonly string _connectionString;

        public RotaRepository(string connectionString)
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
				CREATE TABLE IF NOT EXISTS Rota (
					Id INTEGER PRIMARY KEY AUTOINCREMENT,
					Origem TEXT NOT NULL,
					Destino TEXT NOT NULL,
					Custo INTEGER NOT NULL,
					FOREIGN KEY (Origem) REFERENCES Ponto(Nome),
					FOREIGN KEY (Destino) REFERENCES Ponto(Nome)
				);
			";
            command.ExecuteNonQuery();
        }

        public List<Rota> ObterRotas()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Origem, Destino, Custo FROM Rota;";
            using var reader = command.ExecuteReader();

            var rotas = new List<Rota>();
            while (reader.Read())
            {
                rotas.Add(new Rota
                {
                    Origem = reader.GetString(0),
                    Destino = reader.GetString(1),
                    Custo = reader.GetInt32(2)
                });
            }
            return rotas;
        }

        public void SaveRota(Rota rota)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Rota (Origem, Destino, Custo)
                VALUES (@origem, @destino, @custo);
            ";
            command.Parameters.AddWithValue("@origem", rota.Origem);
            command.Parameters.AddWithValue("@destino", rota.Destino);
            command.Parameters.AddWithValue("@custo", rota.Custo);
            command.ExecuteNonQuery();
        }
    }
}