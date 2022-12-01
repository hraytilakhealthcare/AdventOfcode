// See https://aka.ms/new-console-template for more information

using Mono.Options;


internal static class AdventOfCodeRunner
{
    private static int Day; //What day should we run ?
    private static bool Extra; //Is it the extra problem of the day ?

    private static void Main(string[] args)
    {
        OptionSet commandLineOptions = new()
        {
            { "d|day=", "The day to run the problem on", value => Day = int.Parse(value) },
            { "extra", "True if this is the second problem of the day", _ => Extra = true }
        };
        commandLineOptions.Parse(args);
        Log($"Running advent of code for day:{Day} extraPuzzle:{Extra}");
        string fileContent = File.ReadAllText($"/home/ahub/src/private/AdventOfcode/input_{Day}")
            .TrimEnd('\n'); //Remove the last \n that is usually outside the scope of the puzzle

        //TODO: Dynamic dispatch for the adequate function
        Puzzle1(fileContent);
    }

    private static void Puzzle1(string fileContent)
    {
        Log(fileContent.Split("\n\n").Select(Calories).Max().ToString());
    }

    private static int Calories(string elfInventory) => elfInventory.Split("\n").Select(i => int.Parse(i)).Sum();

    private static void Log(string text) => Console.WriteLine(text);
}
