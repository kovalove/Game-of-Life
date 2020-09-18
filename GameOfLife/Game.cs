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

        /// <summary>
        /// Initialize new instance of the game of the given size.
        /// </summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="columns">Number of columns.</param>
        public Game(int rows, int columns)
        {
            Setup(rows, columns);
        }

        /// <summary>
        /// Initialize primary and auxiliary arrays of the given size.
        /// </summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="columns">Number of columns.</param>
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

        /// <summary>
        /// Read game state from the file.
        /// </summary>
        /// <param name="filename">Path to a file to read.</param>
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

        /// <summary>
        /// Save the game to a text file.
        /// </summary>
        /// <param name="filename">Path to a file where to write game state.</param>
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
