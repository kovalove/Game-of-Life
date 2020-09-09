using System;

namespace GameOfLife
{
    public class Program
    {
        const bool ADVANCE_MANUALLY = true;

        static void Main(string[] args)
        {
            Game game = Start();
            if (game == null)
            {
                Console.WriteLine("Exiting...");
                return;
            }

            // show initial state
            game.Print();

            // advance while playing
            while (Advance(game))
            {
                game.Step();
                game.Print();
            }
        }

        static Game Start()
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

        static Game CreateGame()
        {
            // read input to start the game
            int rows = Ask("Enter the number of rows (max: 45000): ");
            int columns = Ask("Enter the number of columns (max: 45000): ");

            // initailize game with randomly dead or alive cells
            Game game = new Game(rows, columns);
            game.Randomize();
            return game;
        }

        static int Ask(string label)
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

        static Game LoadGame()
        {
            // load from file
            Game game = new Game(0, 0);
            game.Load("../../../save.txt");
            return game;
        }

        static bool Advance(Game game)
        {
            if (ADVANCE_MANUALLY)
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
            else
            {
                // advance every 1s
                System.Threading.Thread.Sleep(1000);
                return true;
            }
        }
    }
}
