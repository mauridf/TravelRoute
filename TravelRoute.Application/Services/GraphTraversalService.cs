using System;
using System.Collections.Generic;
using System.Linq;

namespace TravelRoute.Application.Services
{
    public class GraphTraversalService
    {
        private readonly List<(string Origem, string Destino, int Custo)> _rotas;

        public GraphTraversalService(List<(string Origem, string Destino, int Custo)> rotas)
        {
            _rotas = rotas;
        }

        public (List<string> MelhorRota, int Custo) ObterMelhorRota(string origem, string destino)
        {
            var caminhos = new List<(List<string> Caminho, int Custo)>();
            BuscarCaminhos(origem, destino, new List<string>(), 0, caminhos);

            if (!caminhos.Any())
                return (new List<string>(), int.MaxValue);

            // Retorna o caminho com o menor custo
            var melhorCaminho = caminhos.OrderBy(c => c.Custo).First();
            return (melhorCaminho.Caminho, melhorCaminho.Custo);
        }

        private void BuscarCaminhos(string atual, string destino, List<string> caminhoAtual, int custoAtual, List<(List<string>, int)> caminhos)
        {
            caminhoAtual.Add(atual);

            // Caso base: chegou no destino
            if (atual == destino)
            {
                caminhos.Add((new List<string>(caminhoAtual), custoAtual));
                return;
            }

            // Continua a busca recursiva para os destinos adjacentes
            foreach (var rota in _rotas.Where(r => r.Origem == atual))
            {
                if (!caminhoAtual.Contains(rota.Destino)) // Evitar ciclos
                {
                    BuscarCaminhos(rota.Destino, destino, caminhoAtual, custoAtual + rota.Custo, caminhos);
                }
            }

            // Remove o ponto atual para explorar outros caminhos
            caminhoAtual.RemoveAt(caminhoAtual.Count - 1);
        }
    }
}