using SistemaEleicao.ConsoleApp.Dominio;

namespace SistemaEleicao.Tests;

public class VotacaoTests
{
    private static EleicaoManager CriarEleicao()
    {
        var eleicao = new EleicaoManager();
        eleicao.CadastrarCandidato("Ana", 10);
        eleicao.CadastrarCandidato("Bruno", 20);
        return eleicao;
    }

    [Fact]
    public void RegistrarEleitor_NormalizaDocumentoSimulado()
    {
        var eleicao = CriarEleicao();
        var eleitor = eleicao.RegistrarEleitor(" 123.456.789-00 ");
        Assert.Equal("12345678900", eleitor.Documento);
        Assert.False(eleitor.JaVotou);
        Assert.Single(eleicao.Eleitores);
    }

    [Fact]
    public void RegistrarEleitor_RejeitaDuplicadoMesmoComOutraFormatacao()
    {
        var eleicao = CriarEleicao();
        eleicao.RegistrarEleitor("123.456.789-00");
        Assert.Throws<InvalidOperationException>(() => eleicao.RegistrarEleitor("12345678900"));
        Assert.Single(eleicao.Eleitores);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(".-/")]
    [InlineData("123abc")]
    public void RegistrarEleitor_RejeitaDocumentoInvalido(string? documento)
    {
        var eleicao = CriarEleicao();
        Assert.Throws<ArgumentException>(() => eleicao.RegistrarEleitor(documento!));
        Assert.Empty(eleicao.Eleitores);
    }

    [Theory]
    [InlineData(10, TipoVoto.Valido)]
    [InlineData(0, TipoVoto.Branco)]
    [InlineData(-1, TipoVoto.Nulo)]
    [InlineData(999, TipoVoto.Nulo)]
    public void Votar_ClassificaVotoEMarcaEleitor(int numero, TipoVoto esperado)
    {
        var eleicao = CriarEleicao();
        var eleitor = eleicao.RegistrarEleitor("123");
        Assert.Equal(esperado, eleicao.Votar("123", numero));
        Assert.True(eleitor.JaVotou);
        Assert.Equal(1, eleicao.Apurar().TotalVotos);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(0)]
    [InlineData(999)]
    public void Votar_ImpedeSegundoVotoInclusiveAposBrancoOuNulo(int primeiroVoto)
    {
        var eleicao = CriarEleicao();
        eleicao.RegistrarEleitor("123");
        eleicao.Votar("123", primeiroVoto);
        Assert.Throws<InvalidOperationException>(() => eleicao.Votar("123", 20));
        Assert.Equal(1, eleicao.Apurar().TotalVotos);
        Assert.Equal(0, eleicao.Apurar().VotosPorCandidato.Single(v => v.Candidato.Numero == 20).TotalVotos);
    }

    [Fact]
    public void Votar_RejeitaEleitorNaoRegistradoSemAlterarApuracao()
    {
        var eleicao = CriarEleicao();
        Assert.Throws<InvalidOperationException>(() => eleicao.Votar("123", 10));
        Assert.Equal(0, eleicao.Apurar().TotalVotos);
    }

    [Fact]
    public void Votar_ReconheceDocumentoComOutraFormatacao()
    {
        var eleicao = CriarEleicao();
        eleicao.RegistrarEleitor("123.456.789-00");
        eleicao.Votar("12345678900", 10);
        Assert.Throws<InvalidOperationException>(() => eleicao.Votar("123.456.789-00", 10));
    }
}
