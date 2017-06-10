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
        public static int direction;

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
                    direction = 0;

                    // checking for movements and rotation
                    InputEvents();

                    // trying to move the piece 1 row down
                    if (!tryMove(curPiece, curX + 1, curY + direction))
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

                    // printing matrix
                    HelperFunctions.PrintMatrix(matrix, 1, 1);
                    Thread.Sleep(150);
                }
            }

            Console.WriteLine("   Restart: Y/N");
            var restartKey = Console.ReadKey(true);
            switch (restartKey.Key)
            {
                case ConsoleKey.Y:
                    Console.Clear();
                    Launcher.DrawBorder();
                    //StartGame();
                    break;
                case ConsoleKey.N:
                    Launcher.MainMenu();
                    break;
            }
        }

        private static void InputEvents()
        {
            if (Console.KeyAvailable)
            {
                key = Console.ReadKey();
                isKeyPressed = true;
            }
            else isKeyPressed = false;

            if (isKeyPressed)
            {
                if (Tetris.key.Key == ConsoleKey.LeftArrow & isKeyPressed)
                {
                    direction = -1;
                }
                else if (Tetris.key.Key == ConsoleKey.RightArrow & isKeyPressed)
                {
                    direction = 1;
                }
                else if (Tetris.key.Key == ConsoleKey.Z)
                {
                    HelperFunctions.DeletePreviousPosition(curPiece, matrix, curX, curY);
                    curPiece = HelperFunctions.RotateMatrixCounterClockwise(curPiece);
                }
                else if (Tetris.key.Key == ConsoleKey.X)
                {
                    HelperFunctions.DeletePreviousPosition(curPiece, matrix, curX, curY);
                    curPiece = HelperFunctions.RotateMatrixClockwise(curPiece);
                }
            }

            isKeyPressed = false;
        }
        
        private static bool tryMove(bool[,] piece, int newX, int newY)
        {
            if (!Collision(piece, newX, newY)) return false;

            // deleting previous position
            HelperFunctions.DeletePreviousPosition(piece, matrix, curX, curY);

            // setting the new coordinates of the current piece
            curX = newX;
            curY = newY;

            // adding piece on the new position
            HelperFunctions.SettingPieceInMatrix(piece, matrix, curX, curY);

            return true;
        }

        private static bool Collision(bool[,] piece, int newX, int newY)
        {
            // check if end of frame
            if (newX + piece.GetLength(0) > matrix.GetLength(0))
                return false;

            //collision - needs fixing
            for (int row = 0; row < piece.GetLength(0); row++)
            {
                for (int col = 0; col < piece.GetLength(1); col++)
                {
                    if (row < piece.GetLength(0) - 1
                        && direction == 0
                        && !piece[row + 1, col]
                        && matrix[newX + row, newY + col])
                        return false;
                }
            }

            for (int col = 0; col < piece.GetLength(1); col++)
            {
                if (piece[piece.GetLength(0) - 1, col]
                    && matrix[newX + piece.GetLength(0) - 1, newY + col])
                    return false;
            }

            return true;
        }
    }
}
