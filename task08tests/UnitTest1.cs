using FileSystemCommands;
using CommandRunner;

namespace FileSystemCommandsTests
{
    public class UnitTest1
    {
        [Fact]
        public void DirectorySizeCommand_ShouldCalculateSize()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
            File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

            var command = new DirectorySizeCommand(testDir);
            command.Execute();

            Assert.Equal(10, command.Size);

            Directory.Delete(testDir, true);
        }

        [Fact]
        public void FindFilesCommand_ShouldFindMatchingFiles()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
            File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

            var command = new FindFilesCommand(testDir, "*.txt");
            command.Execute();

            Assert.Single(command.Files);
            Assert.Contains("file1.txt", command.Files);

            Directory.Delete(testDir, true);
        }

        [Fact]
        public void CommandRunner_PrintsCorrectOutput()
        {
            var output = new StringWriter();
            Console.SetOut(output);

            Program.Main(new string[] {});

            Assert.Contains("Total size: 10 bytes", output.ToString());
            Assert.Contains("File: test1.txt", output.ToString());

            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        }
    }
}
