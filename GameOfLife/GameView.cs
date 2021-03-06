﻿using System;
using System.Collections.Generic;

namespace GameOfLife
{
    /// <summary>
    /// Presentation layer for the console window input / output operations..
    /// </summary>
    public class GameView
    {
        const char ALIVE_CELL = '+';
        const char DEAD_CELL = ' ';

        /// <summary>
        /// Show main menu asking for user choice.
        /// Keeps asking until user selects a correct item.
        /// </summary>
        /// <returns>User choice.</returns>
        public GameMenuOption AskGameMenu()
        {
            Console.WriteLine("GAME OF LIFE");
            Console.WriteLine("1. New Game");
            Console.WriteLine("2. Load Game");
            Console.WriteLine("0. Exit");
            Console.WriteLine();

            while (true)
            {
                int selection;
                Console.Write("Selection: ");
                bool sucess = int.TryParse(Console.ReadLine(), out selection);
                if (sucess)
                    switch (selection)
                    {
                        case 1:
                            return GameMenuOption.NewGame;
                        case 2:
                            return GameMenuOption.LoadGame;
                        case 0:
                            return GameMenuOption.Exit;
                        default:
                            Console.WriteLine("Please select item from a list.");
                            break;
                    }
            }
        }

        /// <summary>
        /// Show pause menu items.
        /// </summary>
        public void ShowPauseMenu()
        {
            Console.WriteLine("Press 'ENTER' advance to next generation...");
            Console.WriteLine("Press 'ESC' to exit the game...");
            Console.WriteLine("Press 'S' to save the game...");
            Console.WriteLine("Press 'C' to change the games on the screen...");
        }

        /// <summary>
        /// Read input key to selection action available while game on pause.
        /// Keeps reading until user selects a correct item.
        /// </summary>
        /// <returns>User choice.</returns>
        public GamePauseOption ReadPauseKey()
        {
            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:
                        return GamePauseOption.Continue;
                    case ConsoleKey.S:
                        return GamePauseOption.Save;
                    case ConsoleKey.C:
                        return GamePauseOption.ChangeGames;
                    case ConsoleKey.Escape:
                        return GamePauseOption.Exit;
                }
            }
        }

        /// <summary>
        /// Show the message to the user.
        /// </summary>
        /// <param name="message">The message to show.</param>
        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Read number from the user input within defined range.
        /// Keeps asking until user inputs a correct value.
        /// </summary>
        /// <param name="label">Label to show while asking for a value.</param>
        /// <param name="min">Minimum value to accept.</param>
        /// <param name="max">Maximum value to accept.</param>
        /// <returns>A valid number within defined range.</returns>
        public int AskNumber(string label, int min, int max)
        {
            while (true)
            {
                Console.Write(label + " [{0} - {1}]: ", min, max);

                if (int.TryParse(Console.ReadLine(), out int number))
                {
                    if (number < min || number > max)
                    {
                        Console.WriteLine("Please enter the number in the range: from {0} to {1}", min, max);
                    }
                    else
                    {
                        return number;
                    }
                }

                else
                {
                    Console.WriteLine("Value should be an integer");
                }

            }
        }

        /// <summary>
        /// Read list of numbers from the user input within defined range.
        /// Keeps asking until user inputs a correct value.
        /// </summary>
        /// <param name="label">Label to show while asking for a value.</param>
        /// <param name="limit">Maximum number of items to read.</param>
        /// <param name="min">Minimum value to accept.</param>
        /// <param name="max">Maximum value to accept.</param>
        /// <returns>A valid number within defined range.</returns>
        public List<int> AskNumberList(string label, int limit, int min, int max)
        {
            while (true)
            {
                Console.Write("{0} [{1} - {2}] (max {3} items): ", label, min, max, limit);
                string[] inputs = Console.ReadLine().Split(" ");
                if (inputs.Length < 1)
                {
                    Console.WriteLine("You have to enter at least one number");
                    continue;
                }

                if (inputs.Length > limit)
                {
                    Console.WriteLine("You cannot enter more than {0} items", limit);
                    continue;
                }

                bool success = true;
                var numbers = new List<int>();
                foreach (string input in inputs)
                {
                    if (int.TryParse(input, out int number))
                    {
                        if (number >= min && number <= max)
                        {
                            numbers.Add(number);
                        }
                        else
                        {
                            success = false;
                            Console.WriteLine("Each number should be in the range: [{0} - {1}]", min, max);
                            break;
                        }
                    }
                    else
                    {
                        success = false;
                        Console.WriteLine("Value should be an integer");
                        break;
                    }
                }

                if (success)
                {
                    return numbers;
                }
            }
        }

        /// <summary>
        /// Check if user pressed any key.
        /// </summary>
        /// <returns>True if any key is down; false otherwise.</returns>
        public bool IsAnyKeyDown()
        {
            if (Console.KeyAvailable)
            {
                Console.ReadKey(true);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Print game state to the console.
        /// </summary>
        /// <param name="game">Game which state to display.</param>
        public void Print(Game game)
        {
            for (int r = 0; r < game.Rows; r++)
            {
                for (int c = 0; c < game.Columns; c++)
                {
                    bool alive = game.IsAlive(r, c);
                    Console.Write(alive ? ALIVE_CELL : DEAD_CELL);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("Count of live cells: {0}", game.CountAlive);
            Console.WriteLine("Step: {0}", game.Generation);
            Console.WriteLine();
        }

        /// <summary>
        /// Print state of previously selected games to the console.
        /// </summary>
        /// <param name="games">List of games.</param>
        /// <param name="selections">List of games indexes to display.</param>
        /// <param name="activeCount">Count of live games.</param>
        /// <param name="totalAlive">Count of total alive cells from all games.</param>
        public void PrintGames(List<Game> games, List<int> selections, int activeCount, int totalAlive)
        {
            Console.Clear();
            foreach (int index in selections)
            {
                Game game = games[index];
                Print(game);
            }

            Console.WriteLine("Live games count: {0}", activeCount);
            Console.WriteLine("Total live cells count: {0}", totalAlive);
        }
    }
}
