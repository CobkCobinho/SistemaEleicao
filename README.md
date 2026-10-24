# Sistema de Eleição

Aplicação de console em **C# / .NET 10**, desenvolvida como **trabalho individual** para a atividade de Garantia da Qualidade de Software / Gestão e Qualidade de Software, professor Daniel Henrique Matos de Paiva.

Responsável pelo repositório: [CobkCobinho](https://github.com/CobkCobinho).

## Executar

Instale o **SDK do .NET 10** e o Git. Na pasta onde deseja manter o projeto:

```sh
git clone https://github.com/CobkCobinho/SistemaEleicao.git
cd SistemaEleicao
dotnet restore SistemaEleicao.sln
dotnet run --project SistemaEleicao.Console/SistemaEleicao.Console.csproj
```

O `global.json` aceita SDKs 10.0 com atualizações de feature band. O SDK, e não apenas o runtime, é necessário para compilar e testar. A primeira restauração baixa as dependências xUnit do NuGet e exige internet.

## Menu

| Opção | Operação |
| --- | --- |
| 1 | Cadastrar candidato por nome e número |
| 2 | Registrar eleitor por título/CPF simulado |
| 3 | Registrar voto de eleitor cadastrado |
| 4 | Exibir apuração acumulada |
| 5 | Listar candidatos por número |
| 0 | Encerrar |

### Regras de negócio

- O nome do candidato é obrigatório; espaços nas extremidades são removidos.
- O número do candidato deve ser positivo e único. Zero é reservado ao voto branco.
- O documento do eleitor é **simulado**: deve conter dígitos. Espaços, pontos, barras e hífens são ignorados. Não há validação de tamanho ou dígitos verificadores de um CPF/título real. Use apenas dados fictícios.
- O mesmo documento não pode ser cadastrado duas vezes, mesmo com outra pontuação.
- O eleitor precisa estar cadastrado para votar e pode votar **uma única vez**, inclusive se escolher branco ou nulo.
- Voto em candidato cadastrado: válido. Número `0`: branco. Número `-1` ou qualquer outro número inexistente: nulo.
- Entrada não numérica é rejeitada sem consumir o voto do eleitor.
- A apuração mostra todos os candidatos, inclusive os que têm zero votos, além de válidos, brancos, nulos e total geral.
- Vence o candidato com mais votos válidos. Em empate, todos os empatados são exibidos e não se escolhe vencedor arbitrariamente. Sem votos válidos, não há vencedor.
- A apuração é uma consulta e pode ser repetida; não encerra a votação.
- **Os dados ficam em memória** e são descartados ao fechar o programa. Não há banco de dados, persistência ou autenticação. A aplicação é uma simulação acadêmica, de uso local e sequencial.

## Compilar e testar

```sh
dotnet build SistemaEleicao.sln --configuration Release
dotnet test SistemaEleicao.sln --configuration Release
```

São **40 casos de teste xUnit** nas seguintes classes:

| Classe | Casos | Verificações |
| --- | ---: | --- |
| `CandidatoTests` | 7 | Cadastro, número duplicado, nome vazio e número não positivo |
| `VotacaoTests` | 16 | Documento simulado, duplicidade, eleitor inexistente, tipos de voto e segundo voto |
| `ApuracaoTests` | 6 | Totais, vencedor, empate, ausência de votos válidos e independência dos resultados |
| `MenuEleicaoTests` | 11 | Fluxo completo, entradas inválidas, voto repetido, empate e fim da entrada |

Os casos de `[Theory]` são contabilizados individualmente. Os testes usam entradas e saídas em memória e não dependem de interação manual ou serviços externos.

Cobertura opcional, usando o coletor já referenciado no projeto de testes:

```sh
dotnet test SistemaEleicao.sln --collect:"XPlat Code Coverage"
```

Os relatórios são gerados em `SistemaEleicao.Tests/TestResults/` e não entram no Git.

## Estrutura

```text
SistemaEleicao.sln
SistemaEleicao.Console/
  Program.cs                         # Ponto de entrada
  Apresentacao/MenuEleicao.cs         # Menu e tratamento das entradas
  Dominio/
    Candidato.cs                     # Nome e número do candidato
    Eleitor.cs                       # Documento simulado e controle de voto
    EleicaoManager.cs                # Cadastro, votação e apuração
    TipoVoto.cs                      # Válido, branco e nulo
    ResultadoEleicao.cs              # Totais, vencedor e empate
SistemaEleicao.Tests/                # Testes xUnit
docs/
  entrega.md                         # Correspondência com os requisitos e roteiro
  exemplo-execucao.txt                # Saída de uma execução real
```

A lógica de negócio independe da interface. O menu recebe `TextReader` e `TextWriter`, permitindo testar o mesmo fluxo usado no console.

## Demonstração rápida

1. Cadastre `Ana`, número `10`, e `Bruno`, número `20`.
2. Cadastre três eleitores fictícios: `101`, `102` e `103`.
3. Vote com `101` em `10`, com `102` em `0` e com `103` em `999`.
4. Tente votar novamente com `101`: o sistema deve recusar.
5. Apure: Ana = 1, Bruno = 0, branco = 1, nulo = 1, total = 3, vencedora = Ana.

O arquivo [docs/exemplo-execucao.txt](docs/exemplo-execucao.txt) registra esse cenário executado no programa.

## Histórico de desenvolvimento

Os commits seguem **Conventional Commits**, com tipos `chore`, `test`, `feat`, `refactor` e `docs`. Os testes das regras foram registrados antes das implementações correspondentes, seguidos do menu, da separação entre apresentação e domínio e da documentação.

```sh
git log --oneline
git status
```

Consulte [docs/entrega.md](docs/entrega.md) para o roteiro de verificação dos requisitos.

