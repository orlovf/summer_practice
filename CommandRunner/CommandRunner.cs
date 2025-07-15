using System;
using System.IO;
using System.Reflection;
using CommandLib;
using FileSystemCommands;

namespace CommandRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileSystemCommands.dll");
            if (!File.Exists(dllPath))
            {
                Console.WriteLine("DLL не найдена: " + dllPath);
                return;
            }

            Assembly assembly = Assembly.LoadFrom(dllPath);
            Type[] types = assembly.GetTypes();

            var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
            File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

            foreach (Type type in types)
            {
                if (typeof(ICommand).IsAssignableFrom(type) && !type.IsInterface)
                {
                    ConstructorInfo constructor = type.GetConstructor(new[] { typeof(string) }) ?? 
                                                 type.GetConstructor(new[] { typeof(string), typeof(string) });
                    if (constructor != null)
                    {
                        ICommand command = (ICommand)constructor.Invoke(constructor.GetParameters().Length == 1 
                            ? new object[] { testDir } 
                            : new object[] { testDir, "*.txt" });
                        command.Execute();
                        if (command is DirectorySizeCommand sizeCommand)
                        {
                            Console.WriteLine($"Total size: {sizeCommand.Size} bytes");
                        }
                        else if (command is FindFilesCommand filesCommand)
                        {
                            foreach (var file in filesCommand.Files)
                            {
                                Console.WriteLine($"File: {file}");
                            }
                        }
                    }
                }
            }

            Directory.Delete(testDir, true);
        }
    }
}