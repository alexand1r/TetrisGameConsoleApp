using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public static class Blocks
    {
        public static List<bool[,]> createBlocks()
        {
            List<bool[,]> blocks = new List<bool[,]>();

            //blocks.Add(new bool[,] // ----
            //{
            //    {true, true, true, true}
            //});
            blocks.Add(new bool[,] // I
            {
                {true, false, false, false},
                {true, false, false, false},
                {true, false, false, false},
                {true, false, false, false}
            });
            blocks.Add(new bool[,] // J
            {
                {true, true, true},
                {false, false, true},
                { false, false, false}
            });
            blocks.Add(new bool[,] // L
            {
                {true, true, true},
                {true, false, false},
                {false, false, false}
            });
            blocks.Add(new bool[,] // O
            {
                {true, true},
                {true, true}
            });
            blocks.Add(new bool[,] // S
            {
                {false, true, true},
                {true, true, false},
                { false, false, false}
            });
            blocks.Add(new bool[,] // T
            {
                {true, true, true},
                {false, true, false},
                {false, false, false }
            });
            blocks.Add(new bool[,] // Z
            {
                {true, true, false},
                {false, true, true},
                {false, false, false}
            });

            return blocks;
        }
    }
}
