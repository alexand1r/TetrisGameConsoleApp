using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public static class Blocks
    {
        private static string block = "■";
        public static string[][] CreateBlocks()
        {
            List<string[][]> blocks = new List<string[][]>();


            //O piece
            blocks.Add(new[]
            {
                new [] { block, block },
                new [] { block, block }
            });

            //I Piece
            blocks.Add(new[]
            {
                new [] { block },
                new [] { block },
                new [] { block },
                new [] { block }
            });


            //S Piece
            blocks.Add(new[]
            {
                new [] { "", block, block },
                new [] { block, block, "" }
            });


            //Z Piece

            blocks.Add(new[]
            {
                 new [] { block, block, "" },
                 new [] { "", block, block }
            });

            //L Piece
            blocks.Add(new[]
            {
                new [] { block, "" },
                new [] { block, "" },
                new [] { block, block }
            });

            //J Piece
            blocks.Add(new[]
            {
                new [] { "", block },
                new [] { "", block },
                new [] { block, block }
            });

            //T Piece
            blocks.Add(new[]
            {
                new [] { block, block, block },
                new [] { "", block, "" }
            });

            var random = new Random();

            var pieceIndex = random.Next(0, blocks.Count);

            return blocks[pieceIndex];
        }
    }
}
