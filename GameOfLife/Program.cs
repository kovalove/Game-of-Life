using System;


namespace GameOfLife
{
    public class Program
    {
        const bool ADVANCE_MANUALLY = false;

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
            while (Advance())
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
                string input = Console.ReadLine();
                int selection = int.Parse(input);

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
            Console.Write("Enter the number of rows: ");
            int rows = int.Parse(Console.ReadLine());

            Console.Write("Enter the number of Columns: ");
            int columns = int.Parse(Console.ReadLine());

            // initailize game with randomly dead or alive cells
            Game game = new Game(rows, columns);
            game.Randomize();
            return game;
        }

        static Game LoadGame()
        {
            // TODO: load from file
            Game game = new Game(0, 0);
            game.Load();
            return game;
        }

        static bool Advance()
        {
            if (ADVANCE_MANUALLY)
            {
                // TODO: implement advance on keypress
                return false;
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
