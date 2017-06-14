using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public static class HelperFunctions
    {
        public static string Block = "■";

        public static void InputEvents()
        {
            if (Console.KeyAvailable)
            {
                Tetris.key = Console.ReadKey();
                Tetris.isKeyPressed = true;
            }
            else Tetris.isKeyPressed = false;

            if (Tetris.isKeyPressed)
            {
                // move left
                if (Tetris.key.Key == ConsoleKey.LeftArrow & Tetris.isKeyPressed)
                {
                    if (HelperFunctions.TryMove(Tetris.matrix, Tetris.curPiece, Tetris.curX, Tetris.curY - 1, "left"))
                    {
                        HelperFunctions.ReplacePosition(Tetris.curPiece, Tetris.curX, Tetris.curY - 1);
                    }
                }
                // move right
                else if (Tetris.key.Key == ConsoleKey.RightArrow & Tetris.isKeyPressed)
                {
                    if (HelperFunctions.TryMove(Tetris.matrix, Tetris.curPiece, Tetris.curX, Tetris.curY + 1, "right"))
                    {
                        HelperFunctions.ReplacePosition(Tetris.curPiece, Tetris.curX, Tetris.curY + 1);
                    }
                }
                // rotate to the left
                else if (Tetris.key.Key == ConsoleKey.Z)
                {
                    bool[,] tempPiece = HelperFunctions.RotateMatrixCounterClockwise(Tetris.curPiece);
                    RotateIfPossible(tempPiece);
                }
                // rotate to the right
                else if (Tetris.key.Key == ConsoleKey.X)
                {
                    bool[,] tempPiece = HelperFunctions.RotateMatrixClockwise(Tetris.curPiece);
                    RotateIfPossible(tempPiece);
                }
                // fall down
                else if(Tetris.key.Key == ConsoleKey.DownArrow)
                {
                    Tetris.Speed = 20;
                }
            }
            Tetris.isKeyPressed = false;
        }

        private static void RotateIfPossible(bool[,] tempPiece)
        {
            HelperFunctions.DeletePreviousPosition(Tetris.curPiece, Tetris.matrix, Tetris.curX, Tetris.curY);
            if (HelperFunctions.IsPossibleToRotate(tempPiece, Tetris.matrix, Tetris.curX, Tetris.curY))
                Tetris.curPiece = tempPiece;
            HelperFunctions.SettingPieceInMatrix(Tetris.curPiece, Tetris.matrix, Tetris.curX, Tetris.curY);
        }

        public static void FillMatrix(bool[,] matrixToFill)
        {
            for (int row = 0; row < matrixToFill.GetLength(0); row++)
            {
                for (int col = 0; col < matrixToFill.GetLength(1); col++)
                {
                    matrixToFill[row, col] = false;
                }
            }
        }

        public static void PrintMatrix(bool[,] matrixToPrint, int left, int top)
        {
            for (int row = 0; row < matrixToPrint.GetLength(0); row++)
            {
                for (int col = 0; col < matrixToPrint.GetLength(1); col++)
                {
                    Console.SetCursorPosition(col + left, row + top);
                    Console.Write(matrixToPrint[row, col] ? Block : " ");
                }
            }
        }

        public static void DeletePreviousPosition(bool[,] piece, bool[,] matrixToDeleteFrom, int curX, int curY)
        {
            for (int row = 0; row < piece.GetLength(0); row++)
            {
                for (int col = 0; col < piece.GetLength(1); col++)
                {
                    matrixToDeleteFrom[curX + row, curY + col] = false;
                }
            }
        }

        public static void SettingPieceInMatrix(bool[,] piece, bool[,] matrixToAddTo, int curX, int curY)
        {
            for (int row = 0; row < piece.GetLength(0); row++)
            {
                for (int col = 0; col < piece.GetLength(1); col++)
                {
                    if (piece[row, col])
                        matrixToAddTo[curX + row, curY + col] = piece[row, col];
                }
            }
        }

        public static void ReplacePosition(bool[,] piece, int newX, int newY)
        {
            // deleting previous position
            HelperFunctions.DeletePreviousPosition(piece, Tetris.matrix, Tetris.curX, Tetris.curY);

            // setting the new coordinates of the current piece
            Tetris.curX = newX;
            Tetris.curY = newY;

            // adding piece on the new position
            HelperFunctions.SettingPieceInMatrix(piece, Tetris.matrix, Tetris.curX, Tetris.curY);
        }

        public static void NextBlock(bool[,] nextPiece)
        {
            bool[,] matrixNext = new bool[4, 6];
            HelperFunctions.FillMatrix(matrixNext);

            HelperFunctions.SettingPieceInMatrix(nextPiece, matrixNext, 0, 2);

            HelperFunctions.PrintMatrix(matrixNext, 38, 11);
        }

        //public static bool[,] Rotate(bool[,] piece, bool left)
        //{
        //    int dim = piece.GetLength(0);
        //    bool[,] rPiece= new bool[dim, dim];

        //    for (int i = 0; i < dim; i++)
        //        for (int j = 0; j < dim; j++)
        //        {
        //            if (left)
        //                rPiece[j, i] = piece[i, dim - 1 - j];
        //            else
        //                rPiece[j, i] = piece[dim - 1 - i, j];
        //        }

        //    return rPiece;
        //}

        public static bool TryMove(bool[,] matrix, bool[,] piece, int newX, int newY, string direction)
        {
            switch (direction)
            {
                case "down":
                    if (!HelperFunctions.CollisionDown(piece, newX, newY, matrix)) return false;
                    break;

                case "left":
                    if (!HelperFunctions.CollisionLeft(piece, newX, newY, matrix)) return false;
                    break;

                case "right":
                    if (!HelperFunctions.CollisionRight(piece, newX, newY, matrix)) return false;
                    break;
            }

            return true;
        }

        public static bool[,] RotateMatrixCounterClockwise(bool[,] oldMatrix)
        {
            bool[,] newMatrix = new bool[oldMatrix.GetLength(1), oldMatrix.GetLength(0)];
            int newColumn, newRow = 0;
            for (int oldColumn = oldMatrix.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
            {
                newColumn = 0;
                for (int oldRow = 0; oldRow < oldMatrix.GetLength(0); oldRow++)
                {
                    newMatrix[newRow, newColumn] = oldMatrix[oldRow, oldColumn];
                    newColumn++;
                }
                newRow++;
            }
            return newMatrix;
        }

        public static bool[,] RotateMatrixClockwise(bool[,] oldMatrix)
        {
            bool[,] newMatrix = new bool[oldMatrix.GetLength(1), oldMatrix.GetLength(0)];
            int newColumn, newRow = 0;
            for (int oldColumn = 0; oldColumn < oldMatrix.GetLength(1); oldColumn++)
            {
                newColumn = 0;
                for (int oldRow = oldMatrix.GetLength(0) - 1; oldRow >= 0; oldRow--)
                {
                    newMatrix[newRow, newColumn] = oldMatrix[oldRow, oldColumn];
                    newColumn++;
                }
                newRow++;
            }
            return newMatrix;
        }

        public static bool CollisionRight(bool[,] piece, int newX, int newY, bool[,] matrix)
        {
            // check if end of frame
            var currentFigureLowestCol = newY + piece.GetLength(1);
            if (currentFigureLowestCol > matrix.GetLength(1))
                return false;

            //collision with blocks
            for (int col = piece.GetLength(1) - 2; col >= 0; col--)
            {
                for (int row = 0; row < piece.GetLength(0); row++)
                {
                    if (!piece[row, col + 1]
                        && matrix[newX + row, newY + col])
                        return false;
                }
            }

            for (int row = 0; row < piece.GetLength(0); row++)
            {
                if (piece[row, piece.GetLength(1) - 1]
                    && matrix[newX + row, newY + piece.GetLength(1) - 1])
                    return false;
            }

            return true;
        }

        public static bool CollisionLeft(bool[,] piece, int newX, int newY, bool[,] matrix)
        {
            // check if end of frame
            if (newY < 0) return false;

            //collision with blocks
            for (int col = 1; col < piece.GetLength(1); col++)
            {
                for (int row = 0; row < piece.GetLength(0); row++)
                {
                    if (!piece[row, col - 1]
                        && matrix[newX + row, newY + col])
                        return false;
                }
            }

            for (int row = 0; row < piece.GetLength(0); row++)
            {
                if (piece[row, 0]
                    && matrix[newX + row, newY])
                    return false;
            }

            return true;
        }

        public static bool CollisionDown(bool[,] piece, int newX, int newY, bool[,] matrix)
        {
            // check if end of frame
            var currentFigureLowestRow = newX + piece.GetLength(0);
            if (currentFigureLowestRow > matrix.GetLength(0))
                return false;

            //collision with blocks
            for (int row = 0; row < piece.GetLength(0) - 1; row++)
            {
                for (int col = 0; col < piece.GetLength(1); col++)
                {
                    if (!piece[row + 1, col]
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

        public static bool IsPossibleToRotate(bool[,] piece, bool[,] matrix, int curX, int curY)
        {
            // check if end of right frame
            var currentFigureLowestCol = curY + piece.GetLength(1);
            if (currentFigureLowestCol > matrix.GetLength(1))
                return false;

            // check if end of left frame
            if (curY < 0) return false;

            // check if end of frame
            var currentFigureLowestRow = curX + piece.GetLength(0);
            if (currentFigureLowestRow > matrix.GetLength(0))
                return false;

            for (int row = 0; row < piece.GetLength(0); row++)
            {
                for (int col = 0; col < piece.GetLength(1); col++)
                {
                    if (piece[row, col]
                        && matrix[curX + row, curY + col])
                        return false;
                }
            }

            return true;
        }

        public static bool[,] PickRandomBlock(List<bool[,]> blocks, Random rnd)
        {
            int index = rnd.Next(0, blocks.Count);
            var block = blocks[index];
            return block;
        }

        public static void AskForRestart()
        {
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
    }
}
