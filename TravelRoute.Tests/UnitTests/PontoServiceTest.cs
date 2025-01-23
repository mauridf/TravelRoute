using Microsoft.VisualStudio.TestTools.UnitTesting;
using TravelRoute.Application.Services;
using TravelRoute.Domain.Entities;
using TravelRoute.Infrastructure.Persistence;

namespace TravelRoute.Tests.UnitTests
{
    [TestClass]
    public class PontoServiceTests
    {
        private PontoRepository _repository;
        private PontoService _service;

        [TestInitialize]
        public void Setup()
        {
            var connectionString = "Data Source=rotas_test.db";
            _repository = new PontoRepository(connectionString);
            _service = new PontoService(_repository);
        }

        [TestMethod]
        public void RegistrarPonto_DeveAdicionarPontoSeNaoExistir()
        {
            _service.RegistrarPonto("GRU");

            var pontos = _repository.ObterTodos();
            Assert.AreEqual(1, pontos.Count);
            Assert.AreEqual("GRU", pontos[0].Nome);
        }

        [TestMethod]
        public void RegistrarPonto_NaoDeveDuplicarPontosExistentes()
        {
            _service.RegistrarPonto("GRU");
            _service.RegistrarPonto("GRU");

            var pontos = _repository.ObterTodos();
            Assert.AreEqual(1, pontos.Count);
        }

        [TestMethod]
        public void AdicionarConexao_DeveCriarConexoesEntrePontos()
        {
            _service.AdicionarConexao("GRU", "BRC", 10);

            var ponto = _repository.ObterPorNome("GRU");
            Assert.IsNotNull(ponto);
            Assert.AreEqual(1, ponto.Conexoes.Count);
            Assert.AreEqual("BRC", ponto.Conexoes[0].Destino);
            Assert.AreEqual(10, ponto.Conexoes[0].Custo);
        }

        [TestMethod]
        public void AdicionarConexao_DeveRegistrarPontoSeNaoExistir()
        {
            _service.AdicionarConexao("GRU", "BRC", 10);

            var pontoDestino = _repository.ObterPorNome("BRC");
            Assert.IsNotNull(pontoDestino);
            Assert.AreEqual("BRC", pontoDestino.Nome);
        }

        [TestMethod]
        public void ListarPontos_DeveRetornarTodosPontos()
        {
            _service.RegistrarPonto("GRU");
            _service.RegistrarPonto("BRC");

            var pontos = _service.ListarPontos();
            Assert.AreEqual(2, pontos.Count);
        }
    }

}
