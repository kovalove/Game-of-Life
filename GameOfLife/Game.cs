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
        private int Generation = 1;

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
        /// Print game state to the provided output.
        /// </summary>
        /// <param name="output">Destination where to write the game state.</param>
        public void Print(TextWriter output)
        {
            int count = 0;

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    output.Write(Cells[r, c] ? "+" : " ");
                    if (Cells[r, c] == true)
                    {
                        count++;
                    }
                }

                output.WriteLine();
            }

            output.WriteLine();
            output.WriteLine("Count of live cells: {0}", count);
            output.WriteLine("Step: {0}", Generation);
            output.WriteLine();
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
                    // Do not count self
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

        public void Load(string filename)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                string[] size = reader.ReadLine().Split(" ");
                int rows = int.Parse(size[0]);
                int columns = int.Parse(size[1]);
                Generation = int.Parse(size[2]);
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
                writer.Write(' '); ;
                writer.Write(Generation);
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
