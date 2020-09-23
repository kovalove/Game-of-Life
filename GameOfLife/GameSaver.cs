using System;
using System.Collections.Generic;
using System.IO;

namespace GameOfLife
{
    /// <summary>
    /// Game serialization / desirialization.
    /// </summary>
    class GameSaver
    {
        /// <summary>
        /// Read game state from the file.
        /// </summary>
        /// <param name="filename">Path to a file to read.</param>
        public Game Load(StreamReader reader)
        {
            try
            {
                string[] size = reader.ReadLine().Split(" ");
                int rows = int.Parse(size[0]);
                int columns = int.Parse(size[1]);
                int generation = int.Parse(size[2]);
                bool[,] cells = new bool[rows, columns];

                for (int r = 0; r < rows; r++)
                {
                    string line = reader.ReadLine();
                    for (int c = 0; c < line.Length; c++)
                    {
                        bool alive = line[c] == '+';
                        cells[r, c] = alive;
                    }
                }

                return new Game(cells, generation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Save the game to a text file.
        /// </summary>
        /// <param name="filename">Path to a file where to write game state.</param>
        public void SaveGame(Game game, StreamWriter writer)
        {
            {
                writer.Write(game.Rows);
                writer.Write(' ');
                writer.Write(game.Columns);
                writer.Write(' '); ;
                writer.Write(game.Generation);
                writer.WriteLine();

                for (int r = 0; r < game.Rows; r++)
                {
                    for (int c = 0; c < game.Columns; c++)
                    {
                        writer.Write(game.IsAlive(r, c) ? "+" : " ");
                    }
                    writer.WriteLine();
                }
            }
        }

        public void SaveGames(List<Game> games, string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine(games.Count);

                foreach (Game game in games)
                {
                    SaveGame(game, writer);
                }
            }
        }

        public List<Game> LoadGames(string filename)
        {
            List<Game> games = new List<Game>();

            using (StreamReader reader = new StreamReader(filename))
            {
                string line = reader.ReadLine();
                int count = int.Parse(line);

                for (int i = 1; i <= count; i++)
                {
                    Game game = Load(reader);
                    games.Add(game);
                }
            }

            return games;
        }
    }
}
