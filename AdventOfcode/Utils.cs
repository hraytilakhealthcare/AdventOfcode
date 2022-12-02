namespace AdventOfcode;

public static class Utils
{
    public static T Pop<T>(ICollection<T> list, T element)
    {
        //TODO: assert lib
        list.Remove(element);
        return element;
    }

    public static void Log(int val) => Log(val.ToString());
    public static void Log(string text) => Console.WriteLine(text);
}
