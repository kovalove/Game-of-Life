using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;

namespace GameOfLife
{
    /// <summary>
    /// Allows to interact with the game through console window.
    /// </summary>
    public class Terminal
    {
        private Timer timer;
        private bool waiting;
        private readonly GameView view = new GameView();
        private readonly GameSaver saver = new GameSaver();
        private List<Game> games = new List<Game>();
        private List<int> displayGames;

        /// <summary>
        /// Initialize new terminal instance with default values.
        /// </summary>
        public Terminal()
        {
            timer = new Timer(1000);
            timer.AutoReset = false;
            timer.Elapsed += OnTimerElapsed;
        }

        /// <summary>
        /// Start the main program loop.
        /// </summary>
        public void Run()
        {
            if (!Start())
            {
                Console.WriteLine("Exiting...");
                return;
            }

            // Show initial state
            StartTimer();
            Print();

            // Advance while playing
            while (Advance())
            {
                StartTimer();
                Step();
                Print();
            }
        }

        /// <summary>
        /// Show main menu and perform an operation depending on the user choice.
        /// </summary>
        /// <returns> True if application should start; false if user choose to exit.</returns>
        private bool Start()
        {
            Console.WriteLine("GAME OF LIFE");
            Console.WriteLine("1. New Game");
            Console.WriteLine("2. Load Game");
            Console.WriteLine("0. Exit");
            Console.WriteLine("");

            while (true)
            {
                Console.Write("Selection: ");
                int selection;
                bool sucess = int.TryParse(Console.ReadLine(), out selection);
                if (sucess)
                    switch (selection)
                    {
                        case 1:
                            CreateGame();
                            return true;
                        case 2:
                            return LoadGame();
                        case 0:
                            return false;
                        default:
                            Console.WriteLine("Please select item from a list.");
                            break;
                    }
            }
        }

        /// <summary>
        /// Advance all games to a next generation.
        /// </summary>
        private void Step()
        {
            foreach (Game game in games)
            {
                game.Step();
            }
        }

        /// <summary>
        /// Ask user for game paramters and initialize application with new games.
        /// Games will have randomly assigned dead or alive cells.
        /// </summary>
        private void CreateGame()
        {
            // Read input to start the game
            int rows = Ask("Enter the number of rows (max: 45000): ");
            int columns = Ask("Enter the number of columns (max: 45000): ");
            int count = AskGamesQuantity("Enter the number of games you want to generate (max: 1000): ");
            displayGames = AskGamesPrintQuantity("Select 8 games you want to see ont the screen separated by a space (max: 8 numbers): ");

            // Initailize games with randomly dead or alive cells
            for (int i = 0; i < count; i++)
            {
                Game game = new Game(rows, columns);
                game.Randomize();
                games.Add(game);
            }
        }

        /// <summary>
        /// Read number from the user input within defined range.
        /// Number must be greater than zero and less than 45000.
        /// </summary>
        /// <param name="label">Label to add when asking for a value.</param>
        /// <returns>A valid number within defined range.</returns>
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

        /// <summary>
        /// Read number of games the user wants to play.
        /// </summary>
        /// <param name="label">Label to add when asking for a value.</param>
        /// <returns>A valid number within defined range.</returns>
        private int AskGamesQuantity(string label)
        {
            while (true)
            {
                try
                {
                    Console.Write(label);
                    int input = int.Parse(Console.ReadLine());

                    if (input <= 0 || input > 1000)
                    {
                        Console.WriteLine("Please enter the number in the range from 1 to 1000");
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

        /// <summary>
        /// Read number of games the user wants to see on the screen.
        /// </summary>
        /// <param name="label">Label to add when asking for a value.</param>
        /// <returns>A list with numbers of games indexes.</returns>
        private List<int> AskGamesPrintQuantity(string label)
        {
            while (true)
            {
                try
                {
                    Console.Write(label);
                    string[] size = Console.ReadLine().Split(" ");
                    var list = new List<int>();
                    foreach(string s in size)
                    {
                        list.Add(int.Parse(s));
                    }
                    
                    if (size.Length < 0 || size.Length > 8)
                    {
                        Console.WriteLine("You entered more tham 8 games. Please enter maximum 8 games.");
                    }
                    else
                    {
                        return list;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Loading games from the file.
        /// </summary>
        /// <returns>True when sucessfully loaded; false if error occured or no games were loaded.</returns>
        private bool LoadGame()
        {
            // load many games
            games = saver.LoadGames("save.txt");
            displayGames = AskGamesPrintQuantity("Select 8 games you want to see ont the screen separated by a space (max: 8 numbers): ");
            return games != null;
        }

        /// <summary>
        /// Process user input and decide if game should advance to a next step.
        /// </summary>
        /// <returns>True when program should advance to a next step; false to exit the program.</returns>
        private bool Advance()
        {
            bool pause = false;
            Console.WriteLine("Press any key to pause...");
            while (waiting)
            {
                if (Console.KeyAvailable)
                {
                    pause = true;
                    Console.ReadKey(true);
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
                    switch (Console.ReadKey(true).Key)
                    {
                        // Continue the game
                        case ConsoleKey.Enter:
                            return true;

                        case ConsoleKey.S:
                            // save all games
                            saver.SaveGames(games, "save.txt");
                            Console.WriteLine("Game successfully saved!");
                            break;

                        case ConsoleKey.Escape:
                            return false;
                    }
                }
            }

            return true;
        }

        private void Print()
        {
            view.PrintGames(games, displayGames);
        }


        private void StartTimer()
        {
            waiting = true;
            timer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            waiting = false;
        }
    }
}
