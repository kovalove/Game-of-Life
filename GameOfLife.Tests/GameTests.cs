using System;
using Xunit;

namespace GameOfLife.Tests
{
    public class GameTests
    {

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(100, 100)]
        public void ConstructorInitializedProperly(int rows, int cols)
        {
            var game = new Game(rows, cols);
            Assert.Equal(rows, game.Rows);
            Assert.Equal(cols, game.Columns);
            Assert.Equal(0, game.CountAlive);
            Assert.Equal(1, game.Generation);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, -1)]
        [InlineData(-1, -1)]
        public void ConstructorThrowsOverflowException(int rows, int cols)
        {
            Assert.Throws<OverflowException>(() => new Game(rows, cols));
        }


        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(100, 100)]
        public void NewGameCellsAreDead(int rows, int cols)
        {
            var game = new Game(rows, cols);
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    var alive = game.IsAlive(r, c);
                    Assert.False(alive);
                }
            }
        }
    }
}
