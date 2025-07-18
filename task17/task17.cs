using System.Collections.Concurrent;
using System.Threading;

namespace task17;

public interface ICommand
{
    void Execute();
}

public interface IExceptionHandler
{
    void Handle(ICommand command, Exception exception);
}

public class ServerThread
{
    private readonly ConcurrentQueue<ICommand> _commands;
    private readonly Thread _thread;
    private readonly IExceptionHandler? _exceptionHandler;
    private volatile bool _running;
    private readonly ManualResetEvent _signal;

    public ServerThread(IExceptionHandler? exceptionHandler = null)
    {
        _commands = new ConcurrentQueue<ICommand>();
        _exceptionHandler = exceptionHandler;
        _running = true;
        _signal = new ManualResetEvent(false);
        _thread = new Thread(Run);
        _thread.Start();
    }

    public int ThreadId => _thread.ManagedThreadId;

    public bool IsRunning => _thread.IsAlive;

    public void EnqueueCommand(ICommand command)
    {
        _commands.Enqueue(command);
        _signal.Set();
    }

    private void Run()
    {
        while (_running || !_commands.IsEmpty)
        {
            if (_commands.TryDequeue(out var command))
            {
                try
                {
                    command.Execute();
                }
                catch (ThreadInterruptedException)
                {
                    _running = false;
                    break; // Немедленно выходим из цикла для HardStop
                }
                catch (Exception ex)
                {
                    _exceptionHandler?.Handle(command, ex);
                }
            }
            else
            {
                _signal.Reset();
                _signal.WaitOne();
            }
        }
    }

    public void Stop()
    {
        _running = false;
        _signal.Set();
        _thread.Join(500);
    }
}

public class HardStopCommand : ICommand
{
    private readonly int _targetThreadId;

    public HardStopCommand(int targetThreadId)
    {
        _targetThreadId = targetThreadId;
    }

    public void Execute()
    {
        if (Thread.CurrentThread.ManagedThreadId != _targetThreadId)
            throw new InvalidOperationException("Команда HardStop должна выполняться в целевом потоке");

        throw new ThreadInterruptedException("Немедленная остановка потока");
    }
}

public class SoftStopCommand : ICommand
{
    private readonly int _targetThreadId;
    private readonly ServerThread _serverThread;

    public SoftStopCommand(int targetThreadId, ServerThread serverThread)
    {
        _targetThreadId = targetThreadId;
        _serverThread = serverThread;
    }

    public void Execute()
    {
        if (Thread.CurrentThread.ManagedThreadId != _targetThreadId)
            throw new InvalidOperationException("Команда SoftStop должна выполняться в целевом потоке");

        _serverThread.Stop();
    }
}
