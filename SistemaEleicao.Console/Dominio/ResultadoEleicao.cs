namespace SistemaEleicao.ConsoleApp.Dominio;

public sealed record ResultadoCandidato(Candidato Candidato, int TotalVotos);

public sealed class ResultadoEleicao
{
    public IReadOnlyList<ResultadoCandidato> VotosPorCandidato { get; }
    public IReadOnlyList<Candidato> Vencedores { get; }
    public int VotosBrancos { get; }
    public int VotosNulos { get; }
    public int VotosValidos => VotosPorCandidato.Sum(v => v.TotalVotos);
    public int TotalVotos => VotosValidos + VotosBrancos + VotosNulos;
    public bool HouveEmpate => Vencedores.Count > 1;
    public Candidato? Vencedor => Vencedores.Count == 1 ? Vencedores[0] : null;

    internal ResultadoEleicao(IEnumerable<ResultadoCandidato> votos, int brancos, int nulos)
    {
        VotosPorCandidato = votos.OrderBy(v => v.Candidato.Numero).ToList().AsReadOnly();
        VotosBrancos = brancos;
        VotosNulos = nulos;
        var maiorTotal = VotosPorCandidato.Select(v => v.TotalVotos).DefaultIfEmpty(0).Max();
        Vencedores = VotosPorCandidato
            .Where(v => maiorTotal > 0 && v.TotalVotos == maiorTotal)
            .Select(v => v.Candidato).ToList().AsReadOnly();
    }
}
