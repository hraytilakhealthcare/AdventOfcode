using Mono.Options;
using NUnit.Framework;

namespace AdventOfcode
{
    internal static class AdventOfCodeRunner
    {
        public static void Main(string[] args)
        {
            int day = DateTime.Now.Day; //Lazy default to today, let's hope I don't trail behind this year
            OptionSet commandLineOptions = new()
            {
                { "d|day=", "The day to run the problem on", value => day = int.Parse(value) }
            };
            commandLineOptions.Parse(args);
            Utils.Log($"Running advent of code for day:{day}");

            string path = $"/home/ahub/src/private/AdventOfcode/input_{day}";
            Assert.IsTrue(File.Exists(path), $"File not found {path}");
            string fileContent = File.ReadAllText(path)
                .TrimEnd('\n'); //Remove the last \n that is usually outside the scope of the puzzle

            string typeName = $"AdventOfcode.Puzzle{day}";
            Type type = Type.GetType(typeName) ?? throw new ArgumentNullException($"Couldn't find type: {typeName}");
            PuzzleSolver puzzleSolver = Activator.CreateInstance(type, fileContent) as PuzzleSolver ??
                                        throw new InvalidOperationException($"Couldn't instantiate type: {typeName}");
            puzzleSolver.Compute();
            Utils.Log(puzzleSolver.ToString());
        }
    }
}
