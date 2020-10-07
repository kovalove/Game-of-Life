using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GameOfLife.Tests
{
    public class GameTests
    {
        const char ALIVE_CELL = '+';
        const char DEAD_CELL = ' ';

        // https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life#/media/File:Game_of_life_block_with_border.svg
        private static string[] Block = new string[]
        {
            "++",
            "++",
        };


        // https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life#/media/File:Game_of_life_beehive.svg
        private static string[] BeeHive = new string[]
        {
            " ++ ",
            "+  +",
            " ++ ",
        };


        // https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life#/media/File:Game_of_life_blinker.gif
        private static string[][] Blinker = new string[][]
        {
            new string[]
            {
                " + ",
                " + ",
                " + ",
            },
            new string[]
            {
                "   ",
                "+++",
                "   ",
            }
        };

        // https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life#/media/File:Game_of_life_toad.gif
        private static string[][] Toad = new string[][]
        {
            new string[]
            {
                "  + ",
                "+  +",
                "+  +",
                " +  ",
            },
            new string[]
            {
                "    ",
                " +++",
                "+++ ",
                "    ",
            },
        };

        // https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life#/media/File:Game_of_life_animated_glider.gif
        private static string[][] Glider = new string[][]
        {
            new string[]
            {
                "+ + ",
                " ++ ",
                " +  ",
                "    ",
            },
            new string[]
            {
                "  + ",
                "+ + ",
                " ++ ",
                "    ",
            },
            new string[]
            {
                " +  ",
                "  ++",
                " ++ ",
                "    ",
            },
            new string[]
            {
                "  + ",
                "   +",
                " +++",
                "    ",
            },
            new string[]
            {
                "    ",
                " + +",
                "  ++",
                "  + ",
            },
        };

        private static string[][] Custom = new string[][]
        {
            new string[]
            {
                "++++ ",
                "+   +",
                "+ + +",
                "+    ",
                " ++++",
            },
            new string[]
            {
                "++++ ",
                "+   +",
                "+  + ",
                "+   +",
                " +++ ",
            },
            new string[]
            {
                "++++ ",
                "+   +",
                "++ ++",
                "+   +",
                " +++ ",
            },
            new string[]
            {
                "++++ ",
                "    +",
                "++ ++",
                "+   +",
                " +++ ",
            },
        };

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

        [Theory]
        [MemberData(nameof(GenerationSequenceData))]
        public void GenerationSequeneIsCorrect(string[][] states)
        {
            string[] initial = states[0];
            var cells = ToCells(initial);
            var game = new Game(cells, 0);
            for (int s = 1; s < states.Length; s++)
            {
                game.Step();
                var lines = states[s];
                for (int row = 0; row < lines.Length; row++)
                {
                    string expect = lines[row];
                    string actual = GameRowToString(game, row);
                    Assert.Equal(expect, actual);
                }
            }
        }

        public static IEnumerable<object[]> GenerationSequenceData()
        {
            // https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life
            return new List<object[]>
            {
                // still lifes
                new object[] {new string[][] { Block, Block } },
                new object[] {new string[][] { BeeHive, BeeHive } },

                // oscilators
                new object[] { Blinker },
                new object[] { Toad },

                // spaceships
                new object[] { Glider },

                // custom
                new object[] { Custom },
            };
        }

        private static string GameRowToString(Game game, int r)
        {
            int columns = game.Columns;
            StringBuilder sb = new StringBuilder();
            for (int c = 0; c < columns; c++)
            {
                char cell = game.IsAlive(r, c) ? ALIVE_CELL : DEAD_CELL;
                sb.Append(cell);
            }

            return sb.ToString();
        }

        private static bool[,] ToCells(string[] lines)
        {
            int rows = lines.Length;
            int cols = lines[0].Length;
            bool[,] cells = new bool[rows, cols];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    cells[r, c] = lines[r][c] == ALIVE_CELL;
                }
            }

            return cells;
        }
    }
}
