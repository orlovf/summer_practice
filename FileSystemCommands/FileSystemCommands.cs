using CommandLib;

namespace FileSystemCommands
{
    public class DirectorySizeCommand : ICommand
    {
        private readonly string _directoryPath;
        public long Size { get; private set; }

        public DirectorySizeCommand(string directoryPath)
        {
            _directoryPath = directoryPath;
        }

        public void Execute()
        {
            Size = 0;
            if (Directory.Exists(_directoryPath))
            {
                foreach (var file in Directory.GetFiles(_directoryPath, "*", SearchOption.TopDirectoryOnly))
                {
                    Size += new FileInfo(file).Length;
                }
            }
        }
    }

    public class FindFilesCommand : ICommand
    {
        private readonly string _directoryPath;
        private readonly string _searchPattern;
        public List<string> Files { get; private set; } = new List<string>();

        public FindFilesCommand(string directoryPath, string searchPattern)
        {
            _directoryPath = directoryPath;
            _searchPattern = searchPattern;
        }

        public void Execute()
        {
            Files.Clear();
            if (Directory.Exists(_directoryPath))
            {
                Files = Directory.GetFiles(_directoryPath, _searchPattern, SearchOption.TopDirectoryOnly)
                    .Select(f => Path.GetFileName(f)!)
                    .ToList();
            }
        }
    }
}
