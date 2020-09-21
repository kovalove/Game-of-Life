using System;
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
        private Stopwatch watch;
        private readonly GamePrinter printer = new GamePrinter();
        private readonly GameSaver saver = new GameSaver();

        public Terminal()
        {
            timer = new Timer(1000);
            timer.AutoReset = false;
            timer.Elapsed += OnTimerElapsed;
            watch = new Stopwatch();
        }

        /// <summary>
        /// Start the main program loop.
        /// </summary>
        public void Run()
        {
            Game game = Start();
            if (game == null)
            {
                Console.WriteLine("Exiting...");
                return;
            }

            // Show initial state
            StartTimer();
            Print(game);

            // Advance while playing
            while (Advance(game))
            {
                StartTimer();
                game.Step();
                Print(game);
            }
        }

        /// <summary>
        /// Loading or creating a new game depending on the user choise.
        /// </summary>
        /// <returns>New game instance; null if user want to exit the programm.</returns>
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
                int selection;
                if (int.TryParse(Console.ReadLine(), out selection))
                    switch (selection)
                    {
                        case 1:
                            return CreateGame();
                        case 2:
                            Game game = LoadGame();
                            if (game != null)
                            {
                                return game;
                            }
                            break;
                        case 0:
                            return null;
                        default:
                            Console.WriteLine("Please select item from a list.");
                            break;
                    }
            }
        }

        /// <summary>
        /// Read the size and initailize game with randomly dead or alive cells.
        /// </summary>
        /// <returns>New game state of the user specified size.</returns>
        private Game CreateGame()
        {
            // Read input to start the game
            int rows = Ask("Enter the number of rows (max: 45000): ");
            int columns = Ask("Enter the number of columns (max: 45000): ");

            // Initailize game with randomly dead or alive cells
            Game game = new Game(rows, columns);
            game.Randomize();
            return game;
        }

        /// <summary>
        /// Read number from the user input withing defined range.
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
        /// Loading game from file.
        /// </summary>
        /// <returns>Newly crated game instance having data from file.</returns>
        private Game LoadGame()
        {
            // Load from file
            return saver.Load("save.txt");
        }

        /// <summary>
        /// Process user input and decide if game should advance to a next step.
        /// </summary>
        /// <param name="game">The game in which to perform an operation.</param>
        /// <returns>True when game should advance to a next step; false to exit the program.</returns>
        private bool Advance(Game game)
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
                            saver.SaveGame(game, "save.txt");
                            Console.WriteLine("Game successfully saved!");
                            break;

                        case ConsoleKey.Escape:
                            return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Print game state to the console.
        /// </summary>
        /// <param name="game">Game to be printed.</param>
        public void Print(Game game)
        {
            long processingTime = watch.ElapsedMilliseconds;

            Console.Clear();
            printer.Print(game, Console.Out);
            long printingTime = watch.ElapsedMilliseconds - processingTime;

            Console.WriteLine("Processing Time: " + processingTime + "ms");
            Console.WriteLine("Printing Time: " + printingTime + "ms");
        }


        private void StartTimer()
        {
            waiting = true;
            timer.Start();
            watch.Restart();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            waiting = false;
        }
    }
}
