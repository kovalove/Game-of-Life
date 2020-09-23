using System;

namespace GameOfLife
{
    /// <summary>
    /// Model of the game including basic logic operations.
    /// </summary>
    public class Game
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        private bool[,] Cells;
        private bool[,] Buffer;
        public int Generation { get; private set; } = 1;

        /// <summary>
        /// Initialize primary and auxiliary arrays of the given size.
        /// </summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="columns">Number of columns.</param>
        public Game(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Cells = new bool[rows, columns];
            Buffer = new bool[rows, columns];
        }

        public Game(bool[,] cells, int generation)
        {
            Cells = cells;
            Rows = cells.GetLength(0);
            Columns = cells.GetLength(1);
            Buffer = new bool[Rows, Columns];
            Generation = generation;
        }

        /// <summary>
        /// Count number of alive cells around the given position.
        /// </summary>
        /// <param name="x">X coordinate of the cell.</param>
        /// <param name="y">Y coordinate of the cell.</param>
        /// <returns>The number of alive cells around the given position.</returns>
        private int CountNearby(int x, int y)
        {
            int count = 0;
            for (int r = x - 1; r <= x + 1; r++)
            {
                for (int c = y - 1; c <= y + 1; c++)
                {
                    // Do not count self
                    if (r == x && c == y)
                    {
                        continue;
                    }
                    if (IsAlive(r, c))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Check if cell by the given position is alive.
        /// </summary>
        /// <param name="rows">X coordinate position.</param>
        /// <param name="columns">Y coordinate position.</param>
        /// <returns>True if cell is alive; false otherwise.</returns>
        public bool IsAlive(int rows, int columns)
        {
            if (rows < 0 || columns < 0)
            {
                return false;
            }
            if (rows >= Rows || columns >= Columns)
            {
                return false;
            }

            return Cells[rows, columns];
        }

        /// <summary>
        /// Advance game to a next generation.
        /// </summary>
        public void Step()
        {
            Generation++;
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    int nearby = CountNearby(r, c);
                    if (nearby < 2 || nearby > 3)
                    {
                        Buffer[r, c] = false;
                    }
                    else if (nearby == 3)
                    {
                        Buffer[r, c] = true;
                    } 
                    else
                    {
                        Buffer[r, c] = Cells[r, c];
                    }
                }
            }

            bool[,] temp = Cells;
            Cells = Buffer;
            Buffer = temp;
        }

        /// <summary>
        /// Randomly generate dead and alive cells.
        /// </summary>
        public void Randomize()
        {
            Random random = new Random();

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    Cells[r, c] = random.Next(0, 2) == 1;
                }
            }
        }
    }
}
