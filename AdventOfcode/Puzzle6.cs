using NUnit.Framework;

namespace AdventOfcode;

// ReSharper disable once UnusedType.Global
public class Puzzle6 : PuzzleSolver
{
    private int signalLength;

    public Puzzle6(string fileContent) : base(fileContent)
    {
    }

    protected override string Step1(string fileContent)
    {
        return FindStartOfPacketIndex(fileContent, 4).ToString();
    }

    private static int FindStartOfPacketIndex(string signal, int markerLength)
    {
        HashSet<char> uniqueChars = new();
        for (int i = 0; i < signal.Length; i++)
        {
            string analyze = signal.Substring(i, markerLength);
            uniqueChars = new HashSet<char>(analyze);
            if (analyze.Length == new List<char>(uniqueChars).Count)
                return i + markerLength;
        }

        throw new Exception("That shouldn't happen with aoc input");
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
            Assert.AreEqual(5, FindStartOfPacketIndex("bvwbjplbgvbhsrlpgdmjqwftvncz", 4));
        }
    }
}
