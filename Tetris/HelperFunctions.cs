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

        public static bool[,] PickRandomBlock(List<bool[,]> blocks, Random rnd)
        {
            int index = rnd.Next(0, blocks.Count);
            var block = blocks[index];
            return block;
        }

    }
}
