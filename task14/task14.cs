using System;
using System.Threading.Tasks;

namespace task14;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {
        double[] partialSums = new double[threadsNumber];
        double segmentLength = (b - a) / threadsNumber;

        Parallel.For(0, threadsNumber, i =>
        {
            double start = a + i * segmentLength;
            double end = i == threadsNumber - 1 ? b : start + segmentLength;
            double localSum = 0;
            for (double x = start; x < end; x += step)
            {
                double nextX = Math.Min(x + step, end);
                localSum += (function(x) + function(nextX)) * (nextX - x) / 2;
            }
            partialSums[i] = localSum;
        });

        return partialSums.Sum();
    }

    public static double SolveSingleThread(double a, double b, Func<double, double> function, double step)
    {
        double result = 0;
        for (double x = a; x < b; x += step)
        {
            double nextX = Math.Min(x + step, b);
            result += (function(x) + function(nextX)) * (nextX - x) / 2;
        }
        return result;
    }
}