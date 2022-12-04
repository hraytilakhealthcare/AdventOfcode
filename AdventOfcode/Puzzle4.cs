using NUnit.Framework;

namespace AdventOfcode;

// ReSharper disable once UnusedType.Global
public class Puzzle4 : PuzzleSolver
{
    public Puzzle4(string fileContent) : base(fileContent)
    {
    }

    protected override string Step1(string fileContent)
    {
        return fileContent.Split("\n")
            .Select(ParseRanges)
            .ToList()
            .FindAll(Contains)
            .ToList()
            .Count
            .ToString();
    }

    protected override string Step2(string fileContent)
    {
        return fileContent.Split("\n")
            .Select(ParseRanges)
            .ToList()
            .FindAll(Overlaps)
            .ToList()
            .Count
            .ToString();
    }

    private static bool Overlaps(Tuple<Range, Range> obj)
    {
        return obj.Item1.Overlaps(obj.Item2);
    }


    private static bool Contains(Tuple<Range, Range> pairRanges)
    {
        return pairRanges.Item1.Contains(pairRanges.Item2)
               || pairRanges.Item2.Contains(pairRanges.Item1);
    }

    private static Tuple<Range, Range> ParseRanges(string line)
    {
        string[] strings = line.Split(",");
        Assert.AreEqual(2, strings.Length);
        return new Tuple<Range, Range>(
            Range.Parse(strings[0]),
            Range.Parse(strings[1])
        );
    }

    private struct Range
    {
        public readonly int Min;
        public readonly int Max;

        public Range(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public static Range Parse(string repr)
        {
            string[] strings = repr.Split("-");
            Assert.AreEqual(2, strings.Length);
            return new Range(int.Parse(strings[0]), int.Parse(strings[1]));
        }

        public bool Contains(Range other)
        {
            return other.Min >= Min && other.Max <= Max;
        }

        public bool Overlaps(Range other)
        {
            return other.Min <= Max && other.Max >= Min;
        }
    }


    public static class Test4
    {
        [Test]
        public static void PuzzleTest()
        {
        }
    }
}
