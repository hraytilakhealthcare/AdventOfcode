using System.Diagnostics;

namespace AdventOfcode;

public abstract class PuzzleSolver
{
    protected readonly string FileContent;
    private bool isComputed;
    private (string? text, TimeSpan duration) answer1;
    private (string? text, TimeSpan duration) answer2;

    protected PuzzleSolver(string fileContent)
    {
        FileContent = fileContent;
    }

    protected abstract string Step1(string fileContent);
    protected abstract string Step2(string fileContent);


    public void Compute()
    {
        answer1 = ComputeStep(Step1);
        answer2 = ComputeStep(Step2);
        isComputed = true;
    }

    private (string?, TimeSpan) ComputeStep(Func<string, string> step1)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        try
        {
            return (step1.Invoke(FileContent), stopwatch.Elapsed);
        }
        catch (Exception)
        {
            return ("Not implemented", stopwatch.Elapsed);
        }
    }

    public override string ToString()
    {
        if (isComputed)
            return $"Step1: {answer1.text} ({answer1.duration})\n" +
                   $"Step2: {answer2.text} ({answer2.duration})";
        return "Not computed yet";
    }
}
