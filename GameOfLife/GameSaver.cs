using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameOfLife
{
    /// <summary>
    /// Game serialization / desirialization.
    /// </summary>
    class GameSaver
    {
        const char ALIVE_CELL = '+';
        const char DEAD_CELL = ' ';


        private string filename;

        /// <summary>
        /// Initialize GameSaver.
        /// </summary>
        /// <param name="filename">Path to a file where to write or read json contents.</param>
        public GameSaver(string filename)
        {
            this.filename = filename;
        }

        /// <summary>
        /// Load game from the text file.
        /// </summary>
        /// <param name="reader">Stream to read game from.</param>
        /// <returns>Game loaded from the file block.</returns>
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
                        bool alive = line[c] == ALIVE_CELL;
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
        /// Save game to a text file.
        /// </summary>
        /// <param name="game">Game to save.</param>
        /// <param name="writer">Stream where to write provided game.</param>
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
                        writer.Write(game.IsAlive(r, c) ? ALIVE_CELL : DEAD_CELL);
                    }
                    writer.WriteLine();
                }
            }
        }

        /// <summary>
        /// Save multiple games to a text file.
        /// </summary>
        /// <param name="games">List of games to save.</param>
        /// <param name="filename">Path to a file where to write text contents.</param>
        public void SaveGames(List<Game> games)
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

        /// <summary>
        /// Load multiple games from the text file.
        /// </summary>
        /// <param name="filename">Path to a text file from which to read games.</param>
        /// <returns>List of games loaded from the file.</returns>
        public List<Game> LoadGames()
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

        /// <summary>
        /// Load multiple games from the json file.
        /// </summary>
        /// <returns>List of games loaded from the file.</returns>
        public List<Game> LoadGamesJson()
        {
            try
            {
                string json = File.ReadAllText(filename);
                var data = JsonConvert.DeserializeObject<List<GameInfo>>(json);
                return data.Select(info => new Game(info.Cells, info.Generation)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Save multiple games to a json file.
        /// </summary>
        /// <param name="games">List of games to save.</param>
        public void SaveGamesJson(List<Game> games)
        {
            try
            {
                var data = games.Select(game => game.AsGameInfo()).ToList();
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filename, json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
