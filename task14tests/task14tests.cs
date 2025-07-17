using System;
using Xunit;
using task14;

namespace task14tests;

public class IntegralTests
{
    private readonly Func<double, double> X = x => x;
    private readonly Func<double, double> SIN = x => Math.Sin(x);

    [Fact]
    public void LinearFunction_TwoThreads_ReturnsZero()
    {
        double result = DefiniteIntegral.Solve(-1, 1, X, 1e-4, 2);
        Assert.Equal(0, result, 4);
    }

    [Fact]
    public void SinFunction_EightThreads_ReturnsZero()
    {
        double result = DefiniteIntegral.Solve(-1, 1, SIN, 1e-5, 8);
        Assert.Equal(0, result, 4);
    }

    [Fact]
    public void LinearFunction_EightThreads_ReturnsTwelvePointFive()
    {
        double result = DefiniteIntegral.Solve(0, 5, X, 1e-6, 8);
        Assert.Equal(12.5, result, 5);
    }
}