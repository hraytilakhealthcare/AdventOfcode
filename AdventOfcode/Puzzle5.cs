using System.IO.Pipes;
using NUnit.Framework;

namespace AdventOfcode;

// ReSharper disable once UnusedType.Global
public class Puzzle5 : PuzzleSolver
{
    public Puzzle5(string fileContent) : base(fileContent)
    {
    }

    private static List<Stack<char>> ParseCrateStacks(string crates)
    {
        List<string> lines = crates.Split("\n").ToList();
        int stackCount = ComputeStackCount(lines);
        lines.RemoveAt(lines.Count - 1);

        List<Stack<char>> stacks = CreateStackList(stackCount);

        for (int lineIndex = lines.Count - 1; lineIndex >= 0; lineIndex--)
        {
            char[] lineChars = lines[lineIndex].ToCharArray();
            for (int stackIndex = 0; stackIndex < stackCount; stackIndex++)
            {
                int contentLocation = 1 + stackIndex * 4;
                if (contentLocation < lineChars.Length && lineChars[contentLocation] != ' ')
                    stacks[stackIndex].Push(lineChars[contentLocation]);
            }
        }

        return stacks;
    }

    private static List<Stack<char>> CreateStackList(int stackCount)
    {
        List<Stack<char>> stacks = new(stackCount);
        for (int i = 0; i < stackCount; i++)
            stacks.Add(new Stack<char>());
        return stacks;
    }

    private static int ComputeStackCount(IEnumerable<string> lines)
    {
        int stackCount = lines
            .Last()
            .Split(" ")
            .Where(item => !string.IsNullOrEmpty(item))
            .ToList()
            .Count;
        return stackCount;
    }
    //TODO: Index -1

    protected override string Step1(string fileContent)
    {
        string[] strings = fileContent.Split("\n\n");
        string crateDesc = strings[0];
        List<Stack<char>> stacks = ParseCrateStacks(crateDesc);
        string ordersString = strings[1];

        foreach (Tuple<int, int, int> orders in ordersString.Split("\n").Select(ParseOrder))
            ExecuteMove(orders.Item1, orders.Item2, orders.Item3, stacks);

        return new string(stacks.Select(s => s.Peek()).ToArray());
    }

    protected override string Step2(string fileContent)
    {
        string[] strings = fileContent.Split("\n\n");
        string crateDesc = strings[0];
        List<Stack<char>> stacks = ParseCrateStacks(crateDesc);
        string ordersString = strings[1];

        foreach (Tuple<int, int, int> orders in ordersString.Split("\n").Select(ParseOrder))
            ExecuteMove2(orders.Item1, orders.Item2, orders.Item3, stacks);

        return new string(stacks.Select(s => s.Peek()).ToArray());
    }

    //SO fucking lazy :)
    private static void ExecuteMove2(int crateCount, int from, int to, List<Stack<char>> stacks)
    {
        Stack<char> tmpStack = new Stack<char>();
        for (int i = 0; i < crateCount; i++)
            tmpStack.Push(stacks[from].Pop());
        for (int i = 0; i < crateCount; i++)
            stacks[to].Push(tmpStack.Pop());
    }


    private static void ExecuteMove(int crateCount, int from, int to, List<Stack<char>> stacks)
    {
        for (int i = 0; i < crateCount; i++)
            stacks[to].Push(stacks[from].Pop());
    }

    // count, from, to
    private static Tuple<int, int, int> ParseOrder(string orderString)
    {
        string[] strings = orderString.Split(" ");
        Assert.AreEqual(6, strings.Length);
        return new Tuple<int, int, int>(
            int.Parse(strings[1]),
            int.Parse(strings[3]) - 1, //-1 because we're indexing on 0
            int.Parse(strings[5]) - 1
        );
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
