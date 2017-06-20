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
        public static bool[,] curPiece, matrix, bomb = new bool[1, 1] { { true } };
        public static int MATRIX_ROWS = 22, MATRIX_COLS = 10, curX, curY;

        public static Random rnd = new Random();
        public static Stack<bool[,]> pieces = new Stack<bool[,]>();
        public static bool gameOver, isKeyPressed;
        public static ConsoleKeyInfo key;

        public static int LineCleared, Score, Level = 1, Combo, Speed = 250;

        public static void StartGame()
        {
            isKeyPressed = false;
            gameOver = false;
            LineCleared = 0;
            Score = 0; Combo = 0;
            Level = 1; Speed = 250;
            int piecesCounter = 0;

            var blocks = Blocks.createBlocks();
            matrix = new bool[MATRIX_ROWS, MATRIX_COLS];

            while (true)
            {
                if (gameOver) break;

                // printing score/level/lines
                HelperFunctions.PrintStats();

                // picking new piece and next piece
                bool[,] newPiece;
                newPiece = pieces.Count == 0
                    ? HelperFunctions.PickRandomBlock(blocks, rnd)
                    : pieces.Pop();
                piecesCounter++;

                // if first piece is a bomb -> pick another one
                while (piecesCounter == 1 && newPiece.GetLength(0) == 1 && newPiece.GetLength(1) == 1)
                {
                    newPiece = HelperFunctions.PickRandomBlock(blocks, rnd);
                }

                pieces.Push(HelperFunctions.PickRandomBlock(blocks, rnd));
                HelperFunctions.NextBlock(pieces.Peek());

                // setting new piece's coordinates
                curPiece = newPiece;
                curX = 0;
                curY = matrix.GetLength(1) / 2 - 1;

                // change tetris color when bomb incoming
                HelperFunctions.ChangeConsoleColor(newPiece);

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
                        // if piece is the bomb - destroy
                        if (curPiece.GetLength(0) == 1 && curPiece.GetLength(1) == 1)
                        {
                            HelperFunctions.DestroyWithBomb();
                        }

                        // check if piece can't move from the top of the frame
                        if (curX + 1 == 1)
                        {
                            // get player's name and add highscore
                            HelperFunctions.GameOver();
                            gameOver = true;
                        }

                        // exit the cicle and get next block
                        break;
                    }

                    // move piece down
                    HelperFunctions.ReplacePosition(curPiece, curX + 1, curY);

                    // printing matrix
                    HelperFunctions.PrintMatrix(matrix, 1, 1);
                    Thread.Sleep(Speed);
                }

                // check for full lines and drop blocks
                HelperFunctions.ClearLines();

                // calculate level, score and speed
                HelperFunctions.SetLevelScoreAndSpeed();
            }

            HelperFunctions.AskForRestart();
        }
    }
}
