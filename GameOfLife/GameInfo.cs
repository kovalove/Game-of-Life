namespace GameOfLife
{
    /// <summary>
    /// Serialization data of the game.
    /// </summary>
    public class GameInfo
    {
        /// <summary>
        /// Two-dimensional array representing field of the cells where first dimension is rows and second is columns.
        /// </summary>
        public bool[,] Cells { get; set; }

        /// <summary>
        /// Number of generations.
        /// </summary>
        public int Generation { get; set; }
    }
}
