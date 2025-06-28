using System.Reflection;

namespace task05;

public class ClassAnalyzer
{
    private Type _type;

    public ClassAnalyzer(Type type)
    {
        _type = type;
    }

    public IEnumerable<string> GetPublicMethods()
    {
        return _type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Select(m => m.Name);
    }

    public IEnumerable<string?> GetMethodParams(string methodname)
    {
        var method = _type.GetMethod(methodname, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        return method?.GetParameters().Select(p => p.Name) ?? Enumerable.Empty<string>();
    }

    public IEnumerable<string> GetAllFields()
    {
        return _type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Select(f => f.Name);
    }

    public IEnumerable<string> GetProperties()
    {
        return _type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Select(p => p.Name);
    }

    public bool HasAttribute<T>() where T : Attribute
    {
        return Attribute.IsDefined(_type, typeof(T));
    }
}