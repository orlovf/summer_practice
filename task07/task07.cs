using System.Reflection;
namespace task07;

public class DisplayNameAttribute : Attribute
{
    public string DisplayName = string.Empty;
    public DisplayNameAttribute(string DisplayName)
    {
        this.DisplayName = DisplayName;
    }
}

public class VersionAttribute : Attribute
{
    public int Major { get; }
    public int Minor { get; }
    
    public VersionAttribute(int Major, int Minor)
    {
        this.Major = Major;
        this.Minor = Minor;
    }
}

[DisplayName("Пример класса")]
[Version(1, 0)]
public class SampleClass
{
    [DisplayName("Тестовый метод")]
    public void TestMethod() { }
    
    [DisplayName("Числовое свойство")]
    public int Number { get; set; }
}

public static class ReflectionHelper
{
    public static void PrintTypeInfo(Type type)
    {
        var displayName = type.GetCustomAttribute<DisplayNameAttribute>();
        if (displayName != null) Console.WriteLine(displayName.DisplayName);

        var version = type.GetCustomAttribute<VersionAttribute>();
        if (version != null) Console.WriteLine($"{version.Major}.{version.Minor}");

        foreach (var method in type.GetMethods())
        {
            var methodAttr = method.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault();
            if (methodAttr != null) Console.WriteLine($"{method.Name}: {methodAttr.DisplayName}");
        }

        foreach (var property in type.GetProperties())
        {
            var propertyAttr = property.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault();
            if (propertyAttr != null) Console.WriteLine($"{property.Name}: {propertyAttr.DisplayName}");
        }
    }
}