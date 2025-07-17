using System.Reflection;

namespace task10;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class PluginLoadAttribute : Attribute
{
    public string[] Dependencies { get; }

    public PluginLoadAttribute(params string[] dependencies)
    {
        Dependencies = dependencies;
    }
}

public interface IPlugin
{
    void Execute();
}

public class PluginLoader
{
    private readonly string _pluginsPath;
    private readonly Dictionary<string, Type> _pluginTypes;
    private readonly Dictionary<string, string[]> _dependencies;

    public PluginLoader(string pluginsPath)
    {
        _pluginsPath = pluginsPath;
        _pluginTypes = new Dictionary<string, Type>();
        _dependencies = new Dictionary<string, string[]>();
    }

    public void LoadAndExecutePlugins()
    {
        DiscoverPlugins();
        var sortedPlugins = TopologicalSort();
        ExecutePlugins(sortedPlugins);
    }

    private void DiscoverPlugins()
    {
        foreach (var dll in Directory.GetFiles(_pluginsPath, "*.dll"))
        {
            try
            {
                var assembly = Assembly.LoadFrom(dll);
                foreach (var type in assembly.GetTypes())
                {
                    var attr = type.GetCustomAttribute<PluginLoadAttribute>();
                    if (attr != null && typeof(IPlugin).IsAssignableFrom(type))
                    {
                        _pluginTypes[type.FullName] = type;
                        _dependencies[type.FullName] = attr.Dependencies ?? Array.Empty<string>();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки сборки {dll}: {ex.Message}");
            }
        }
    }

    private List<Type> TopologicalSort()
    {
        var sorted = new List<Type>();
        var visited = new HashSet<string>();
        var tempMarks = new HashSet<string>();

        void Visit(string pluginName)
        {
            if (pluginName == null) return;
            if (tempMarks.Contains(pluginName))
                throw new InvalidOperationException($"Обнаружена циклическая зависимость: {pluginName}");

            if (!visited.Contains(pluginName) && _pluginTypes.ContainsKey(pluginName))
            {
                tempMarks.Add(pluginName);
                foreach (var dep in _dependencies[pluginName])
                {
                    Visit(dep);
                }
                tempMarks.Remove(pluginName);
                visited.Add(pluginName);
                sorted.Add(_pluginTypes[pluginName]);
            }
        }

        foreach (var plugin in _pluginTypes.Keys)
        {
            if (!visited.Contains(plugin))
                Visit(plugin);
        }

        return sorted;
    }

    private void ExecutePlugins(List<Type> sortedPlugins)
    {
        foreach (var type in sortedPlugins)
        {
            try
            {
                var plugin = Activator.CreateInstance(type);
                if (plugin is IPlugin instance)
                {
                    instance.Execute();
                }
                else
                {
                    Console.WriteLine($"Ошибка: плагин {type.FullName} не реализует IPlugin");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка выполнения плагина {type.FullName}: {ex.Message}");
            }
        }
    }
}