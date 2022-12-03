using NUnit.Framework;

namespace AdventOfcode;

// ReSharper disable once UnusedType.Global
public class Puzzle3 : PuzzleSolver
{
    public Puzzle3(string fileContent) : base(fileContent)
    {
    }

    protected override string Step1(string fileContent)
    {
        return fileContent.Split("\n")
            .Select(
                backpack => LetterValue(MisplacedItem(backpack))
            )
            .Sum()
            .ToString();
    }

    protected override string Step2(string fileContent)
    {
        string[] lines = fileContent.Split();
        Assert.IsTrue(lines.Length % 3 == 0);
        return AllTriplets(lines).Select(triplet => LetterValue(FindCommonLetter(triplet))).Sum().ToString();
    }

    private static char FindCommonLetter(Tuple<string, string, string> triplet)
    {
        HashSet<char> h1 = new(triplet.Item1.ToCharArray());
        HashSet<char> h2 = new(triplet.Item2.ToCharArray());
        HashSet<char> h3 = new(triplet.Item3.ToCharArray());
        h1.IntersectWith(h2);
        h1.IntersectWith(h3);
        return new List<char>(h1)[0];
    }

    private static IEnumerable<Tuple<string, string, string>> AllTriplets(IReadOnlyList<string> lines)
    {
        for (int i = 0; i < lines.Count; i += 3)
            yield return new Tuple<string, string, string>(lines[i + 0], lines[i + 1], lines[i + 2]);
    }

    private static char MisplacedItem(string backpack)
    {
        char[] charArray = backpack.ToCharArray();
        HashSet<char> left = new(charArray.Take(backpack.Length / 2));
        HashSet<char> right = new(charArray.Skip(backpack.Length / 2).Take(backpack.Length / 2));
        left.IntersectWith(right);
        return new List<char>(left)[0];
    }

    private static int LetterValue(char letter)
    {
        int asciiValue = letter;
        if (asciiValue >= 97)
            return asciiValue - 96;
        return asciiValue - 64 + 26;
    }


    public static class Test3
    {
        [Test]
        public static void ValueTest()
        {
            Assert.AreEqual(1, LetterValue('a'));
            Assert.AreEqual(27, LetterValue('A'));
        }

        [Test]
        public static void PuzzleTest()
        {
            const string input = @"vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw";
            int sum = input.Split("\n")
                .Select(
                    backpack => LetterValue(MisplacedItem(backpack))
                )
                .Sum();
            Assert.AreEqual(157, sum);
        }
    }
}
