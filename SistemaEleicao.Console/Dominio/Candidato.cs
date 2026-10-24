namespace SistemaEleicao.ConsoleApp.Dominio;

public sealed record Candidato
{
    public string Nome { get; }
    public int Numero { get; }

    public Candidato(string nome, int numero)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Informe o nome do candidato.", nameof(nome));
        if (numero <= 0)
            throw new ArgumentOutOfRangeException(nameof(numero), "O número deve ser maior que zero; zero é reservado ao voto branco.");
        Nome = nome.Trim();
        Numero = numero;
    }
}
