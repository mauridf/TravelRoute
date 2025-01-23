namespace TravelRoute.Domain.Entities
{
    public class Rota
    {
        public string Origem { get; set; }
        public string Destino { get; set; }
        public int Custo { get; set; }

        public Rota(string origem, string destino, int custo)
        {
            Origem = origem;
            Destino = destino;
            Custo = custo;
        }

        // Construtor padrão, se necessário
        public Rota() { }
    }
}