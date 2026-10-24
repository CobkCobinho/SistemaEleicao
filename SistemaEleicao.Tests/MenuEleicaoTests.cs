using SistemaEleicao.ConsoleApp.Apresentacao;
using SistemaEleicao.ConsoleApp.Dominio;

namespace SistemaEleicao.Tests;

public class MenuEleicaoTests
{
    private static string Executar(string entradas, EleicaoManager? eleicao = null)
    {
        using var saida = new StringWriter();
        new MenuEleicao(eleicao ?? new EleicaoManager(), new StringReader(entradas), saida).Executar();
        return saida.ToString();
    }

    [Fact]
    public void Menu_PermiteCadastrarVotarEApurar()
    {
        var saida = Executar("1\nAna\n10\n2\n123\n3\n123\n10\n4\n0\n");
        Assert.Contains("Candidato cadastrado: 10 - Ana", saida);
        Assert.Contains("Eleitor registrado com sucesso", saida);
        Assert.Contains("Voto registrado com sucesso", saida);
        Assert.Contains("Total de votos: 1", saida);
        Assert.Contains("Vencedor: Ana (10)", saida);
    }

    [Fact]
    public void Menu_InformaOpcaoInvalidaEContinua()
    {
        var saida = Executar("99\n0\n");
        Assert.Contains("Opção inválida", saida);
        Assert.Contains("Sistema encerrado", saida);
    }

    [Fact]
    public void Menu_NumeroInvalidoNaoConsomeVoto()
    {
        var eleicao = new EleicaoManager();
        eleicao.RegistrarEleitor("1");
        var saida = Executar("3\n1\nabc\n0\n", eleicao);
        Assert.Contains("Número inválido", saida);
        Assert.Equal(0, eleicao.Apurar().TotalVotos);
        Assert.False(Assert.Single(eleicao.Eleitores).JaVotou);
    }

    [Fact]
    public void Menu_MostraErroDeVotoDuplicadoSemEncerrar()
    {
        var saida = Executar("2\n1\n3\n1\n0\n3\n1\n0\n4\n0\n");
        Assert.Contains("Este eleitor já votou", saida);
        Assert.Contains("Total de votos: 1", saida);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1\n")]
    [InlineData("1\nAna\n")]
    [InlineData("2\n")]
    [InlineData("3\n1\n")]
    public void Menu_FimDaEntradaEncerraSemErro(string entrada)
    {
        Assert.Contains("Sistema encerrado", Executar(entrada));
    }

    [Fact]
    public void Menu_ExibeEmpate()
    {
        var saida = Executar("1\nAna\n10\n1\nBruno\n20\n2\n1\n2\n2\n3\n1\n10\n3\n2\n20\n4\n0\n");
        Assert.Contains("Empate entre: Ana, Bruno", saida);
        Assert.Contains("Não há vencedor único", saida);
    }

    [Fact]
    public void Menu_SemVotosInformaAusenciaDeVencedor()
    {
        Assert.Contains("Sem vencedor", Executar("4\n0\n"));
    }
}

