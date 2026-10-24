using SistemaEleicao.ConsoleApp.Apresentacao;
using SistemaEleicao.ConsoleApp.Dominio;
using System.Text;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;
new MenuEleicao(new EleicaoManager(), Console.In, Console.Out).Executar();

