using System;
using System.Security.Cryptography.X509Certificates;

namespace GameOfLife
{
    public class Program
    {
        static void Main(string[] args)
        {
            // read input to start the game
            Console.Write("Enter the number of rows: ");
            int rows = int.Parse(Console.ReadLine());

            Console.Write("Enter the number of Columns: ");
            int columns = int.Parse(Console.ReadLine());

            // initailize game with randomly dead or alive cells
            Game game = new Game(rows, columns);
            game.Randomize();
            game.Print();
            
            // advance game every 1s
            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                game.Step();
                game.Print();
            }
        }
    }
}
