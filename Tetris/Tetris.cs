using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tetris
{
    public class Tetris
    {
        public static bool[,] curPiece;
        public static bool[,] matrix;
        public static int curX;
        public static int curY;

        public static Random rnd = new Random();
        public static Stack<bool[,]> pieces = new Stack<bool[,]>();
        public static bool gameOver;
        public static ConsoleKeyInfo key;
        public static bool isKeyPressed = false;

        public static void StartGame()
        {
            isKeyPressed = false;
            gameOver = false;

            var blocks = Blocks.createBlocks();
            matrix = new bool[22, 24];

            // filling matrix
            HelperFunctions.FillMatrix(matrix);

            while (true)
            {
                if (gameOver) break;

                // picking new piece and next piece
                bool[,] newPiece;
                newPiece = pieces.Count == 0
                    ? HelperFunctions.PickRandomBlock(blocks, rnd) : pieces.Pop();

                pieces.Push(HelperFunctions.PickRandomBlock(blocks, rnd));
                HelperFunctions.NextBlock(pieces.Peek());

                curPiece = newPiece;
                curX = 0;
                curY = matrix.GetLength(1) / 2 - 1;

                // setting the new piece on the top middle
                HelperFunctions.SettingPieceInMatrix(curPiece, matrix, curX, curY);

                // printing matrix
                HelperFunctions.PrintMatrix(matrix, 1, 1);

                while (true)
                {
                    // checking for movements and rotation
                    HelperFunctions.InputEvents();

                    // trying to move the piece 1 row down
                    if (!HelperFunctions.TryMove(matrix, curPiece, curX + 1, curY, "down"))
                    {
                        // check if piece can't move from the top of the frame
                        if (curX + 1 == 1)
                        {
                            Console.SetCursorPosition(1, 24);
                            Console.Write("Game Over");
                            gameOver = true;
                        }
                        break;
                    }
                    else
                    {
                        HelperFunctions.ReplacePosition(curPiece, curX + 1, curY);
                    }

                    // printing matrix
                    HelperFunctions.PrintMatrix(matrix, 1, 1);
                    Thread.Sleep(150);
                }
            }

            HelperFunctions.AskForRestart();
        }
    }
}
