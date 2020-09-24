namespace GameOfLife
{
    /// <summary>
    /// Entry point for starting a game in the console window.
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            GameController controller = new GameController();
            controller.Run();
        }
    }
}
