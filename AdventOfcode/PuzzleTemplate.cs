using NUnit.Framework;

namespace AdventOfcode;

//Usage:
// sed "s/DAY/7/" PuzzleTemplate.cs > Puzzle7.cs
// ReSharper disable once UnusedType.Global
public class PuzzleDAY : PuzzleSolver
{
    public PuzzleDAY(string fileContent) : base(fileContent)
    {
    }

    protected override string Step1(string fileContent)
    {
        throw new Exception();
    }

    protected override string Step2(string fileContent)
    {
        throw new Exception();
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
