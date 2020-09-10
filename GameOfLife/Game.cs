using System;
using System.IO;

namespace GameOfLife
{
    public class Game
    {
        private int Rows;
        private int Columns;
        private bool[,] Cells;
        private bool[,] Buffer;
        private int CountStep = 1;

        public Game(int rows, int columns)
        {
            Setup(rows, columns);
        }

        private void Setup(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Cells = new bool[rows, columns];
            Buffer = new bool[rows, columns];
        }

        /// <summary>
        /// Print the game to the console.
        /// </summary>
        public void Print()
        {
            Console.Clear();
            //Console.SetCursorPosition(1, 1);
            int count = 0;

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    Console.Write(Cells[r, c] ? "+" : " ");
                    if (Cells[r, c] == true)
                    {
                        count++;
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("Count of live cells: {0}", count);
            Console.WriteLine("Step: {0}", CountStep);
            Console.WriteLine();
        }

        /// <summary>
        /// Count number of alive cells around the given position.
        /// </summary>
        private int CountNearby(int x, int y)
        {
            int count = 0;
            for (int r = x - 1; r <= x + 1; r++)
            {
                for (int c = y - 1; c <= y + 1; c++)
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
        private bool IsAlive(int rows, int columns)
        {
            if (rows < 0 || columns < 0) return false;
            if (rows >= Rows || columns >= Columns) return false;
            return Cells[rows, columns];
        }

        /// <summary>
        /// Advance game to a next generation.
        /// </summary>
        public void Step()
        {
            CountStep++;
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    Buffer[r, c] = Cells[r, c];
                    int nearby = CountNearby(r, c);
                    if (nearby < 2 || nearby > 3)
                    {
                        Buffer[r, c] = false;
                    }
                    else if (nearby == 3)
                    {
                        Buffer[r, c] = true;
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

        public void SaveGame(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.Write(Rows);
                writer.Write(' ');
                writer.Write(Columns);
                writer.WriteLine();

                for (int r = 0; r < Rows; r++)
                {
                    for (int c = 0; c < Columns; c++)
                    {
                        writer.Write(Cells[r, c] ? "+" : " ");
                    }
                    writer.WriteLine();
                }
            }
        }
    }
}
