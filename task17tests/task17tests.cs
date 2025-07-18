using System.Threading;
using Xunit;
using task17;

namespace task17tests;

public class ServerThreadTests
{
    private class TestCommand : ICommand
    {
        private readonly Action _action;

        public TestCommand(Action action)
        {
            _action = action;
        }

        public void Execute()
        {
            _action();
        }
    }

    [Fact]
    public void HardStop_StopsImmediately()
    {
        var server = new ServerThread();
        bool command1Executed = false;
        bool command2Executed = false;

        server.EnqueueCommand(new TestCommand(() => command1Executed = true));
        server.EnqueueCommand(new HardStopCommand(server.ThreadId));
        server.EnqueueCommand(new TestCommand(() => command2Executed = true));

        server.Stop();

        Assert.True(command1Executed);
        Assert.False(command2Executed);
        Assert.False(server.IsRunning);
    }

    [Fact]
    public void SoftStop_ExecutesAllCommandsBeforeStopping()
    {
        var server = new ServerThread();
        bool command1Executed = false;
        bool command2Executed = false;

        server.EnqueueCommand(new TestCommand(() => command1Executed = true));
        server.EnqueueCommand(new TestCommand(() => command2Executed = true));
        server.EnqueueCommand(new SoftStopCommand(server.ThreadId, server));

        Thread.Sleep(100);
        server.Stop();

        Assert.True(command1Executed);
        Assert.True(command2Executed);
        Assert.False(server.IsRunning);
    }

    [Fact]
    public void HardStop_WrongThread_ThrowsException()
    {
        var server = new ServerThread();
        var wrongThreadId = Thread.CurrentThread.ManagedThreadId + 1;
        var command = new HardStopCommand(wrongThreadId);

        var exception = Assert.Throws<InvalidOperationException>(() => command.Execute());
        Assert.Equal("Команда HardStop должна выполняться в целевом потоке", exception.Message);
    }

    [Fact]
    public void SoftStop_WrongThread_ThrowsException()
    {
        var server = new ServerThread();
        var wrongThreadId = Thread.CurrentThread.ManagedThreadId + 1;
        var command = new SoftStopCommand(wrongThreadId, server);

        var exception = Assert.Throws<InvalidOperationException>(() => command.Execute());
        Assert.Equal("Команда SoftStop должна выполняться в целевом потоке", exception.Message);
    }
}