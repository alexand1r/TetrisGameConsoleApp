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
        public static string block = "■";
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
            FillMatrix();

            while (true)
            {
                if (gameOver) break;
                
                bool[,] newPiece;
                newPiece = pieces.Count == 0 
                    ? PickRandomBlock(blocks) : pieces.Pop();

                pieces.Push(PickRandomBlock(blocks));
                NextBlock(pieces.Peek());

                curPiece = newPiece;
                curX = 0;
                curY = matrix.GetLength(1) / 2;
                
                // setting the new piece on the top middle
                SettingPieceInMatrix();

                // printing matrix
                PrintMatrix();

                while (true)
                {
                    direction = 0;
                    InputEvents();
                    // trying to move the piece 1 row down
                    if (!tryMove(curPiece, curX + 1, curY + direction))
                    {
                        if (curX + 1 == 1)
                        {
                            Console.SetCursorPosition(1, 24);
                            Console.Write("Game Over");
                            gameOver = true;
                        }
                        break;
                    }

                    // printing matrix
                    PrintMatrix();
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

            if (Tetris.key.Key == ConsoleKey.LeftArrow & isKeyPressed)
            {
                direction = -1;
            }
            else if (Tetris.key.Key == ConsoleKey.RightArrow & isKeyPressed)
            {
                direction = 1;
            }
        }

        private static void NextBlock(bool[,] nextPiece)
        {
            bool[,] matrixNext = new bool[4, 6];
            for (int row = 0; row < matrixNext.GetLength(0); row++)
            {
                for (int col = 0; col < matrixNext.GetLength(1); col++)
                {
                    matrixNext[row, col] = false;
                }
            }

            for (int row = 0; row < nextPiece.GetLength(0); row++)
            {
                for (int col = 0; col < nextPiece.GetLength(1); col++)
                {
                    matrixNext[row, 2 + col] = nextPiece[row, col];
                }
            }

            for (int row = 0; row < matrixNext.GetLength(0); row++)
            {
                for (int col = 0; col < matrixNext.GetLength(1); col++)
                {
                    Console.SetCursorPosition(col + 38, row + 11);
                    Console.Write(matrixNext[row, col] ? block : " ");
                }
            }
        }

        private static void SettingPieceInMatrix()
        {
            for (int row = 0; row < curPiece.GetLength(0); row++)
            {
                for (int col = 0; col < curPiece.GetLength(1); col++)
                {
                    if (curPiece[row, col])
                        matrix[curX + row, curY + col] = curPiece[row, col];
                }
            }
        }

        private static void FillMatrix()
        {
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    matrix[row, col] = false;
                }
            }
        }

        private static void PrintMatrix()
        {
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    Console.SetCursorPosition(col + 1, row + 1);
                    Console.Write(matrix[row, col] ? block : " ");
                }
            }
        }

        private static bool tryMove(bool[,] piece, int newX, int newY)
        {
            if (!Collision(piece, newX, newY)) return false;

            // deleting previous position
            for (int row = 0; row < piece.GetLength(0); row++)
            {
                for (int col = 0; col < piece.GetLength(1); col++)
                {
                    matrix[curX + row, curY + col] = false;
                }
            }

            curX = newX;
            curY = newY;

            // adding piece on the new position
            SettingPieceInMatrix();
            
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

        private static bool[,] Rotate(bool[,] piece, bool left)
        {
            int dim = piece.GetLength(0);
            bool[,] rPiece= new bool[dim, dim];

            for (int i = 0; i < dim; i++)
                for (int j = 0; j < dim; j++)
                {
                    if (left)
                        rPiece[j, i] = piece[i, dim - 1 - j];
                    else
                        rPiece[j, i] = piece[dim - 1 - i, j];
                }

            return rPiece;
        }

        private static bool[,] PickRandomBlock(List<bool[,]> blocks)
        {
            int index = rnd.Next(0, blocks.Count);
            var block = blocks[index];
            return block;
        }
    }
}
