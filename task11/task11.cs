using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace task11;

public interface ICalculator
{
    int Add(int a, int b);
    int Minus(int a, int b);
    int Mul(int a, int b);
    int Div(int a, int b);
}

public static class CalculatorGenerator
{
    public static ICalculator BuildCalculator(string code)
    {
        var tree = CSharpSyntaxTree.ParseText(code);
        var references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ICalculator).Assembly.Location)
        };

        var compilation = CSharpCompilation.Create(
            "CalcGenAssembly",
            new[] { tree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using var ms = new MemoryStream();
        var result = compilation.Emit(ms);
        if (!result.Success)
        {
            throw new Exception("Compilation failed.");
        }

        ms.Seek(0, SeekOrigin.Begin);
        var assembly = Assembly.Load(ms.ToArray());
        var type = assembly.GetType("Calculator");
        var instance = Activator.CreateInstance(type);
        return (ICalculator)instance;
    }
}