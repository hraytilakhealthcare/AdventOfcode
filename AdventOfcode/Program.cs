using AdventOfcode;
using Mono.Options;


internal static class AdventOfCodeRunner
{
    private static int Day; //What day should we run ?

    private static void Main(string[] args)
    {
        OptionSet commandLineOptions = new()
        {
            { "d|day=", "The day to run the problem on", value => Day = int.Parse(value) }
        };
        commandLineOptions.Parse(args);
        Utils.Log($"Running advent of code for day:{Day}");

        string fileContent = File.ReadAllText($"/home/ahub/src/private/AdventOfcode/input_{Day}")
            .TrimEnd('\n'); //Remove the last \n that is usually outside the scope of the puzzle

        //TODO: asserts
        Type? type = Type.GetType($"AdventOfcode.Puzzle{Day}");
        PuzzleSolver? puzzleSolver = Activator.CreateInstance(type, fileContent) as PuzzleSolver;
        puzzleSolver.Compute();
        Utils.Log(puzzleSolver.ToString());
    }
}
