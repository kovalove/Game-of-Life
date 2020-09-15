using System;
using System.Diagnostics;

namespace GameOfLife
{
    public class Terminal
    {
        public void Run()
        {
            Game game = Start();
            if (game == null)
            {
                Console.WriteLine("Exiting...");
                return;
            }

            // show initial state
            Stopwatch watch = Stopwatch.StartNew();
            game.Print();

            // advance while playing
            while (Advance(game, watch))
            {
                watch.Restart();
                game.Step();
                game.Print();
            }
        }

        private Game Start()
        {
            Console.WriteLine("GAME OF LIFE");
            Console.WriteLine("1. New Game");
            Console.WriteLine("2. Load Game");
            Console.WriteLine("0. Exit");
            Console.WriteLine("");

            while (true)
            {
                Console.Write("Selection: ");
                //string input = Console.ReadLine();
                int selection;
                if (int.TryParse(Console.ReadLine(), out selection))
                    switch (selection)
                    {
                        case 1:
                            return CreateGame();
                        case 2:
                            return LoadGame();
                        case 0:
                            return null;
                        default:
                            Console.WriteLine("Please select item from a list.");
                            break;
                    }
            }
        }

        private Game CreateGame()
        {
            // read input to start the game
            int rows = Ask("Enter the number of rows (max: 45000): ");
            int columns = Ask("Enter the number of columns (max: 45000): ");

            // initailize game with randomly dead or alive cells
            Game game = new Game(rows, columns);
            game.Randomize();
            return game;
        }

        private int Ask(string label)
        {
            while (true)
            {
                try
                {
                    Console.Write(label);
                    int input = int.Parse(Console.ReadLine());

                    if (input <= 0 || input > 45000)
                    {
                        Console.WriteLine("Please enter the number in the range from 1 to 45000");
                    }
                    else
                    {
                        return input;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private Game LoadGame()
        {
            // load from file
            Game game = new Game(0, 0);
            game.Load("../../../save.txt");
            return game;
        }

        private bool Advance(Game game, Stopwatch watch)
        {
            bool pause = false;
            Console.WriteLine("Press any key to pause...");
            while (watch.ElapsedMilliseconds < 1000)
            {
                if (Console.KeyAvailable)
                {
                    pause = true;
                    Console.ReadKey();
                    break;
                }
            }

            if (pause)
            {
                Console.WriteLine("Press 'ENTER' advance to next generation...");
                Console.WriteLine("Press 'ESC' to exit the game...");
                Console.WriteLine("Press 'S' to save the game...");

                while (true)
                {
                    switch (Console.ReadKey().Key)
                    {
                        // continue the game
                        case ConsoleKey.Enter:
                            return true;

                        case ConsoleKey.S:
                            game.SaveGame("../../../save.txt");
                            Console.WriteLine("Game successfully saved!");
                            break;

                        case ConsoleKey.Escape:
                            return false;
                    }
                }
            }

            return true;
        }
    }
}
