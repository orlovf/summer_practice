using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using task14;
using ScottPlot;

namespace task15;

class Program
{
    private static readonly Func<double, double> SIN = x => Math.Sin(x);
    private static readonly double[] Steps = { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };
    private static readonly int[] ThreadCounts = { 1, 2, 4, 8, 16 };
    private const int Iterations = 100;

    static void Main()
    {
        double a = -100, b = 100;
        double expected = 0;
        double bestStep = 0;
        double minError = double.MaxValue;
        double bestSingleThreadTime = 0;
        double bestMultiThreadTime = double.MaxValue;
        int bestThreadCount = 0;
        double[] times = new double[ThreadCounts.Length];

        foreach (double step in Steps)
        {
            double result = DefiniteIntegral.SolveSingleThread(a, b, SIN, step);
            double error = Math.Abs(result - expected);
            if (error < 1e-4 && step > bestStep)
            {
                minError = error;
                bestStep = step;
            }
        }

        double singleThreadTotalTime = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var stopwatch = Stopwatch.StartNew();
            DefiniteIntegral.SolveSingleThread(a, b, SIN, bestStep);
            stopwatch.Stop();
            singleThreadTotalTime += stopwatch.ElapsedTicks;
        }
        bestSingleThreadTime = singleThreadTotalTime / Iterations / Stopwatch.Frequency * 1000;

        for (int i = 0; i < ThreadCounts.Length; i++)
        {
            int threads = ThreadCounts[i];
            double totalTime = 0;
            for (int j = 0; j < Iterations; j++)
            {
                var stopwatch = Stopwatch.StartNew();
                DefiniteIntegral.Solve(a, b, SIN, bestStep, threads);
                stopwatch.Stop();
                totalTime += stopwatch.ElapsedTicks;
            }
            times[i] = totalTime / Iterations / Stopwatch.Frequency * 1000;
            if (times[i] < bestMultiThreadTime)
            {
                bestMultiThreadTime = times[i];
                bestThreadCount = threads;
            }
        }

        double percentDifference = (bestSingleThreadTime - bestMultiThreadTime) / bestSingleThreadTime * 100;

        File.WriteAllText("results.txt",
            $"Оптимальный шаг: {bestStep} \n" +
            $"Оптимальное количество потоков: {bestThreadCount} \n" +
            $"Время однопоточной версии: {bestSingleThreadTime:F3} мс \n" +
            $"Время многопоточной версии: {bestMultiThreadTime:F3} мс \n" +
            $"Разница в производительности: {percentDifference:F2}% \n");
            
        var plot = new Plot();
        plot.Add.Scatter(times, ThreadCounts);
        plot.XLabel("Время выполнения (мс)");
        plot.YLabel("Количество потоков");
        plot.Title("Производительность многопоточного вычисления интеграла");
        plot.SavePng("plot.png", 800, 600);
    }
}