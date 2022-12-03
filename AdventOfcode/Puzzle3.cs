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

    private static char MisplacedItem(string backpack)
    {
        char[] charArray = backpack.ToCharArray();
        HashSet<char> left = new(charArray.Take(backpack.Length / 2));
        HashSet<char> right = new(charArray.Skip(backpack.Length / 2).Take(backpack.Length / 2));
        left.IntersectWith(right);
        return new List<char>(left)[0];
    }

    protected override string Step2(string fileContent)
    {
        throw new Exception();
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
