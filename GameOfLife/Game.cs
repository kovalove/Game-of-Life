using System;

namespace GameOfLife
{
    /// <summary>
    /// Model of the game including basic logic operations.
    /// </summary>
    public class Game
    {
        private bool[,] Cells;
        private bool[,] Buffer;

        /// <summary>
        /// Height in the the game field.
        /// </summary>
        public int Rows { get; private set; }

        /// <summary>
        /// Width in the game field.
        /// </summary>
        public int Columns { get; private set; }

        /// <summary>
        /// For how long the game is running.
        /// </summary>
        public int Generation { get; private set; } = 1;

        /// <summary>
        /// True if game is changing the state when new generation and false otherwise.
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        /// Count of alive cells in the game field.
        /// </summary>
        public int CountAlive { get; private set; }

        /// <summary>
        /// Initialize new game of the given size.
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

        /// <summary>
        /// Initialize game from the provided state.
        /// Detects number of rows and columns from the dimensions of provided array.
        /// Use this method for loading the game from external source.
        /// </summary>
        /// <param name="cells">Initial game state to use.</param>
        /// <param name="generation">Generation of the provided initial state.</param>
        public Game(bool[,] cells, int generation)
        {
            Cells = cells;
            Rows = cells.GetLength(0);
            Columns = cells.GetLength(1);
            Buffer = new bool[Rows, Columns];
            Generation = generation;

            foreach (bool alive in Cells)
            {
                if (alive)
                {
                    CountAlive++;
                }
            }
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
        /// <param name="x">X coordinate position.</param>
        /// <param name="y">Y coordinate position.</param>
        /// <returns>True if cell is alive; false otherwise.</returns>
        public bool IsAlive(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return false;
            }
            if (x >= Rows || y >= Columns)
            {
                return false;
            }

            return Cells[x, y];
        }

        /// <summary>
        /// Advance game to a next generation.
        /// </summary>
        public void Step()
        {
            Active = false;
            Generation++;
            CountAlive = 0;

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

                    if (Buffer[r, c])
                    {
                        CountAlive++;
                    }

                    if (Buffer[r, c] != Cells[r, c])
                    {
                        Active = true;
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
        public void Randomize(Random random)
        {
            CountAlive = 0;

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    Cells[r, c] = random.Next(0, 2) == 1;
                    if (Cells[r, c])
                    {
                        CountAlive++;
                    }
                }
            }
        }

        /// <summary>
        /// Convert to serializeable data.
        /// </summary>
        /// <returns>Game serializeable data.</returns>
        public GameInfo AsGameInfo()
        {
            return new GameInfo()
            {
                Cells = Cells,
                Generation = Generation,
            };
        }
    }
}
