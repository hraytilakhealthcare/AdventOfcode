using System.Diagnostics;
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
        Rope rope = new(startPos, 1);

        int tailMoves = new HashSet<Coords>(
            fileContent.Split("\n").Select(ParseMove).SelectMany(move => rope.Head.Walk(move.step, move.count))
        )
        {
            startPos
        }.Count;
        return tailMoves.ToString();
    }

    protected override string Step2(string fileContent)
    {
        Coords startPos = new(0, 0);
        Rope rope = new(startPos, 9);

        int tailMoves = new HashSet<Coords>(
            fileContent.Split("\n").Select(ParseMove).SelectMany(move => rope.Head.Walk(move.step, move.count))
        )
        {
            startPos
        }.Count;
        return tailMoves.ToString();
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

    private class Knot
    {
        private Coords coords;
        private Knot? child;
        private int X => coords.X;
        private int Y => coords.Y;

        public Knot(Coords start)
        {
            coords = start.Clone();
        }

        public void Bind(Knot boundChild) => child = boundChild;

        private bool IsChildAdjacent()
        {
            if (child == null)
                return true;
            return Math.Abs(X - child.X) <= 1
                   && Math.Abs(Y - child.Y) <= 1;
        }

        public IEnumerable<Coords> Walk(Coords step, int count) =>
            Enumerable.Repeat(step, count).Select(Move);

        private Coords Move(Coords step)
        {
            coords += step;
            return !IsChildAdjacent()
                ? Catchup()
                : TailMostCoords();
        }

        private Coords TailMostCoords() =>
            child?.TailMostCoords() ?? coords;

        //Moves tail one step towards head
        private Coords Catchup()
        {
            Debug.Assert(child != null, nameof(child) + " != null");
            Coords distance = coords - child.coords;
            Coords step = new(Math.Sign(distance.X), Math.Sign(distance.Y));
            return child.Move(step);
        }

        public override string ToString() => $"Knot:{coords}";
    }

    private class Rope
    {
        public readonly Knot Head;
        private readonly Knot tail;

        public Rope(Coords start, int knotCount)
        {
            Head = new Knot(start);
            Knot previous = Head;
            for (int i = 0; i < knotCount; i++)
            {
                Knot newKnot = new(start);
                previous.Bind(newKnot);
                previous = newKnot;
            }

            tail = previous;
        }

        public override string ToString() => $"H:{Head} T:{tail}";
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
        public static void PuzzleTest2()
        {
            const string sample = @"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20";
            Coords startPos = new(0, 0);
            Rope rope = new(startPos, 9);

            int tailMoves = new HashSet<Coords>(
                sample.Split("\n")
                    .Select(ParseMove)
                    .SelectMany(
                        move => rope.Head.Walk(move.step, move.count)
                    ).ToList()
            )
            {
                startPos
            }.Count;

            Assert.AreEqual(36, tailMoves);
        }

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
            Rope rope = new(startPos, 1);

            int tailMoves = new HashSet<Coords>(
                sample.Split("\n")
                    .Select(ParseMove)
                    .SelectMany(
                        move => rope.Head.Walk(move.step, move.count)
                    ).ToList()
            )
            {
                startPos
            }.Count;

            Assert.AreEqual(13, tailMoves);
        }
    }
}
