using Microsoft.VisualStudio.TestTools.UnitTesting;
using TravelRoute.Domain.Entities;
using TravelRoute.Infrastructure.Persistence;

namespace TravelRoute.Tests.UnitTests
{
    [TestClass]
    public class RotaServiceTests
    {
        private RotaRepository _repository;
        private RotaService _service;

        [TestInitialize]
        public void Setup()
        {
            var connectionString = "Data Source=:memory:;Mode=Memory;Cache=Shared";
            _repository = new RotaRepository(connectionString);

            // Inicializa o banco em memória
            var pontoRepository = new PontoRepository(connectionString);
            pontoRepository.Salvar(new Ponto("GRU"));
            pontoRepository.Salvar(new Ponto("BRC"));
            pontoRepository.Salvar(new Ponto("SCL"));

            _service = new RotaService(_repository);
        }

        [TestMethod]
        public void ObterMelhorRota_DeveRetornarRotaMaisBarata()
        {
            _repository.SaveRota(new Rota { Origem = "GRU", Destino = "BRC", Custo = 10 });
            _repository.SaveRota(new Rota { Origem = "BRC", Destino = "SCL", Custo = 5 });

            var resultado = _service.ObterMelhorRota("GRU", "SCL");

            Assert.AreEqual("GRU - BRC - SCL ao custo de $15", resultado);
        }
    }

}
