//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace AdventOfCodeDay10
//{
//    public static class Part2Solver
//    {
//        public static int Solve(string[] lines)
//        {
//            int totalPresses = 0;

//            foreach (var line in lines)
//            {
//                var (goal, buttons) = ParseLine(line);
//                totalPresses += SolveMachine(goal, buttons);
//            }

//            return totalPresses;
//        }

//        // ------------------------------------------------------------
//        // Parse input: [.#..] (0,1) (1,2) {3,5}
//        // ------------------------------------------------------------
//        private static (int[] goal, List<int[]> buttons) ParseLine(string line)
//        {
//            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
//            int[] goal = parts[0].Trim('[', ']').Select(c => c == '#' ? 1 : 0).ToArray();

//            List<int[]> buttons = new();
//            foreach (var p in parts.Where(p => p.StartsWith("(")))
//            {
//                buttons.Add(p.Trim('(', ')').Split(',').Select(int.Parse).ToArray());
//            }

//            return (goal, buttons);
//        }

//        // ------------------------------------------------------------
//        // Solve a single machine using DFS on free variables
//        // ------------------------------------------------------------
//        private static int SolveMachine(int[] goal, List<int[]> buttons)
//        {
//            int n = goal.Length;
//            int m = buttons.Count;

//            // Build matrix A (n x m)
//            int[,] A = new int[n, m];
//            for (int j = 0; j < m; j++)
//                foreach (var idx in buttons[j])
//                    A[idx, j] = 1;

//            // Gaussian elimination to find independent / dependent columns
//            var (dependents, independents, matrix) = GaussianElim(A, goal);

//            // DFS over independent variables (free buttons)
//            int[] values = new int[independents.Count];
//            int minPresses = int.MaxValue;
//            int maxValue = goal.Max() + 1;

//            DFS(matrix, dependents, independents, values, 0, maxValue, ref minPresses);

//            return minPresses;
//        }

//        // ------------------------------------------------------------
//        // Gaussian elimination over integers (double for stability)
//        // Returns: dependent indices, independent indices, augmented matrix
//        // ------------------------------------------------------------
//        private static (List<int> dependents, List<int> independents, double[,] matrix)
//            GaussianElim(int[,] A, int[] b)
//        {
//            int n = A.GetLength(0);
//            int m = A.GetLength(1);
//            double[,] M = new double[n, m + 1];

//            for (int i = 0; i < n; i++)
//            {
//                for (int j = 0; j < m; j++)
//                    M[i, j] = A[i, j];
//                M[i, m] = b[i];
//            }

//            List<int> dependents = new();
//            List<int> independents = new();

//            int row = 0;
//            for (int col = 0; col < m && row < n; col++)
//            {
//                // Pivot
//                int pivot = row;
//                while (pivot < n && Math.Abs(M[pivot, col]) < 1e-9) pivot++;

//                if (pivot == n)
//                {
//                    independents.Add(col);
//                    continue;
//                }

//                // Swap rows
//                if (pivot != row)
//                    for (int k = 0; k <= m; k++)
//                        (M[row, k], M[pivot, k]) = (M[pivot, k], M[row, k]);

//                dependents.Add(col);

//                // Normalize pivot row
//                double div = M[row, col];
//                for (int k = col; k <= m; k++)
//                    M[row, k] /= div;

//                // Eliminate other rows
//                for (int r = 0; r < n; r++)
//                {
//                    if (r == row) continue;
//                    double factor = M[r, col];
//                    if (Math.Abs(factor) > 1e-9)
//                        for (int k = col; k <= m; k++)
//                            M[r, k] -= factor * M[row, k];
//                }

//                row++;
//            }

//            for (int c = row; c < m; c++)
//                independents.Add(c);

//            return (dependents, independents, M);
//        }

//        // ------------------------------------------------------------
//        // DFS over independent variables (free buttons)
//        // ------------------------------------------------------------
//        private static void DFS(double[,] matrix, List<int> dependents, List<int> independents,
//            int[] values, int idx, int maxValue, ref int minPresses)
//        {
//            if (idx == independents.Count)
//            {
//                if (CheckSolution(matrix, dependents, independents, values, out int total))
//                    minPresses = Math.Min(minPresses, total);
//                return;
//            }

//            int sumSoFar = values.Take(idx).Sum();
//            for (int val = 0; val < maxValue; val++)
//            {
//                if (sumSoFar + val >= minPresses) break; // prune
//                values[idx] = val;
//                DFS(matrix, dependents, independents, values, idx + 1, maxValue, ref minPresses);
//            }
//        }

//        // ------------------------------------------------------------
//        // Compute dependent variables and check non-negativity
//        // ------------------------------------------------------------
//        private static bool CheckSolution(double[,] matrix, List<int> dependents, List<int> independents,
//            int[] freeValues, out int total)
//        {
//            total = freeValues.Sum();
//            int n = matrix.GetLength(0);
//            int m = matrix.GetLength(1) - 1;

//            for (int i = 0; i < dependents.Count; i++)
//            {
//                double val = matrix[i, m];
//                for (int j = 0; j < independents.Count; j++)
//                    val -= matrix[i, independents[j]] * freeValues[j];

//                if (val < -1e-9 || Math.Abs(val - Math.Round(val)) > 1e-9)
//                    return false;

//                total += (int)Math.Round(val);
//            }

//            return true;
//        }
//    }
//}
