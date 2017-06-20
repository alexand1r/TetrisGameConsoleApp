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

        /////////////////////////////////////////////////////////// --- CASE EVENTS --- /////////////////////////////////////////////////////////////

        public static void InputEvents()
        {
            Console.SetCursorPosition(0, 24);
            if (Console.KeyAvailable)
            {
                Tetris.key = Console.ReadKey(true);
                Tetris.isKeyPressed = true;
            }
            else Tetris.isKeyPressed = false;

            if (Tetris.isKeyPressed)
            {
                // move left
                if (Tetris.key.Key == ConsoleKey.LeftArrow & Tetris.isKeyPressed)
                {
                    if (TryMove(Tetris.matrix, Tetris.curPiece, Tetris.curX, Tetris.curY - 1, "left"))
                    {
                        ReplacePosition(Tetris.curPiece, Tetris.curX, Tetris.curY - 1);
                    }
                }
                // move right
                else if (Tetris.key.Key == ConsoleKey.RightArrow & Tetris.isKeyPressed)
                {
                    if (TryMove(Tetris.matrix, Tetris.curPiece, Tetris.curX, Tetris.curY + 1, "right"))
                    {
                        ReplacePosition(Tetris.curPiece, Tetris.curX, Tetris.curY + 1);
                    }
                }
                // rotate to the left
                else if (Tetris.key.Key == ConsoleKey.Z)
                {
                    bool[,] tempPiece = RotateMatrixCounterClockwise(Tetris.curPiece);
                    RotateIfPossible(tempPiece);
                }
                // rotate to the right
                else if (Tetris.key.Key == ConsoleKey.X)
                {
                    bool[,] tempPiece = RotateMatrixClockwise(Tetris.curPiece);
                    RotateIfPossible(tempPiece);
                }
                // fall down
                else if(Tetris.key.Key == ConsoleKey.DownArrow)
                {
                    Tetris.Speed = 20;
                }
                // pause
                else if (Tetris.key.Key == ConsoleKey.P)
                {
                    Console.SetCursorPosition(45, 23);
                    Console.WriteLine("paused");
                    ConsoleKeyInfo key;
                    do
                    {
                        key = Console.ReadKey(true);
                    } while (key.Key != ConsoleKey.P);
                    Console.SetCursorPosition(45, 23);
                    Console.WriteLine(new string(' ', 6));
                }
            }
            Tetris.isKeyPressed = false;
        }

        public static bool TryMove(bool[,] matrix, bool[,] piece, int newX, int newY, string direction)
        {
            switch (direction)
            {
                case "down":
                    if (!CollisionDown(piece, newX, newY, matrix)) return false;
                    break;

                case "left":
                    if (!CollisionLeft(piece, newX, newY, matrix)) return false;
                    break;

                case "right":
                    if (!CollisionRight(piece, newX, newY, matrix)) return false;
                    break;
            }

            return true;
        }

        /////////////////////////////////////////////////////// --- COLLISIONS --- /////////////////////////////////////////////////////////////

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
                    if (piece[row, col] &&
                        !piece[row + 1, col]
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
        private static void RotateIfPossible(bool[,] tempPiece)
        {
            DeletePreviousPosition(Tetris.curPiece, Tetris.matrix, Tetris.curX, Tetris.curY);
            if (IsPossibleToRotate(tempPiece, Tetris.matrix, Tetris.curX, Tetris.curY))
                Tetris.curPiece = tempPiece;
            SettingPieceInMatrix(Tetris.curPiece, Tetris.matrix, Tetris.curX, Tetris.curY);
        }

        //////////////////////////////////////////////////// --- MATRIX ACTIONS --- //////////////////////////////////////////////////////////////

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
                    if (piece[row, col])
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
            DeletePreviousPosition(piece, Tetris.matrix, Tetris.curX, Tetris.curY);

            // setting the new coordinates of the current piece
            Tetris.curX = newX;
            Tetris.curY = newY;

            // adding piece on the new position
            SettingPieceInMatrix(piece, Tetris.matrix, Tetris.curX, Tetris.curY);
        }

        ///////////////////////////////////////////////////////// --- NEXT BLOCK --- //////////////////////////////////////////////////////////

        public static void NextBlock(bool[,] nextPiece)
        {
            bool[,] matrixNext = new bool[4, 6];
            FillMatrix(matrixNext);

            SettingPieceInMatrix(nextPiece, matrixNext, 0, 2);

            PrintMatrix(matrixNext, 38, 11);
        }

        public static bool[,] PickRandomBlock(List<bool[,]> blocks, Random rnd)
        {
            int index = rnd.Next(0, blocks.Count);
            var block = blocks[index];
            return block;
        }

        ////////////////////////////////////////////////////////// --- ROTATIONS --- ////////////////////////////////////////////////////////////

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

        /////////////////////////////////////////////////// --- GAME ACTIONS AND ENDING --- /////////////////////////////////////////////////////////////

        public static void ClearLines()
        {
            for (int row = Tetris.matrix.GetLength(0) - 1; row > 0; row--)
            {
                int counter = 0;
                for (int col = 0; col < Tetris.matrix.GetLength(1); col++)
                {
                    if (Tetris.matrix[row, col])
                    {
                        counter++;
                    }
                    // clear the row
                    if (counter == Tetris.MATRIX_COLS)
                    {
                        Tetris.LineCleared++;
                        //aboutToBoomCounter = 0;
                        for (int i = 0; i < Tetris.MATRIX_COLS; i++)
                        {
                            Tetris.matrix[row, i] = false;
                        }
                        // fall down
                        FallDown(row);
                        // combo scores
                        Tetris.Combo++;
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
            if (Tetris.LineCleared < 5) { Tetris.Level = 1; Tetris.Speed = 250; }
            else if (Tetris.LineCleared < 10) { Tetris.Level = 2; Tetris.Speed = 230; }
            else if (Tetris.LineCleared < 15) { Tetris.Level = 3; Tetris.Speed = 210; }
            else if (Tetris.LineCleared < 25) { Tetris.Level = 4; Tetris.Speed = 190; }
            else if (Tetris.LineCleared < 35) { Tetris.Level = 5; Tetris.Speed = 170; }
            else if (Tetris.LineCleared < 50) { Tetris.Level = 6; Tetris.Speed = 150; }
            else if (Tetris.LineCleared < 70) { Tetris.Level = 7; Tetris.Speed = 130; }
            else if (Tetris.LineCleared < 90) { Tetris.Level = 8; Tetris.Speed = 110; }
            else if (Tetris.LineCleared < 110) { Tetris.Level = 9; Tetris.Speed = 90; }

            if (Tetris.Combo >= 1)
            {
                Tetris.Score += (Tetris.Combo * 50) * Tetris.Level;
                Tetris.Combo = 0;
            }
            else
            {
                Tetris.Score += 20;
            }
        }

        public static void FallDown(int row)
        {
            for (int column = 0; column < Tetris.matrix.GetLength(1); column++)
            {
                for (int curRow = row - 1; curRow >= 0; curRow--)
                {
                    if (Tetris.matrix[curRow, column])
                    {
                        Tetris.matrix[curRow + 1, column] = Tetris.matrix[curRow, column];
                        Tetris.matrix[curRow, column] = false;
                    }
                }
            }
        }
        
        public static void PrintStats()
        {
            Console.SetCursorPosition(39, 2);
            Console.Write(Tetris.Score);
            Console.SetCursorPosition(34, 6);
            Console.Write(Tetris.Level);
            Console.SetCursorPosition(47, 6);
            Console.Write(Tetris.LineCleared);
        }

        public static void AskForRestart()
        {
            Console.SetCursorPosition(36, 22);
            Console.Write("Restart: Y/N ");
            string playerDecision = Console.ReadLine();
            if (playerDecision.ToLower().Equals("y"))
            {
                Console.Clear();
                Launcher.DrawBorder();
                Tetris.StartGame();
            }
            else
            {
                Launcher.MainMenu();
            }
        }

        public static void GameOver()
        {
            Console.SetCursorPosition(36, 17);
            Console.Write("Game Over");
            Console.SetCursorPosition(36, 18);
            Console.WriteLine("Type your name: ");
            Console.SetCursorPosition(36, 19);
            string name = Console.ReadLine();
            Launcher.FillHighScores(Tetris.Score, name);
        }

        public static void DestroyWithBomb()
        {
            for (int row = Tetris.curX - 1; row <= Tetris.curX + 1; row++)
            {
                for (int col = Tetris.curY - 1; col <= Tetris.curY + 1; col++)
                {
                    if (row >= 0 && row < Tetris.MATRIX_ROWS && col >= 0 && col < Tetris.MATRIX_COLS)
                    {
                        Tetris.matrix[row, col] = false;
                    }
                }
            }
            Tetris.Score += 100;
        }

        public static void ChangeConsoleColor(bool[,] newPiece)
        {
            if (newPiece.GetLength(0) == 1 && newPiece.GetLength(1) == 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(38, 18);
                Console.Write("BOMB INCOMING!");
            }
            else
            {
                Console.SetCursorPosition(38, 18);
                Console.Write(new string(' ', 14));
            }
        }
    }
}
