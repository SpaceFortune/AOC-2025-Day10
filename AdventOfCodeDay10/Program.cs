using AdventOfCodeDay10;
class Program
{
    static void Main()
    {
        // Input einmal einlesen
        string[] lines = File.ReadAllLines("input10.txt");

        Console.WriteLine("=== Part 1 ===");
        var result1 = Part1Solver.Solve(lines);
        Console.WriteLine(result1);

        //Console.WriteLine("=== Part 2 ===");
        //var result2 = Part2Solver.Solve(lines);
        //Console.WriteLine(result2);
    }
}
