using System;
using System.Collections.Generic;

namespace OptiSolve
{
	public static class SimplexSolver
	{
		public static string SolveMaxLP(double[,] tableau)
		{
			int rows = tableau.GetLength(0);
			int cols = tableau.GetLength(1);
			List<string> log = new List<string>();

			while (true)
			{
				// Step 1: Find pivot column (most negative value in bottom row)
				int pivotCol = -1;
				double minVal = 0;

				for (int j = 0; j < cols - 1; j++)
				{
					if (tableau[rows - 1, j] < minVal)
					{
						minVal = tableau[rows - 1, j];
						pivotCol = j;
					}
				}

				if (pivotCol == -1) break; // Optimal reached

				// Step 2: Find pivot row (min ratio test)
				int pivotRow = -1;
				double minRatio = double.MaxValue;

				for (int i = 0; i < rows - 1; i++)
				{
					double aij = tableau[i, pivotCol];
					double bi = tableau[i, cols - 1];
					if (aij > 0)
					{
						double ratio = bi / aij;
						if (ratio < minRatio)
						{
							minRatio = ratio;
							pivotRow = i;
						}
					}
				}

				if (pivotRow == -1)
					return "Unbounded solution.";

				// Step 3: Pivoting
				double pivot = tableau[pivotRow, pivotCol];
				for (int j = 0; j < cols; j++)
					tableau[pivotRow, j] /= pivot;

				for (int i = 0; i < rows; i++)
				{
					if (i != pivotRow)
					{
						double factor = tableau[i, pivotCol];
						for (int j = 0; j < cols; j++)
							tableau[i, j] -= factor * tableau[pivotRow, j];
					}
				}

				log.Add(DumpTableau(tableau));
			}

			log.Add("Optimal solution found.");
			return string.Join("\n\n", log);
		}

		public static string DumpTableau(double[,] tableau)
		{
			int rows = tableau.GetLength(0);
			int cols = tableau.GetLength(1);
			string output = "Tableau:\n";

			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
					output += tableau[i, j].ToString("0.00").PadLeft(8);
				output += "\n";
			}

			return output;
		}
	}
}
