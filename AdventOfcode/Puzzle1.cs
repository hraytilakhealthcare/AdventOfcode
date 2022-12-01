using NUnit.Framework;

namespace AdventOfcode;

// ReSharper disable once UnusedType.Global
public class Puzzle1 : PuzzleSolver
{
    private readonly List<int> elfs;

    public Puzzle1(string fileContent) : base(fileContent)
    {
        elfs = fileContent.Split("\n\n").Select(Calories).ToList();
    }

    protected override string Step1(string fileContent)
    {
        return elfs.Max().ToString();
    }

    protected override string Step2(string fileContent)
    {
        return elfs.Select( //Not ideal because of the elf enumeration but ¯\_(ツ)_/¯
                _ => Utils.Pop(elfs, elfs.Max())
            )
            .Take(3)
            .Sum()
            .ToString();
    }

    private static int Calories(string elfInventory) => elfInventory.Split("\n").Select(int.Parse).Sum();

    public static class Test
    {
        [Test]
        public static void PuzzleTest()
        {
            Assert.AreEqual(3, int.Parse("3"));
        }
    }
}
