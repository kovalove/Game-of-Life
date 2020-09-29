namespace GameOfLife
{
    /// <summary>
    /// Serialization data of the game.
    /// </summary>
    public class GameInfo
    {
        public bool[,] Cells { get; set; }
        public int Generation { get; set; }
    }
}
