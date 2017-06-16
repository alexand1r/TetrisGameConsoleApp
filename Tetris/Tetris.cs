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
        public static int MATRIX_ROWS = 22;
        public static int MATRIX_COLS = 10;
        public static int curX;
        public static int curY;
        public static bool[,] bomb = new bool[1, 1] { { true } };
        public static int aboutToBoomCounter = 0;

        public static Random rnd = new Random();
        public static Stack<bool[,]> pieces = new Stack<bool[,]>();
        public static bool gameOver;
        public static ConsoleKeyInfo key;
        public static bool isKeyPressed = false;

        public static int LineCleared = 0;
        public static int Score = 0;
        public static int Level = 1;
        public static int Combo = 0;
        public static int Speed = 250;

        public static void StartGame()
        {
            isKeyPressed = false;
            gameOver = false;
            LineCleared = 0;
            Score = 0;
            Combo = 0;
            Level = 1;
            Speed = 250;
            aboutToBoomCounter = 0;

            var blocks = Blocks.createBlocks();
            matrix = new bool[MATRIX_ROWS, MATRIX_COLS];

            // filling matrix
            HelperFunctions.FillMatrix(matrix);

            while (true)
            {
                if (gameOver) break;
                //printing score/level/lines
                PrintStats();
                // picking new piece and next piece
                bool[,] newPiece;
                if (aboutToBoomCounter >= 10)
                    newPiece = bomb;
                else
                {
                    newPiece = pieces.Count == 0
                        ? HelperFunctions.PickRandomBlock(blocks, rnd) : pieces.Pop();

                    pieces.Push(HelperFunctions.PickRandomBlock(blocks, rnd));
                    HelperFunctions.NextBlock(pieces.Peek());
                }

                // setting new piece's coordinates
                curPiece = newPiece;
                // change tetris color when bomb incoming
                if (newPiece.Equals(bomb))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(38, 20);
                    Console.Write("BOMB INCOMING!");
                }
                
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
                        if (curPiece.GetLength(0) == 1 && curPiece.GetLength(1) == 1)
                        {
                            for (int col = 0; col < matrix.GetLength(1); col++)
                            {
                                matrix[curX, col] = true;
                            }
                            ClearLines();
                            SetLevelScoreAndSpeed();
                            for (int row = 0; row < matrix.GetLength(0); row++)
                            {
                                matrix[row, curY] = false;
                            }
                        }

                        // check if piece can't move from the top of the frame
                        if (curX + 1 == 1)
                        {   
                            Console.SetCursorPosition(37, 19);
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
                    Thread.Sleep(Speed);
                }
                ClearLines();
                SetLevelScoreAndSpeed();
                aboutToBoomCounter++;
            }
            HelperFunctions.AskForRestart();
        }

        private static void PrintStats()
        {
            Console.SetCursorPosition(39, 2);
            Console.Write(Score);
            Console.SetCursorPosition(34, 6);
            Console.Write(Level);
            Console.SetCursorPosition(47, 6);
            Console.Write(LineCleared);
        }

        public static void ClearLines()
        {
            for (int row = matrix.GetLength(0) - 1; row > 0; row--)
            {
                int counter = 0;
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    if (matrix[row, col])
                    {
                        counter++;
                    }
                    // clear the row
                    if (counter == MATRIX_COLS)
                    {
                        LineCleared++;
                        aboutToBoomCounter = 0;
                        for (int i = 0; i < MATRIX_COLS; i++)
                        {
                            matrix[row, i] = false;
                        }
                        // fall down
                        FallDown(row);
                        // combo scores
                        Combo++;
                        // check for another empty full row is created (recursion)
                        ClearLines();
                    }
                }
            }
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(38, 20);
                Console.Write(new string(' ', 14));
        }

        public static void SetLevelScoreAndSpeed()
        {
            if (LineCleared < 5) { Level = 1; Speed = 250; }
            else if (LineCleared < 10) { Level = 2; Speed = 230; }
            else if (LineCleared < 15) { Level = 3; Speed = 210; }
            else if (LineCleared < 25) { Level = 4; Speed = 190; }
            else if (LineCleared < 35) { Level = 5; Speed = 170; }
            else if (LineCleared < 50) { Level = 6; Speed = 150; }
            else if (LineCleared < 70) { Level = 7; Speed = 130; }
            else if (LineCleared < 90) { Level = 8; Speed = 110; }
            else if (LineCleared < 110) { Level = 9; Speed = 90; }

            if (Combo >= 1)
            {
                Score += (Combo * 50) * Level;
                Combo = 0;
            }
        }


        public static void FallDown(int row)
        {
            for (int column = 0; column < matrix.GetLength(1); column++)
            {
                for (int curRow = row - 1; curRow >= 0; curRow--)
                {
                    if (matrix[curRow, column])
                    {
                        matrix[curRow + 1, column] = matrix[curRow, column];
                        matrix[curRow, column] = false;
                    }
                }
            }
        }
    }
}
