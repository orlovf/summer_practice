using System;
using System.Threading;
using System.Threading.Tasks;

namespace task14;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {
        double result = 0;
        var barrier = new Barrier(threadsNumber);
        var tasks = new Task[threadsNumber];
        double segmentLength = (b - a) / threadsNumber;

        for (int i = 0; i < threadsNumber; i++)
        {
            int threadIndex = i;
            double start = a + threadIndex * segmentLength;
            double end = start + segmentLength;

            tasks[i] = Task.Run(() =>
            {
                double localSum = 0;
                for (double x = start; x < end; x += step)
                {
                    double nextX = Math.Min(x + step, end);
                    localSum += (function(x) + function(nextX)) * (nextX - x) / 2;
                }
                Interlocked.Exchange(ref result, result + localSum);
                barrier.SignalAndWait();
            });
        }

        Task.WaitAll(tasks);
        return result;
    }
}