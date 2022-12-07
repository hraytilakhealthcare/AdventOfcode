using NUnit.Framework;

namespace AdventOfcode;

//Usage:
// sed "s/7/7/" PuzzleTemplate.cs > Puzzle7.cs
// ReSharper disable once UnusedType.Global
public class Puzzle7 : PuzzleSolver
{
    public Puzzle7(string fileContent) : base(fileContent)
    {
    }

    protected override string Step1(string fileContent)
    {
        Dir root = BuildTree(fileContent);
        // return root.AllDirs().Select(dir => dir.GetSize()).Sum().ToString();
        return root.AllDirs().Where(dir => dir.GetSize() < 100000).Select(dir => dir.GetSize()).Sum().ToString();
    }

    protected override string Step2(string fileContent)
    {
        return "-1";
        throw new Exception();
    }

    private static Dir BuildTree(string fileContent)
    {
        Dir root = new("/", null);
        Dir current = root;
        foreach (string line in fileContent.Split("\n"))
        {
            if (line.StartsWith("$ cd"))
            {
                current = ChangeDir(line.Split(" ")[2], current);
                continue;
            }

            if (line == "$ ls")
                continue;

            BuildNode(line.Split(" "), current);
        }

        return root;
    }

    private static void BuildNode(string[] split, Dir current)
    {
        string size = split[0];
        string name = split[1];
        if (size == "dir")
            current.SubDirs[name] = new Dir(name, current);
        else
            current.Files[name] = new File(name, size);
    }

    private static Dir ChangeDir(string targetDirName, Dir current)
    {
        if (targetDirName == "..")
            return current.Parent;
        if (targetDirName == "/")
            return current.Root();
        if (!current.SubDirs.ContainsKey(targetDirName))
        {
            current.SubDirs[targetDirName] = new Dir(targetDirName, current);
        }

        return current.SubDirs[targetDirName];
    }


    private class Dir
    {
        public readonly string Name;
        public readonly Dir Parent;
        public readonly Dictionary<string, Dir> SubDirs = new();
        public readonly Dictionary<string, File> Files = new();

        public Dir(string name, Dir parent)
        {
            Name = name;
            Parent = parent;
        }

        public int GetSize()
        {
            return AllDirs().Select(d => d.GetSize()).Sum()
                   + Files.Values.Select(f => f.size).Sum();
        }

        public IEnumerable<Dir> AllDirs()
        {
            List<Dir> dirs = new();
            foreach (Dir subDir in SubDirs.Values)
            {
                dirs.Add(subDir);
                dirs.AddRange(subDir.AllDirs());
            }

            return dirs;
        }

        public Dir Root()
        {
            if (Parent == null)
                return this;
            return Parent.Root();
        }
    }

    private struct File
    {
        public string name;
        public int size;

        public File(string name, string size)
        {
            this.name = name;
            this.size = int.Parse(size);
        }
    }

    public static class Test
    {
        [Test]
        public static void PuzzleTest()
        {
            Assert.AreEqual(3, int.Parse("3"));
        }
    }
}
