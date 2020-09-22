using System;

namespace GameOfLife
{
    /// <summary>
    /// Methods for displaying the game in the console window.
    /// </summary>
    public class GamePrinter
    {
        /// <summary>
        /// Print game state to the provided output.
        /// </summary>
        /// <param name="output">Destination where to write the game state.</param>
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
    }
}
