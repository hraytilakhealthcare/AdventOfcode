using NUnit.Framework;

namespace AdventOfcode;

// ReSharper disable once UnusedType.Global
public class Puzzle2 : PuzzleSolver
{
    private enum Result
    {
        Win = 3,
        Loose = 1,
        Draw = 2
    }
    // 12740 -> Too high
    // 9808 -> Too low


    private static readonly Dictionary<Result, int> ResultScores = new()
    {
        { Result.Loose, 0 },
        { Result.Draw, 3 },
        { Result.Win, 6 }
    };

    private static readonly Dictionary<char, int> ShapeScores = new()
    {
        { 'X', 1 },
        { 'A', 1 },

        { 'Y', 2 },
        { 'B', 2 },

        { 'Z', 3 },
        { 'C', 3 }
    };

    private const int MaxVal = 3;

    public Puzzle2(string fileContent) : base(fileContent)
    {
    }

    protected override string Step1(string fileContent)
    {
        return fileContent.Split("\n").Select(
            line => ComputeScore(ParseLine(line))
        ).Sum().ToString();
    }

    protected override string Step2(string fileContent)
    {
        return fileContent.Split("\n").Select(
            line => ComputeScore2(ParseLine(line))
        ).Sum().ToString();
    }

    private static int ComputeScore((char, char) move)
    {
        int hisMove = ShapeScores[move.Item1];
        int myMove = ShapeScores[move.Item2];
        return ResultScores[ComputeResult(hisMove, myMove)] + myMove;
    }

    private static int ComputeScore2((char, char) move)
    {
        int hisMove = ShapeScores[move.Item1];
        Result myTargetResult = (Result)(ShapeScores[move.Item2]);
        return myTargetResult switch
        {
            Result.Win => ResultScores[Result.Win] + FindMove(Result.Win, hisMove),
            Result.Loose => ResultScores[Result.Loose] + FindMove(Result.Loose, hisMove),
            Result.Draw => ResultScores[Result.Draw] + hisMove,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    //Got lazy with %
    private static int FindMove(Result target, int hisMove)
    {
        if (target == Result.Win)
            return hisMove switch
            {
                1 => 2,
                2 => 3,
                3 => 1,
                _ => throw new Exception()
            };
        if (target == Result.Loose)
            return hisMove switch
            {
                1 => 3,
                2 => 1,
                3 => 2,
                _ => throw new Exception()
            };
        return hisMove;
    }

    private static Result ComputeResult(int hisMove, int myMove)
    {
        if (myMove == hisMove)
            return Result.Draw;

        if ((hisMove == 1 && myMove == 2) || (hisMove == 2 && myMove == 3) || (hisMove == 3 && myMove == 1))
            return Result.Win;

        return Result.Loose;
    }

    private static (char, char) ParseLine(string line)
    {
        char[] charArray = line.ToCharArray();
        return (charArray[0], charArray[2]);
    }

    public static class Test2
    {
        [Test]
        public static void ResultTest2()
        {
            const string guide = @"A Y
B X
C Z";
            int finalScore = guide.Split("\n").Select(
                line => ComputeScore2(ParseLine(line))
            ).Sum();
            Assert.AreEqual(finalScore, 12);
            Assert.AreEqual(4, ComputeScore2(('A', 'Y')));
            Assert.AreEqual(1, ComputeScore2(('B', 'X')));
            Assert.AreEqual(7, ComputeScore2(('C', 'Z')));
        }

        [Test]
        public static void ResultTest()
        {
            Assert.AreEqual(Result.Win, ComputeResult(ShapeScores['A'], ShapeScores['Y']));
            Assert.AreEqual(Result.Draw, ComputeResult(ShapeScores['A'], ShapeScores['A']));
            Assert.AreEqual(Result.Win, ComputeResult(ShapeScores['C'], ShapeScores['A']));
            Assert.AreEqual(Result.Loose, ComputeResult(ShapeScores['A'], ShapeScores['C']));
            Assert.AreEqual(Result.Win, ComputeResult(ShapeScores['A'], ShapeScores['B']));
            Assert.AreEqual(Result.Draw, ComputeResult(2, 2));
            Assert.AreEqual(Result.Win, ComputeResult(3, 1));
            Assert.AreEqual(Result.Loose, ComputeResult(3, 2));
            Assert.AreEqual(Result.Win, ComputeResult(2, 3));
        }


        [Test]
        public static void PuzzleTest()
        {
            Assert.AreEqual(8, ComputeScore(('A', 'Y')));
            Assert.AreEqual(1, ComputeScore(('B', 'X')));
            Assert.AreEqual(6, ComputeScore(('C', 'Z')));
            const string guide = @"A Y
B X
C Z";
            int finalScore = guide.Split("\n").Select(
                line => ComputeScore(ParseLine(line))
            ).Sum();
            Assert.AreEqual(finalScore, 15);
        }
    }
}
