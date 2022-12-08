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
        return ParseMap(fileContent).VisibleTreesCount().ToString();
    }

    protected override string Step2(string fileContent)
    {
        return ParseMap(fileContent).AllScenicScores().Max().ToString();
    }

    private static Map ParseMap(string fileContent)
    {
        Map map = new();
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

    private readonly struct Coords
    {
        public readonly int X;
        public readonly int Y;

        public Coords(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"x:{X}, y:{Y}";
    }

    private class Map
    {
        private readonly Dictionary<Coords, int> forest = new();
        private int maxX, maxY;

        public void AddTree(int treeHeight, int x, int y)
        {
            forest.Add(new Coords(x, y), treeHeight);
            maxX = Math.Max(maxX, x + 1);
            maxY = Math.Max(maxY, y + 1);
        }

        public int VisibleTreesCount()
        {
            return forest.Keys.Where(IsVisible).ToList().Count;
        }

        public IEnumerable<int> AllScenicScores() => forest.Keys.Select(ComputeScenicScore);

        public int ComputeScenicScore(Coords tree)
        {
            int westView = SeenTrees(tree, West);
            int northView = SeenTrees(tree, North);
            int eastView = SeenTrees(tree, East);
            int southView = SeenTrees(tree, South);
            return westView
                   * northView
                   * eastView
                   * southView;
        }

        private int SeenTrees(Coords tree, Func<Coords, IEnumerable<Coords>> direction)
        {
            int height = forest[tree];
            int treeSeen = 0;
            foreach (Coords coords in direction.Invoke(tree))
            {
                treeSeen++;
                if (forest[coords] >= height)
                    return treeSeen;
            }

            return treeSeen;
        }

        private bool IsVisible(Coords coords)
        {
            if (IsOnBorder(coords))
                return true;

            int height = forest[coords];
            bool visibleWest = West(coords).All(
                neighbor => height > forest[neighbor]
            );
            bool visibleEast = East(coords).All(
                neighbor => height > forest[neighbor]
            );
            bool visibleSouth = South(coords).All(
                neighbor => height > forest[neighbor]
            );
            bool visibleNorth = North(coords).All(
                neighbor => height > forest[neighbor]
            );

            bool isVisible = visibleWest
                             || visibleEast
                             || visibleSouth
                             || visibleNorth;
            return isVisible;
        }

        private static IEnumerable<Coords> West(Coords coords)
        {
            for (int i = coords.X - 1; i >= 0; i--)
                yield return new Coords(i, coords.Y);
        }

        private IEnumerable<Coords> East(Coords coords)
        {
            for (int x = coords.X + 1; x < maxX; x++)
                yield return new Coords(x, coords.Y);
        }

        private static IEnumerable<Coords> North(Coords coords)
        {
            for (int y = coords.Y - 1; y >= 0; y--)
                yield return new Coords(coords.X, y);
        }

        private IEnumerable<Coords> South(Coords coords)
        {
            for (int y = coords.Y + 1; y < maxY; y++)
                yield return new Coords(coords.X, y);
        }


        private bool IsOnBorder(Coords coords)
        {
            return coords.X == 0
                   || coords.Y == 0
                   || coords.X == maxX - 1
                   || coords.Y == maxY - 1;
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
            Assert.AreEqual(21, map.VisibleTreesCount());
            Assert.AreEqual(4, map.ComputeScenicScore(new Coords(2, 1)));
        }
    }
}
