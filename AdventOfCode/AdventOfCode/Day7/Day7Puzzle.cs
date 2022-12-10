namespace AdventOfCode.Day7;

public class Day7Puzzle
{
    public static int GetSumOfSizesOfDirectories(TerminalOutput terminalOutput)
    {
        // TODO: 
        throw new System.NotImplementedException();
    }
}

public record TerminalOutput(TerminalCommandOutput[] CommandOutputs);

public class TerminalCommandOutput
{
}

public class ChangeDirectoryTerminalCommandOutput : TerminalCommandOutput
{
    public ChangeDirectoryTerminalCommandOutput(string directoryName)
    {
        DirectoryName = directoryName;
    }

    public string DirectoryName { get; }
}

public class ListDirectoryContentsTerminalCommandOutput : TerminalCommandOutput
{
    public ListDirectoryContentsTerminalCommandOutput(DirectorySummary[] directories, FileSummary[] files)
    {
        Directories = directories;
        Files = files;
    }

    public DirectorySummary[] Directories { get; }
    
    public FileSummary[] Files { get; }
}

public record FileSummary(string Name, int Size);

public record DirectorySummary(string Name);
