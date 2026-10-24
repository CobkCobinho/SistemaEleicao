using SistemaEleicao.ConsoleApp.Dominio;

namespace SistemaEleicao.ConsoleApp.Apresentacao;

public sealed class MenuEleicao(EleicaoManager eleicao, TextReader entrada, TextWriter saida)
{
    public void Executar()
    {
        saida.WriteLine("SISTEMA DE ELEIÇÃO | Trabalho individual");
        saida.WriteLine("Dados simulados, mantidos apenas durante esta execução.");
        while (true)
        {
            saida.WriteLine("\n1 - Cadastrar candidato\n2 - Registrar eleitor\n3 - Votar\n4 - Apurar votos\n5 - Listar candidatos\n0 - Sair");
            saida.Write("Escolha: ");
            var opcao = entrada.ReadLine();
            if (opcao is null || opcao.Trim() == "0") break;
            try
            {
                switch (opcao.Trim())
                {
                    case "1": CadastrarCandidato(); break;
                    case "2": RegistrarEleitor(); break;
                    case "3": Votar(); break;
                    case "4": ExibirApuracao(); break;
                    case "5": ListarCandidatos(); break;
                    default: saida.WriteLine("Opção inválida. Escolha uma opção do menu."); break;
                }
            }
            catch (OperationCanceledException) { break; }
            catch (Exception erro) when (erro is ArgumentException or InvalidOperationException or FormatException)
            {
                saida.WriteLine($"Não foi possível concluir: {erro.Message}");
            }
        }
        saida.WriteLine("Sistema encerrado. Os dados desta sessão não serão salvos.");
    }

    private string Ler(string mensagem)
    {
        saida.Write(mensagem);
        return entrada.ReadLine() ?? throw new OperationCanceledException();
    }

    private int LerNumero(string mensagem)
    {
        var texto = Ler(mensagem);
        return int.TryParse(texto, out var numero)
            ? numero
            : throw new FormatException("Número inválido. Digite um número inteiro.");
    }

    private void CadastrarCandidato()
    {
        var nome = Ler("Nome do candidato: ");
        var numero = LerNumero("Número do candidato (maior que zero): ");
        var candidato = eleicao.CadastrarCandidato(nome, numero);
        saida.WriteLine($"Candidato cadastrado: {candidato.Numero} - {candidato.Nome}.");
    }

    private void RegistrarEleitor()
    {
        eleicao.RegistrarEleitor(Ler("Título/CPF simulado (use dados fictícios): "));
        saida.WriteLine("Eleitor registrado com sucesso.");
    }

    private void Votar()
    {
        var documento = Ler("Título/CPF simulado do eleitor: ");
        ListarCandidatos();
        saida.WriteLine("0 = branco | -1 = nulo | número inexistente = nulo");
        var numero = LerNumero("Número escolhido: ");
        eleicao.Votar(documento, numero);
        saida.WriteLine("Voto registrado com sucesso.");
    }

    private void ListarCandidatos()
    {
        if (eleicao.Candidatos.Count == 0)
            saida.WriteLine("Nenhum candidato cadastrado.");
        foreach (var candidato in eleicao.Candidatos)
            saida.WriteLine($"{candidato.Numero} - {candidato.Nome}");
    }

    private void ExibirApuracao()
    {
        var resultado = eleicao.Apurar();
        saida.WriteLine("\n=== APURAÇÃO DE VOTOS ===");
        foreach (var item in resultado.VotosPorCandidato)
            saida.WriteLine($"{item.Candidato.Numero} - {item.Candidato.Nome}: {item.TotalVotos} voto(s)");
        saida.WriteLine($"Votos válidos: {resultado.VotosValidos}");
        saida.WriteLine($"Votos brancos: {resultado.VotosBrancos}");
        saida.WriteLine($"Votos nulos: {resultado.VotosNulos}");
        saida.WriteLine($"Total de votos: {resultado.TotalVotos}");
        saida.WriteLine($"Eleitores cadastrados: {eleicao.Eleitores.Count}");
        saida.WriteLine($"Eleitores que votaram: {eleicao.Eleitores.Count(e => e.JaVotou)}");
        if (resultado.HouveEmpate)
            saida.WriteLine($"Empate entre: {string.Join(", ", resultado.Vencedores.Select(c => c.Nome))}. Não há vencedor único.");
        else if (resultado.Vencedor is { } vencedor)
            saida.WriteLine($"Vencedor: {vencedor.Nome} ({vencedor.Numero}).");
        else
            saida.WriteLine("Sem vencedor: nenhum voto válido registrado.");
    }
}

