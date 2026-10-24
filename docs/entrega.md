# Roteiro de entrega e apresentação

## Identificação

- Projeto: Sistema de Eleição.
- Modalidade: trabalho individual, conforme solicitado pelo responsável.
- Conta responsável: CobkCobinho.
- Disciplina: Garantia da Qualidade de Software / Gestão e Qualidade de Software.
- Professor: Daniel Henrique Matos de Paiva.

Não foram incluídos integrantes fictícios. Se o ambiente de entrega exigir nome completo ou matrícula, esses dados devem ser informados pelo aluno no próprio ambiente.

## Conferência dos requisitos

| Requisito da atividade | Implementação ou evidência |
| --- | --- |
| Repositório Git e solução .NET | `SistemaEleicao.sln` e histórico Git |
| Projeto de console e projeto xUnit | `SistemaEleicao.Console` e `SistemaEleicao.Tests` |
| Referência do projeto de testes ao principal | `ProjectReference` no projeto xUnit |
| Primeiro commit com Conventional Commits | `chore: initial project structure with console and xunit apps` no histórico original preservado no bundle |
| Cadastrar nome e número do candidato | `Candidato` e `EleicaoManager.CadastrarCandidato` |
| Impedir números duplicados | `CandidatoTests.CadastrarCandidato_RejeitaNumeroDuplicado` |
| Registrar eleitores por documento simulado | `Eleitor` e `EleicaoManager.RegistrarEleitor` |
| Impedir voto duplicado | Teoria `Votar_ImpedeSegundoVotoInclusiveAposBrancoOuNulo` |
| Contar candidatos inexistentes como nulos | Teoria `Votar_ClassificaVotoEMarcaEleitor` |
| Votos válidos e brancos | `TipoVoto` e testes de votação/apuração |
| Relatório e candidato vencedor | `ResultadoEleicao` e `MenuEleicao.ExibirApuracao` |
| Menu interativo | `Program.cs` inicia `MenuEleicao.Executar` |
| Refatoração | Namespace `Apresentacao` separado de `Dominio` |
| Documentação | `README.md`, este roteiro e execução registrada |
| Testes correspondentes às etapas | 40 casos xUnit, com commits de teste e implementação |
| Repositório público e branch principal | Destino da entrega: `CobkCobinho/SistemaEleicao`, branch `main` |
| Histórico limpo e padronizado | `git status` e `git log --oneline` |

## Decisões para situações não detalhadas no enunciado

- Candidato com número zero é recusado porque zero identifica voto branco.
- Empate é mostrado explicitamente, sem desempate inventado.
- Uma eleição sem votos válidos não possui vencedor.
- CPF/título é um identificador numérico simulado, sem consulta ou validação externa.
- A contagem dos votos é independente dos documentos: não se armazena associação entre um eleitor e a sua escolha.
- Não se exige persistência no enunciado; os dados existem apenas na sessão atual.

## Roteiro sugerido de apresentação

1. Mostre os dois projetos na solução e a referência entre eles.
2. Explique a separação entre entidades, gerenciador e menu.
3. Execute `dotnet test SistemaEleicao.sln`: confira os 40 casos.
4. Execute o console e siga a demonstração do README.
5. Tente cadastrar dois candidatos com o mesmo número e votar duas vezes com o mesmo eleitor.
6. Mostre o relatório com votos válidos, brancos e nulos.
7. Explique que empate ou ausência de votos válidos não produz vencedor único.
8. Mostre `git log --oneline` e os commits por etapa.

## Entrega acadêmica

O produto solicitado é o link do repositório público com o código, os testes, o README e o histórico. A submissão no ambiente da instituição e o prazo não foram informados; não foi realizada uma submissão acadêmica em nome do aluno.

## Histórico original e publicação

O arquivo `SistemaEleicao-historico.bundle` na raiz permite recuperar os 8 commits originais do desenvolvimento local. A branch pública `main` contém as etapas de envio dos arquivos pelo navegador, também com Conventional Commits. O README explica como abrir o histórico original em outra pasta.
