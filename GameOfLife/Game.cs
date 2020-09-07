using System;
using System.IO;

namespace GameOfLife
{
    public class Game
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public bool[,] Cells { get; set; }
        private readonly Random random;

        public Game(int rows, int columns)
        {
            random = new Random();
            Setup(rows, columns);
        }

        private void Setup(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Cells = new bool[rows, columns];
        }


        /// <summary>
        /// Print the game to the console.
        /// </summary>
        public void Print()
        {
            Console.Clear();
            //Console.SetCursorPosition(1, 1);

            for (int rows = 0; rows < Rows; rows++)
            {
                for (int cells = 0; cells < Columns; cells++)
                {
                    Console.Write(Cells[rows, cells] ? "+" : " ");
                }
                Console.WriteLine();
            }
        }

        
        /// <summary>
        /// Count number of alive cells around the given position.
        /// </summary>
        private int CountNearby(int x, int y)
        {
            int count = 0;
            for (int rows = x - 1; rows < x + 2; rows++)
            {
                for (int cells = y - 1; cells < y + 2; cells++)
                {
                    // do not count self
                    if (rows == x && cells == y) continue;
                    if (IsAlive(rows, cells)) count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Chek if cell by the given position is alive.
        /// </summary>
        private bool IsAlive(int rows, int cells)
        {
            if (rows < 0 || cells < 0) return false;
            if (rows >= Rows || cells >= Columns) return false;
            return Cells[rows, cells];
        }

        /// <summary>
        /// Advance game to a next generation.
        /// </summary>
        public void Step()
        {
            for (int rows = 0; rows < Rows; rows++)
            {
                for (int cells = 0; cells < Columns; cells++)
                {
                    int nearby = CountNearby(rows, cells);
                    if (nearby < 2 || nearby > 3)
                    {
                        Cells[rows, cells] = false;
                    }
                    else if (nearby == 3)
                    {
                        Cells[rows, cells] = true;
                    }

                }
            }
        }

        /// <summary>
        /// Randomly generate dead and alive cells.
        /// </summary>
        public void Randomize()
        {
            for (int rows = 0; rows < Rows; rows++)
            {
                for (int cells = 0; cells < Columns; cells++)
                {
                    Cells[rows, cells] = random.Next(0, 2) == 1;
                }
            }
        }

        public void Load(string filename)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                string[] size = reader.ReadLine().Split(" ");
                int rows = int.Parse(size[0]);
                int columns = int.Parse(size[1]);
                Setup(rows, columns);

                for (int r = 0; r < rows; r++)
                {
                    string line = reader.ReadLine();
                    for (int c = 0; c < line.Length; c++)
                    {
                        bool alive = line[c] == '+';
                        Cells[r, c] = alive;
                    }
                }
            }

        }

        public void SaveGame()
        {
            
        }
    }
}
