using System;
using System.IO;
using Xunit;

namespace task09tests
{
    public class UnitTest1
    {
        [Fact]
        public void Program_NoArguments_ShouldPrintErrorMessage()
        {
            var output = new StringWriter();
            Console.SetOut(output);

            task09.Program.Main(new string[] { });

            Assert.Contains("Укажите путь к .dll", output.ToString());

            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        }

        [Fact]
        public void Program_InvalidPath_ShouldPrintErrorMessage()
        {
            var output = new StringWriter();
            Console.SetOut(output);

            task09.Program.Main(new[] { "invalid.dll" });

            Assert.Contains("Файл не найден", output.ToString());

            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        }

        [Fact]
        public void Program_ShouldPrintCorrectClass()
        {
            var baseDir = AppContext.BaseDirectory;
            var path = Path.Combine(baseDir, "task07.dll");

            var output = new StringWriter();
            Console.SetOut(output);

            task09.Program.Main(new[] { path });

            Assert.Contains("Класс: SampleClass", output.ToString());

            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        }

        [Fact]
        public void Program_ShouldPrintCorrectAttributes()
        {
            var baseDir = AppContext.BaseDirectory;
            var path = Path.Combine(baseDir, "task07.dll");

            var output = new StringWriter();
            Console.SetOut(output);

            task09.Program.Main(new[] { path });

            Assert.Contains("Атрибуты:", output.ToString());
            Assert.Contains("DisplayNameAttribute", output.ToString());
            Assert.Contains("VersionAttribute", output.ToString());

            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        }

        [Fact]
        public void Program_ShouldPrintCorrectConstructors()
        {
            var baseDir = AppContext.BaseDirectory;
            var path = Path.Combine(baseDir, "task07.dll");

            var output = new StringWriter();
            Console.SetOut(output);

            task09.Program.Main(new[] { path });

            Assert.Contains("Конструкторы:", output.ToString());
            Assert.Contains("SampleClass()", output.ToString());

            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        }

        [Fact]
        public void Program_ShouldPrintCorrectMethods()
        {
            var baseDir = AppContext.BaseDirectory;
            var path = Path.Combine(baseDir, "task07.dll");

            var output = new StringWriter();
            Console.SetOut(output);

            task09.Program.Main(new[] { path });

            Assert.Contains("Методы:", output.ToString());
            Assert.Contains("Метод: TestMethod", output.ToString());
            Assert.Contains("Атрибут: DisplayNameAttribute", output.ToString());
            Assert.Contains("Параметры: ", output.ToString());

            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        }

        [Fact]
        public void Program_ShouldPrintCorrectProperties()
        {
            var baseDir = AppContext.BaseDirectory;
            var path = Path.Combine(baseDir, "task07.dll");

            var output = new StringWriter();
            Console.SetOut(output);

            task09.Program.Main(new[] { path });

            Assert.Contains("Свойства:", output.ToString());
            Assert.Contains("Свойство: Number, Тип: Int32", output.ToString());
            Assert.Contains("Атрибут: DisplayNameAttribute", output.ToString());

            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        }
    }
}