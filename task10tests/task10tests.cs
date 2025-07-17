using Xunit;
using task10;

namespace task10tests;
public class PluginTests
{
    private class TestPluginA : IPlugin
    {
        public void Execute()
        {
            Console.WriteLine("Плагин A выполнен");
        }
    }

    private class TestPluginB : IPlugin
    {
        public void Execute()
        {
            Console.WriteLine("Плагин B выполнен");
        }
    }

    [Fact]
    public void TestPluginAExecuteOutput()
    {
        var plugin = new TestPluginA();
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        plugin.Execute();

        string output = stringWriter.ToString().Trim();
        Assert.Equal("Плагин A выполнен", output);
    }

    [Fact]
    public void TestPluginBExecuteOutput()
    {
        var plugin = new TestPluginB();
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        plugin.Execute();

        string output = stringWriter.ToString().Trim();
        Assert.Equal("Плагин B выполнен", output);
    }

    [Fact]
    public void TestPluginLoaderNoDependencies()
    {
        var pluginsPath = Path.Combine(Path.GetTempPath(), "Plugins");
        Directory.CreateDirectory(pluginsPath);
        var loader = new PluginLoader(pluginsPath);

        loader.LoadAndExecutePlugins();

        Directory.Delete(pluginsPath, true);
    }
}