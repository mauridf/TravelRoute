namespace TravelRoute.Domain.Entities
{
    public class Ponto
    {
        public string Nome { get; set; }
        public List<Rota> Conexoes { get; set; } = new();

        public Ponto(string nome)
        {
            Nome = nome;
        }

        public void AdicionarConexao(Rota rota)
        {
            Conexoes.Add(rota);
        }
    }
}
