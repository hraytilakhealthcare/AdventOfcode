using System.Xml.Schema;
using NUnit.Framework;

namespace AdventOfcode;

//Usage:
// sed "s/9/7/" PuzzleTemplate.cs > Puzzle7.cs
// ReSharper disable once UnusedType.Global
public class Puzzle9 : PuzzleSolver
{
    public Puzzle9(string fileContent) : base(fileContent)
    {
    }

    protected override string Step1(string fileContent)
    {
        Coords startPos = new(0, 0);
        Rope rope = new(startPos);

        int tailMoves = new HashSet<Coords>(
            fileContent.Split("\n").Select(ParseMove).SelectMany(move => rope.Walk(move.step, move.count))
        )
        {
            startPos
        }.Count;
        return tailMoves.ToString();
    }

    protected override string Step2(string fileContent)
    {
        throw new Exception();
    }

    private static (Coords step, int count) ParseMove(string line)
    {
        string[] split = line.Split(" ");
        Coords step = split[0] switch
        {
            "R" => new Coords(1, 0),
            "U" => new Coords(0, 1),
            "L" => new Coords(-1, 0),
            "D" => new Coords(0, -1),
            _ => throw new ArgumentOutOfRangeException()
        };
        int count = int.Parse(split[1]);
        return (step, count);
    }

    private class Rope
    {
        private Coords head;

        private Coords tail;
        // private readonly HashSet<Coords> visitedByTail = new();

        public Rope(Coords start)
        {
            head = start.Clone();
            tail = start.Clone();
        }

        public IEnumerable<Coords> Walk(Coords step, int count) =>
            Enumerable.Repeat(step, count).Select(MoveHead);

        private Coords MoveHead(Coords step)
        {
            head += step;
            if (!IsAdjacent())
                Catchup();
            return tail;
        }

        private bool IsAdjacent() =>
            Math.Abs(head.X - tail.X) <= 1
            && Math.Abs(head.Y - tail.Y) <= 1;

        //Moves tail one step towards head
        private void Catchup()
        {
            Coords distance = head - tail;
            Coords step = new(Math.Sign(distance.X), Math.Sign(distance.Y));
            tail += step;
        }

        public override string ToString() => $"H:{head} T:{tail}";
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

        public static Coords operator -(Coords a, Coords b) =>
            new(a.X - b.X, a.Y - b.Y);

        public static Coords operator +(Coords a, Coords b) =>
            new(a.X + b.X, a.Y + b.Y);

        public Coords Clone() => new(X, Y);
        public override string ToString() => $"({X},{Y})";
    }

    public static class Test
    {
        [Test]
        public static void PuzzleTest()
        {
            const string sample = @"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2";
            Coords startPos = new(0, 0);
            Rope rope = new(startPos);

            int tailMoves = new HashSet<Coords>(
                sample.Split("\n").Select(ParseMove).SelectMany(move => rope.Walk(move.step, move.count))
            )
            {
                startPos
            }.Count;

            Assert.AreEqual(13, tailMoves);
        }
    }
}
