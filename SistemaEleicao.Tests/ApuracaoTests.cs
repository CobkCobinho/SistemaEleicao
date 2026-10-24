using SistemaEleicao.ConsoleApp.Dominio;

namespace SistemaEleicao.Tests;

public class ApuracaoTests
{
    [Fact]
    public void Apurar_ContabilizaTodosOsTiposEDeterminaVencedor()
    {
        var eleicao = new EleicaoManager();
        eleicao.CadastrarCandidato("Bruno", 20);
        eleicao.CadastrarCandidato("Ana", 10);
        int[] votos = [10, 10, 20, 0, -1, 999];
        for (var i = 0; i < votos.Length; i++)
        {
            eleicao.RegistrarEleitor(i.ToString());
            eleicao.Votar(i.ToString(), votos[i]);
        }
        var resultado = eleicao.Apurar();
        Assert.Equal(6, resultado.TotalVotos);
        Assert.Equal(3, resultado.VotosValidos);
        Assert.Equal(1, resultado.VotosBrancos);
        Assert.Equal(2, resultado.VotosNulos);
        Assert.Equal([10, 20], resultado.VotosPorCandidato.Select(v => v.Candidato.Numero));
        Assert.Equal([2, 1], resultado.VotosPorCandidato.Select(v => v.TotalVotos));
        Assert.Equal(10, resultado.Vencedor!.Numero);
        Assert.False(resultado.HouveEmpate);
        Assert.Equal(resultado.TotalVotos, eleicao.Eleitores.Count(e => e.JaVotou));
    }

    [Fact]
    public void Apurar_EmpateNaoEscolheVencedorArbitrario()
    {
        var eleicao = new EleicaoManager();
        eleicao.CadastrarCandidato("Ana", 10);
        eleicao.CadastrarCandidato("Bruno", 20);
        eleicao.RegistrarEleitor("1");
        eleicao.RegistrarEleitor("2");
        eleicao.Votar("1", 10);
        eleicao.Votar("2", 20);
        var resultado = eleicao.Apurar();
        Assert.True(resultado.HouveEmpate);
        Assert.Null(resultado.Vencedor);
        Assert.Equal(2, resultado.Vencedores.Count);
    }

    [Fact]
    public void Apurar_SemCandidatosOuVotosNaoTemVencedor()
    {
        var resultado = new EleicaoManager().Apurar();
        Assert.Empty(resultado.VotosPorCandidato);
        Assert.Empty(resultado.Vencedores);
        Assert.Null(resultado.Vencedor);
        Assert.False(resultado.HouveEmpate);
        Assert.Equal(0, resultado.TotalVotos);
    }

    [Fact]
    public void Apurar_CandidatoSemVotosNaoVence()
    {
        var eleicao = new EleicaoManager();
        eleicao.CadastrarCandidato("Ana", 10);
        eleicao.RegistrarEleitor("1");
        eleicao.Votar("1", 0);
        var resultado = eleicao.Apurar();
        Assert.Equal(0, Assert.Single(resultado.VotosPorCandidato).TotalVotos);
        Assert.Null(resultado.Vencedor);
        Assert.False(resultado.HouveEmpate);
    }

    [Fact]
    public void Apurar_SomenteNulosNaoTemVencedor()
    {
        var eleicao = new EleicaoManager();
        eleicao.RegistrarEleitor("1");
        eleicao.Votar("1", 999);
        var resultado = eleicao.Apurar();
        Assert.Equal(1, resultado.VotosNulos);
        Assert.Equal(1, resultado.TotalVotos);
        Assert.Null(resultado.Vencedor);
    }

    [Fact]
    public void Apurar_RetornaRetratoIndependenteDosVotosPosteriores()
    {
        var eleicao = new EleicaoManager();
        eleicao.CadastrarCandidato("Ana", 10);
        var anterior = eleicao.Apurar();
        eleicao.RegistrarEleitor("1");
        eleicao.Votar("1", 10);
        Assert.Equal(0, anterior.TotalVotos);
        Assert.Equal(0, Assert.Single(anterior.VotosPorCandidato).TotalVotos);
        Assert.Equal(1, eleicao.Apurar().TotalVotos);
    }
}
