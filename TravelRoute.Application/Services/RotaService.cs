using TravelRoute.Domain.Entities;
using TravelRoute.Infrastructure.Persistence;

public class RotaService
{
    private readonly RotaRepository _repository;

    public RotaService(RotaRepository repository)
    {
        _repository = repository;
    }

    public string ObterMelhorRota(string origem, string destino)
    {
        var rotas = _repository.ObterRotas();
        var caminhos = new List<(List<string> Path, int Cost)>();
        BuscarCaminhos(rotas, origem, destino, new List<string>(), 0, caminhos);

        var melhorCaminho = caminhos.OrderBy(x => x.Cost).First();
        return $"{string.Join(" - ", melhorCaminho.Path)} ao custo de ${melhorCaminho.Cost}";
    }

    private void BuscarCaminhos(List<Rota> rotas, string atual, string destino, List<string> caminho, int custo, List<(List<string>, int)> caminhos)
    {
        caminho.Add(atual);

        if (atual == destino)
        {
            caminhos.Add((new List<string>(caminho), custo));
            return;
        }

        foreach (var rota in rotas.Where(r => r.Origem == atual))
        {
            if (!caminho.Contains(rota.Destino))
            {
                BuscarCaminhos(rotas, rota.Destino, destino, caminho, custo + rota.Custo, caminhos);
            }
        }

        caminho.RemoveAt(caminho.Count - 1);
    }
}
