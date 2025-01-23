# Travel Route System

## Descrição
Este sistema permite registrar rotas de viagem e consultar a melhor rota com base no menor custo. O programa foi desenvolvido em C# utilizando .NET, com armazenamento em SQLite para persistência de dados.

## Funcionalidades
1. **Registrar nova rota:** Permite adicionar rotas informando os pontos de origem, destino e o custo.
2. **Consultar melhor rota:** Calcula e exibe a rota com o menor custo entre dois pontos, considerando conexões.

## Requisitos
- .NET SDK 8.0
- SQLite para armazenamento dos dados

## Configuração do Banco de Dados
O banco de dados é armazenado em um arquivo SQLite chamado `rotas.db`. 
Certifique-se de que o arquivo esteja na mesma pasta do executável.
O Sistema gera a base e suas tabelas automaticamente.

### Estrutura do Banco de Dados
- **Pontos:** Tabela para armazenar os pontos de origem e destino.
- **Rotas:** Tabela para armazenar as rotas com suas origens, destinos e custos.

## Execução
1. Clone o repositório.
2. Certifique-se de que você possui o .NET SDK instalado.
3. Compile o projeto:
   ```bash
   dotnet build
   ```
4. Execute o programa:
   ```bash
   dotnet run --project TravelRoute.UI
   ```

## Como Usar

### Registrar Nova Rota
1. Escolha a opção **1. Registrar nova rota** no menu principal.
2. Insira os dados solicitados:
   - Ponto de origem (ex: GRU)
   - Ponto de destino (ex: CDG)
   - Custo da rota (ex: 40)
3. A rota será salva no banco de dados.

### Consultar Melhor Rota
1. Escolha a opção **2. Consultar melhor rota** no menu principal.
2. Insira os pontos de origem e destino no formato `ORIGEM-DESTINO` (ex: GRU-CDG).
3. O sistema calculará a rota com o menor custo e exibirá o resultado no formato:
   ```
   Melhor Rota: GRU -> BRC -> SCL -> ORL -> CDG ao custo de $40
   ```

## Exemplo de Uso
Dado o seguinte cenário de rotas:
1. GRU - BRC - SCL - ORL - CDG ao custo de $40
2. GRU - ORL - CDG ao custo de $61
3. GRU - CDG ao custo de $75
4. GRU - SCL - ORL - CDG ao custo de $45

Ao consultar a melhor rota de **GRU** para **CDG**, o sistema exibiria:
```
Melhor Rota: GRU -> BRC -> SCL -> ORL -> CDG ao custo de $40
```

## Estrutura do Projeto

### Diretórios Principais
- **TravelRoute.Application:** Contém as lógicas de negócio, incluindo o serviço de busca de rotas.
- **TravelRoute.Infrastructure:** Contém os repositórios para interação com o banco de dados.
- **TravelRoute.UI:** Interface de linha de comando para interação com o usuário.

### Classes Principais
- `GraphTraversalService`: Responsável por calcular a melhor rota considerando o menor custo.
- `PontoRepository`: Gerencia o armazenamento e recuperação de pontos no banco de dados.
- `RotaRepository`: Gerencia o armazenamento e recuperação de rotas no banco de dados.
- `Program.cs`: Contém o fluxo principal do programa.

## Melhorias Futuras (Ideias para outras versões)
- Implementar uma interface gráfica. (A principio o sistema foi feito em Aplication Console)
- Suporte para diferentes moedas.
- Adição de restrições, como limite de conexões.
- Integração com serviços de mapas para visualização das rotas.