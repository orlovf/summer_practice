using Xunit;
using task11;

namespace task11Tests;

public class CalculatorTests
{
    private const string CalculatorCode = @"
        using task11;

        public class Calculator : ICalculator
        {
            public int Add(int a, int b) => a + b;
            public int Minus(int a, int b) => a - b;
            public int Mul(int a, int b) => a * b;
            public int Div(int a, int b) => a / b;
        }";

    [Fact]
    public void AdditionTest()
    {
        var calc = CalculatorGenerator.BuildCalculator(CalculatorCode);
        Assert.Equal(9, calc.Add(5, 4));
    }

    [Fact]
    public void SubtractionTest()
    {
        var calc = CalculatorGenerator.BuildCalculator(CalculatorCode);
        Assert.Equal(4, calc.Minus(6, 2));
    }

    [Fact]
    public void MultiplicationTest()
    {
        var calc = CalculatorGenerator.BuildCalculator(CalculatorCode);
        Assert.Equal(12, calc.Mul(3, 4));
    }

    [Fact]
    public void DivisionTest()
    {
        var calc = CalculatorGenerator.BuildCalculator(CalculatorCode);
        Assert.Equal(3, calc.Div(9, 3));
    }

    [Fact]
    public void DivisionByZeroTest()
    {
        var calc = CalculatorGenerator.BuildCalculator(CalculatorCode);
        Assert.Throws<DivideByZeroException>(() => calc.Div(8, 0));
    }
}