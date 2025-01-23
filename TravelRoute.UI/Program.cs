using System;
using System.Linq;
using TravelRoute.Application.Services;
using TravelRoute.Infrastructure.Persistence;
using SQLitePCL;

namespace TravelRoute.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Inicialização do SQLite
            Batteries_V2.Init();

            // Definição da string de conexão
            const string connectionString = "Data Source=rotas.db";

            // Inicialização dos repositórios
            var pontoRepository = new PontoRepository(connectionString);
            var rotaRepository = new RotaRepository(connectionString);

            // Obtenção de rotas do banco de dados
            var rotas = rotaRepository.ObterRotas();

            // Conversão das rotas para o formato esperado pelo serviço
            var rotasConvertidas = rotas.Select(r => (r.Origem, r.Destino, r.Custo)).ToList();
            var graphTraversalService = new GraphTraversalService(rotasConvertidas);

            // Interface do usuário
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Bem-vindo ao sistema de rotas!");
                Console.WriteLine("Escolha uma opção:");
                Console.WriteLine("1. Registrar nova rota");
                Console.WriteLine("2. Consultar melhor rota");
                Console.WriteLine("3. Sair");

                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        RegistrarNovaRota(pontoRepository, rotaRepository);
                        break;

                    case "2":
                        ConsultarMelhorRota(graphTraversalService);
                        break;

                    case "3":
                        Console.WriteLine("Obrigado por usar o sistema de rotas!");
                        return;

                    default:
                        Console.WriteLine("Opção inválida. Pressione qualquer tecla para tentar novamente.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void RegistrarNovaRota(PontoRepository pontoRepository, RotaRepository rotaRepository)
        {
            Console.WriteLine("Registro de nova rota:");

            Console.Write("Digite o ponto de origem: ");
            var origem = Console.ReadLine()?.Trim().ToUpper();

            Console.Write("Digite o ponto de destino: ");
            var destino = Console.ReadLine()?.Trim().ToUpper();

            Console.Write("Digite o custo da rota: ");
            if (!decimal.TryParse(Console.ReadLine(), out var custo))
            {
                Console.WriteLine("Custo inválido. A operação foi cancelada.");
                Console.ReadKey();
                return;
            }

            if (string.IsNullOrWhiteSpace(origem) || string.IsNullOrWhiteSpace(destino))
            {
                Console.WriteLine("Origem ou destino inválido. A operação foi cancelada.");
                Console.ReadKey();
                return;
            }

            try
            {
                // Salvar pontos no repositório, caso ainda não existam
                pontoRepository.Salvar(new TravelRoute.Domain.Entities.Ponto(origem));
                pontoRepository.Salvar(new TravelRoute.Domain.Entities.Ponto(destino));

                // Salvar rota no repositório
                rotaRepository.SaveRota(new TravelRoute.Domain.Entities.Rota(origem, destino, (int)custo));

                Console.WriteLine("Rota registrada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao registrar a rota: {ex.Message}");
            }

            Console.WriteLine("Pressione qualquer tecla para continuar.");
            Console.ReadKey();
        }

        private static void ConsultarMelhorRota(GraphTraversalService graphTraversalService)
        {
            Console.WriteLine("Consulta de melhor rota:");

            Console.Write("Digite a rota no formato 'ORIGEM-DESTINO' (ex: GRU-CDG): ");
            var input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                var pontos = input.Split('-');
                if (pontos.Length == 2)
                {
                    var origem = pontos[0].Trim().ToUpper();
                    var destino = pontos[1].Trim().ToUpper();

                    try
                    {
                        var (rota, custo) = graphTraversalService.ObterMelhorRota(origem, destino);

                        if (rota.Any())
                        {
                            Console.WriteLine($"Melhor Rota: {string.Join(" -> ", rota)} ao custo de ${custo}");
                        }
                        else
                        {
                            Console.WriteLine("Nenhuma rota encontrada para os pontos fornecidos.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao buscar a rota: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Formato inválido. Por favor, use o formato 'ORIGEM-DESTINO'.");
                }
            }
            else
            {
                Console.WriteLine("Entrada inválida. Por favor, tente novamente.");
            }

            Console.WriteLine("Pressione qualquer tecla para continuar.");
            Console.ReadKey();
        }
    }
}