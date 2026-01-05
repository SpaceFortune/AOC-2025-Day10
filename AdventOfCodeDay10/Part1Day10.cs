namespace AdventOfCodeDay10
{
    public static class Part1Solver
    {
        public static int Solve(string[] lines)
        {
            int result = 0;

            static (long goal, List<long> buttons) ParseLine(string line)
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                // --- Ziel ---
                string pattern = parts[0].Trim('[', ']');
                long goal = 0;
                for (int i = 0; i < pattern.Length; i++)
                {
                    if (pattern[i] == '#')
                        goal |= (1L << i);
                }

                // --- Buttons ---
                List<long> buttons = new();
                foreach (var p in parts)
                {
                    if (!p.StartsWith("(")) continue;

                    string inner = p.Trim('(', ')');
                    var indices = inner.Split(',');

                    long mask = 0;
                    foreach (var idx in indices)
                        mask |= (1L << int.Parse(idx));

                    buttons.Add(mask);
                }

                return (goal, buttons);
            }

            static long MinPresses(long goal, List<long> buttons)
            {
                var queue = new Queue<long>();
                var distance = new Dictionary<long, int>();

                queue.Enqueue(0);
                distance[0] = 0;

                while (queue.Count > 0)
                {
                    long state = queue.Dequeue();
                    int steps = distance[state];

                    if (state == goal)
                        return steps;

                    foreach (var button in buttons)
                    {
                        long next = state ^ button;
                        if (!distance.ContainsKey(next))
                        {
                            distance[next] = steps + 1;
                            queue.Enqueue(next);
                        }
                    }
                }

                return long.MaxValue;
            }

            foreach (var line in lines)
            {
                var (goal, buttons) = ParseLine(line);
                result += (int)MinPresses(goal, buttons);
            }

            return result;
        }
    }
}

