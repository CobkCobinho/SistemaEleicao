namespace SistemaEleicao.ConsoleApp.Dominio;

public sealed class EleicaoManager
{
    private readonly Dictionary<int, Candidato> _candidatos = new();
    private readonly Dictionary<string, Eleitor> _eleitores = new(StringComparer.Ordinal);
    private readonly Dictionary<int, int> _votosPorCandidato = new();
    private int _votosBrancos;
    private int _votosNulos;

    public IReadOnlyList<Candidato> Candidatos => _candidatos.Values.OrderBy(c => c.Numero).ToList().AsReadOnly();
    public IReadOnlyList<Eleitor> Eleitores => _eleitores.Values.ToList().AsReadOnly();

    public Candidato CadastrarCandidato(string nome, int numero)
    {
        var candidato = new Candidato(nome, numero);
        if (!_candidatos.TryAdd(numero, candidato))
            throw new InvalidOperationException("Já existe um candidato com esse número.");
        _votosPorCandidato.Add(numero, 0);
        return candidato;
    }

    public Eleitor RegistrarEleitor(string documento)
    {
        var identificador = Eleitor.NormalizarDocumento(documento);
        var eleitor = new Eleitor(identificador);
        if (!_eleitores.TryAdd(identificador, eleitor))
            throw new InvalidOperationException("Esse eleitor já está cadastrado.");
        return eleitor;
    }

    public TipoVoto Votar(string documento, int numeroCandidato)
    {
        var identificador = Eleitor.NormalizarDocumento(documento);
        if (!_eleitores.TryGetValue(identificador, out var eleitor))
            throw new InvalidOperationException("Eleitor não cadastrado. Registre o eleitor antes de votar.");
        if (eleitor.JaVotou)
            throw new InvalidOperationException("Este eleitor já votou. Um segundo voto não é permitido.");

        TipoVoto tipo;
        if (numeroCandidato == 0)
        {
            _votosBrancos++;
            tipo = TipoVoto.Branco;
        }
        else if (_candidatos.ContainsKey(numeroCandidato))
        {
            _votosPorCandidato[numeroCandidato]++;
            tipo = TipoVoto.Valido;
        }
        else
        {
            _votosNulos++;
            tipo = TipoVoto.Nulo;
        }
        eleitor.MarcarComoVotante();
        return tipo;
    }

    public ResultadoEleicao Apurar() => new(
        _candidatos.Values.Select(c => new ResultadoCandidato(c, _votosPorCandidato[c.Numero])),
        _votosBrancos, _votosNulos);
}
