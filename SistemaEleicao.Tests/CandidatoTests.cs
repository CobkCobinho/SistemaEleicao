using SistemaEleicao.ConsoleApp.Dominio;

namespace SistemaEleicao.Tests;

public class CandidatoTests
{
    [Fact]
    public void CadastrarCandidato_ArmazenaNomeENumero()
    {
        var eleicao = new EleicaoManager();
        var candidato = eleicao.CadastrarCandidato("  Ana  ", 10);
        Assert.Equal("Ana", candidato.Nome);
        Assert.Equal(10, candidato.Numero);
        Assert.Equal(candidato, Assert.Single(eleicao.Candidatos));
    }

    [Fact]
    public void CadastrarCandidato_RejeitaNumeroDuplicado()
    {
        var eleicao = new EleicaoManager();
        eleicao.CadastrarCandidato("Ana", 10);
        Assert.Throws<InvalidOperationException>(() => eleicao.CadastrarCandidato("Bruno", 10));
        Assert.Single(eleicao.Candidatos);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Candidato_RejeitaNumeroNaoPositivo(int numero)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Candidato("Ana", numero));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Candidato_RejeitaNomeVazio(string? nome)
    {
        Assert.Throws<ArgumentException>(() => new Candidato(nome!, 10));
    }
}
