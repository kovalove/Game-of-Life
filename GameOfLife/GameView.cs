using System;
using System.Collections.Generic;

namespace GameOfLife
{
    /// <summary>
    /// Presentation layer for the console window input / output operations..
    /// </summary>
    public class GameView
    {
        /// <summary>
        /// Print game state to the console.
        /// </summary>
        /// <param name="gane">Game which state to display.</param>
        public void Print(Game game)
        {
            int count = 0;

            for (int r = 0; r < game.Rows; r++)
            {
                for (int c = 0; c < game.Columns; c++)
                {
                    bool alive = game.IsAlive(r, c);
                    Console.Write(alive ? "+" : " ");
                    if (alive)
                    {
                        count++;
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("Count of live cells: {0}", count);
            Console.WriteLine("Step: {0}", game.Generation);
            Console.WriteLine();
        }

        /// <summary>
        /// Print state of previously selected games to the console.
        /// </summary>
        /// <param name="games">List of games.</param>
        /// <param name="selections">List of games indexes to display.</param>
        public void PrintGames(List<Game> games, List<int> selections)
        {
            Console.Clear();
            foreach (int index in selections)
            {
                Game game = games[index - 1];
                Print(game);
            }
        }
    }
}
