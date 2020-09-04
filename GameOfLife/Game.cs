using System;

namespace GameOfLife
{
    public class Game
    {
        private readonly int rows;
        private readonly int columns;
        private readonly bool[,] cells;
        private readonly Random random;

        public Game(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            cells = new bool[rows, columns];
            random = new Random();
        }


        /// <summary>
        /// Print the game to the console.
        /// </summary>
        public void Print()
        {
            Console.Clear();
            //Console.SetCursorPosition(1, 1);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    Console.Write(cells[r, c] ? " " : "+");
                }
                Console.WriteLine();
            }
        }

        
        /// <summary>
        /// Count number of alive cells around the given position.
        /// </summary>
        public int CountNearby(int x, int y)
        {
            int count = 0;
            for (int r = x - 1; r < x + 2; r++)
            {
                for (int c = y - 1; c < y + 2; c++)
                {
                    // do not count self
                    if (r == x && c == y) continue;
                    if (IsAlive(r, c)) count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Chek if cell by the given position is alive.
        /// </summary>
        public bool IsAlive(int r, int c)
        {
            if (r < 0 || c < 0) return false;
            if (r >= rows || c >= columns) return false;
            return cells[r, c];
        }

        /// <summary>
        /// Advance game to a next generation.
        /// </summary>
        public void Step()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    int nearby = CountNearby(r, c);
                    if (nearby < 2 || nearby > 3)
                    {
                        cells[r, c] = false;
                    }
                    else if (nearby == 3)
                    {
                        cells[r, c] = true;
                    }

                }
            }
        }

        /// <summary>
        /// Randomly generate dead and alive cells.
        /// </summary>
        public void Randomize()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    cells[r, c] = random.Next(0, 2) == 1;
                }
            }
        }

        public void Load()
        {
        
        }
    }
}
