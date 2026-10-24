namespace SistemaEleicao.ConsoleApp.Dominio;

public sealed class Eleitor
{
    public string Documento { get; }
    public bool JaVotou { get; private set; }

    internal Eleitor(string documento) => Documento = documento;
    internal void MarcarComoVotante() => JaVotou = true;

    internal static string NormalizarDocumento(string documento)
    {
        if (string.IsNullOrWhiteSpace(documento) ||
            documento.Any(c => !char.IsAsciiDigit(c) && c is not '.' and not '-' and not '/' && !char.IsWhiteSpace(c)))
            throw new ArgumentException("Informe um documento simulado com dígitos; pontuação é opcional.", nameof(documento));
        var normalizado = string.Concat(documento.Where(char.IsAsciiDigit));
        if (normalizado.Length == 0)
            throw new ArgumentException("O documento deve conter pelo menos um dígito.", nameof(documento));
        return normalizado;
    }
}
