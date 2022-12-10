using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day7;

public class Day7Puzzle
{
    public static int GetSumOfSizesOfDirectories(Command[] commands, int maxDirectorySizeToSum)
    {
        var rootDirectory = new Directory("/", null);
        var state = new State(rootDirectory);
        foreach (var command in commands)
        {
            ProcessCommand(command, state);
        }

        return GetAllDirectories(rootDirectory).Select(directory => TreeAggregate(
            directory,
            dir => dir.Directories,
            dir => dir.Files.Sum(f => f.Size)
        )).Sum(size => size <= maxDirectorySizeToSum ? size : 0);
    }

    private static IEnumerable<Directory> GetAllDirectories(Directory directory)
    {
        return new[] {directory}.ToList().Concat(directory.Directories.SelectMany(GetAllDirectories));
    }

    private static int TreeAggregate<TNode>(
        TNode node,
        Func<TNode, IEnumerable<TNode>> getChildren,
        Func<TNode, int> valueSelector)
    {
        var thisNodeValue = valueSelector(node);
        var childrenNodesValues = getChildren(node)
            .Select(child => TreeAggregate(child, getChildren, valueSelector))
            .Sum();

        return thisNodeValue + childrenNodesValues;
    }

    private static void ProcessCommand(Command command, State state)
    {
        switch (command)
        {
            case RootDirectoryCommand:
                state.SetCurrentDirectory(state.RootDirectory);
                break;
            case UpDirectoryCommand:
                state.SetCurrentDirectory(state.CurrentDirectory.Parent);
                break;
            case RelativeChangeDirectoryCommand relativeChangeDirectoryCommand:
                var newDirectory = state.CurrentDirectory
                    .Directories
                    .Single(d => d.Name == relativeChangeDirectoryCommand.DirectoryName);
                state.SetCurrentDirectory(newDirectory);
                break;
            case ListDirectoryContentsCommand listDirectoryContentsCommand:
                listDirectoryContentsCommand
                    .Directories.ToList()
                    .ForEach(d =>
                        state.CurrentDirectory.Directories.Add(new Directory(d.Name, state.CurrentDirectory)));
                listDirectoryContentsCommand
                    .Files.ToList()
                    .ForEach(d => state.CurrentDirectory.Files.Add(new File(d.Name, d.Size)));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(command));
        }
    }
}

public class State
{
    public State(Directory rootDirectory)
    {
        RootDirectory = rootDirectory;
    }

    public Directory RootDirectory { get; }

    public Directory? CurrentDirectory { get; private set; }

    public void SetCurrentDirectory(Directory newDirectory)
    {
        CurrentDirectory = newDirectory;
    }
}

public record Directory(string Name, Directory? Parent)
{
    public ISet<Directory> Directories { get; } = new HashSet<Directory>();
    public ISet<File> Files { get; } = new HashSet<File>();
}

public record File(string Name, int Size);

public class Command
{
}

public class RootDirectoryCommand : Command
{
}

public class UpDirectoryCommand : Command
{
}

public class RelativeChangeDirectoryCommand : Command
{
    public RelativeChangeDirectoryCommand(string directoryName)
    {
        DirectoryName = directoryName;
    }

    public string DirectoryName { get; }
}

public class ListDirectoryContentsCommand : Command
{
    public ListDirectoryContentsCommand(DirectorySummary[] directories, FileSummary[] files)
    {
        Directories = directories;
        Files = files;
    }

    public DirectorySummary[] Directories { get; }

    public FileSummary[] Files { get; }
}

public record FileSummary(string Name, int Size);

public record DirectorySummary(string Name);