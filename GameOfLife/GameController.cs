using System;
using System.Collections.Generic;
using System.Timers;

namespace GameOfLife
{
    /// <summary>
    /// Game controller to process and display game steps.
    /// </summary>
    public class GameController
    {
        private Timer timer;
        private bool waiting;
        private readonly GameView view = new GameView();
        private readonly GameSaver saver = new GameSaver("save.json");
        private List<Game> games = new List<Game>();
        private List<int> displayGames;
        private int activeCount;
        private int totalAlive;

        /// <summary>
        /// Initialize new instance with default values.
        /// </summary>
        public GameController()
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
                view.ShowMessage("Exiting...");
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
        /// <returns> True if application should continue; false if user choose to exit.</returns>
        private bool Start()
        {
            GameMenuOption choice = view.AskGameMenu();
            switch (choice)
            {
                case GameMenuOption.NewGame:
                    CreateGame();
                    return true;
                case GameMenuOption.LoadGame:
                    LoadGame();
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Advance all games to a next generation.
        /// </summary>
        private void Step()
        {
            activeCount = 0;
            totalAlive = 0;

            foreach (Game game in games)
            {
                game.Step();
                totalAlive += game.CountAlive;

                if (game.Active)
                {
                    activeCount++;
                }
            }
        }

        /// <summary>
        /// Ask user for game paramters and initialize application with new games.
        /// Games will have randomly assigned dead or alive cells.
        /// </summary>
        private void CreateGame()
        {
            // Read input to start the game
            int rows = view.AskNumber("Enter the number of rows", 1, 20);
            int columns = view.AskNumber("Enter the number of columns", 1, 20);
            activeCount = view.AskNumber("Enter the number of games you want to generate", 1, 1000);
            AskDisplayGames(activeCount);

            // Initailize games with randomly dead or alive cells
            for (int i = 0; i < activeCount; i++)
            {
                Game game = new Game(rows, columns);
                game.Randomize();
                games.Add(game);
                totalAlive += game.CountAlive;
            }
        }

        /// <summary>
        /// Loading games from the file.
        /// </summary>
        /// <returns>True when sucessfully loaded; false if error occured or no games were loaded.</returns>
        private bool LoadGame()
        {
            // load many games
            games = saver.LoadGamesJson();
            activeCount = games.Count;

            foreach (Game game in games)
            {
                totalAlive += game.CountAlive; 
            }

            AskDisplayGames(activeCount);
            return games != null;
        }

        /// <summary>
        /// Process user input and decide if game should advance to a next step.
        /// Returns true when program should advance to a next step; false to exit the program.
        /// </summary>
        /// <returns>True when program should advance to a next step; false to exit the program.</returns>
        private bool Advance()
        {
            view.ShowMessage("Press any key to pause...");

            while (waiting)
            {
                if (view.IsAnyKeyDown())
                {
                    return Pause();
                }
            }

            return true;
        }

        /// <summary>
        /// Pause the game until further user instructions.
        /// </summary>
        /// <returns>True to continue the game; false to exit.</returns>
        private bool Pause()
        {
            view.ShowPauseMenu();
            while (true)
            {
                switch (view.ReadPauseKey())
                {
                    case GamePauseOption.Continue:
                        return true;

                    case GamePauseOption.Save:
                        // save all games
                        saver.SaveGamesJson(games);
                        view.ShowMessage("Game successfully saved!");
                        break;
                    case GamePauseOption.ChangeGames:
                        AskDisplayGames(games.Count);
                        return true;
                    case GamePauseOption.Exit:
                        return false;
                }
            }
        }

        private void AskDisplayGames(int count)
        {
            displayGames = view.AskNumberList("Select games you want to see on the screen separated by a space", 8, 1, count);
        }

        private void Print()
        {
            view.PrintGames(games, displayGames, activeCount, totalAlive);
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
