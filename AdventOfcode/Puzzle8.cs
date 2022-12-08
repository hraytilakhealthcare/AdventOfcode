using NUnit.Framework;

namespace AdventOfcode;

//Usage:
// sed "s/8/7/" PuzzleTemplate.cs > Puzzle7.cs
// ReSharper disable once UnusedType.Global
public class Puzzle8 : PuzzleSolver
{
    public Puzzle8(string fileContent) : base(fileContent)
    {
    }

    protected override string Step1(string fileContent)
    {
        Map map = ParseMap(fileContent);
        return string.Empty;
    }

    private static Map ParseMap(string fileContent)
    {
        Map map = new Map();
        foreach ((string content, int index) treeLine in fileContent.Split("\n")
                     .Select((line, lIndex) => (line, lIndex)))
        {
            foreach ((int height, int charIndex) tree in treeLine.content.Select(
                         (character, cIndex) => (int.Parse(character.ToString()), cIndex)
                     ))
            {
                map.AddTree(tree.height, tree.charIndex, treeLine.index);
            }
        }

        return map;
    }

    protected override string Step2(string fileContent)
    {
        throw new Exception();
    }

    private struct Coords
    {
        public readonly int x;
        public readonly int y;

        public Coords(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    private class Map
    {
        private readonly Dictionary<Coords, int> trees = new();
        private int maxX, maxY;

        public void AddTree(int treeHeight, int x, int y)
        {
            trees.Add(new Coords(x, y), treeHeight);
            maxX = Math.Max(maxX, x + 1);
            maxY = Math.Max(maxY, y + 1);
        }

        public int VisibleTrees()
        {
            return GenerateTreeLine(0, maxX).Select(CountVisibleTree).Sum()
                   + GenerateTreeLine(maxX - 1, -1).Select(CountVisibleTree).Sum()
                   + GenerateTreeColumn(0, maxY).Select(CountVisibleTree).Sum()
                   + GenerateTreeColumn(maxY - 1, -1).Select(CountVisibleTree).Sum();
        }

        private int CountVisibleTree(IEnumerable<Coords> treeLine)
        {
            int visibleTreeCount = 0;
            int lastTree = -1;
            foreach (int currentTree in treeLine.ToList().Select(coords => trees[coords]))
            {
                if (currentTree > lastTree)
                {
                    visibleTreeCount++;
                }

                lastTree = currentTree;
            }

            return visibleTreeCount;
        }

        private IEnumerable<IEnumerable<Coords>> GenerateTreeLine(int startX, int endX)
        {
            for (int y = 0; y < maxY; y++)
            {
                List<Coords> coordsList = new();
                foreach (int x in Range(startX, endX))
                    coordsList.Add(new Coords(x, y));
                yield return coordsList;
            }
        }

        private IEnumerable<IEnumerable<Coords>> GenerateTreeColumn(int startY, int endY)
        {
            for (int x = 0; x < maxX; x++)
            {
                List<Coords> coordsList = new();
                foreach (int y in Range(startY, endY))
                    coordsList.Add(new Coords(x, y));
                yield return coordsList;
            }
        }

        public static IEnumerable<int> Range(int from, int to)
        {
            int increment = from < to ? 1 : -1;
            for (int i = from; i != to; i += increment)
                yield return i;
        }
    }

    public static class Test
    {
        [Test]
        public static void PuzzleTest()
        {
            Assert.AreEqual(new Coords(0, 2), new Coords(0, 2));
            const string sample = @"30373
25512
65332
33549
35390";
            Map map = ParseMap(sample);
            Assert.AreEqual(21, map.VisibleTrees());
        }
    }
}
