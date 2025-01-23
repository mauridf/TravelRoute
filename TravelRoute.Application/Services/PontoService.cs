using TravelRoute.Domain.Entities;
using TravelRoute.Infrastructure.Persistence;

namespace TravelRoute.Application.Services
{
    public class PontoService
    {
        private readonly PontoRepository _repository;

        public PontoService(PontoRepository repository)
        {
            _repository = repository;
        }

        public void RegistrarPonto(string nome)
        {
            if (_repository.ObterPorNome(nome) == null)
            {
                _repository.Salvar(new Ponto(nome));
            }
        }

        public void AdicionarConexao(string origem, string destino, int custo)
        {
            var pontoOrigem = _repository.ObterPorNome(origem) ?? new Ponto(origem);
            var pontoDestino = _repository.ObterPorNome(destino) ?? new Ponto(destino);

            var rota = new Rota { Origem = origem, Destino = destino, Custo = custo };
            pontoOrigem.AdicionarConexao(rota);

            _repository.Salvar(pontoOrigem);
            _repository.Salvar(pontoDestino);
        }

        public List<Ponto> ListarPontos()
        {
            return _repository.ObterTodos();
        }
    }

}
