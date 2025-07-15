using System;
using System.IO;
using System.Reflection;
using task07;

namespace task09
{
    public class Program
    {
        public static void Main(string[] path)
        {
            if (path.Length == 0)
            {
                Console.WriteLine("Укажите путь к .dll");
                return;
            }

            string dll_path = path[0];
            if (!File.Exists(dll_path))
            {
                Console.WriteLine("Файл не найден");
                return;
            }

            Assembly assembly = Assembly.LoadFrom(dll_path);

            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsClass)
                {
                    Console.WriteLine($"Класс: {type.Name}");

                    Console.WriteLine("Атрибуты:");
                    foreach (var attribute in type.GetCustomAttributes())
                    {
                        Console.WriteLine($"  {attribute.GetType().Name}");
                    }

                    Console.WriteLine("Конструкторы:");
                    foreach (var constructor in type.GetConstructors())
                    {
                        Console.WriteLine($"  {type.Name}({string.Join(", ", constructor.GetParameters().Select(p => $"{p.Name}: {p.ParameterType.Name}"))})");
                    }

                    Console.WriteLine("Методы:");
                    foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    {
                        Console.WriteLine($"  Метод: {method.Name}");
                        var methodAttr = method.GetCustomAttribute<DisplayNameAttribute>();
                        if (methodAttr != null) Console.WriteLine($"    Атрибут: {methodAttr.GetType().Name}");
                        Console.WriteLine($"    Параметры: {string.Join(", ", method.GetParameters().Select(p => $"{p.Name}: {p.ParameterType.Name}"))}");
                    }

                    Console.WriteLine("Свойства:");
                    foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    {
                        Console.WriteLine($"  Свойство: {property.Name}, Тип: {property.PropertyType.Name}");
                        var propertyAttr = property.GetCustomAttribute<DisplayNameAttribute>();
                        if (propertyAttr != null) Console.WriteLine($"    Атрибут: {propertyAttr.GetType().Name}");
                    }

                    Console.WriteLine();
                }
            }
        }
    }
}
