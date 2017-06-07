using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tetris
{
    public class Tetris
    {
        private static string[][] curPiece;
        private static string[][] matrix;
        private static int curX;
        private static int curY;
        private static Random rnd = new Random();
        private static Stack<string[][]> pieces = new Stack<string[][]>();
        private static bool gameOver;

        public static void StartGame()
        {
            gameOver = false;
            var blocks = Blocks.createBlocks();
            var names = new string[] { "o", "i", "s", "z", "l", "j", "t" };
            matrix = new string[22][];

            // filling matrix
            FillMatrix();

            while (true)
            {
                if (gameOver) break;
                
                string[][] newPiece;
                if (pieces.Count == 0)
                    newPiece = PickRandomBlock(blocks, names);
                else
                    newPiece = pieces.Pop();

                pieces.Push(PickRandomBlock(blocks, names));
                NextBlock(pieces.Peek());

                curPiece = newPiece;
                curX = 0;
                curY = matrix.GetLength(0) / 2 + 1;
                
                // setting the new piece on the top middle
                SettingPieceInMatrix();

                // printing matrix
                PrintMatrix();

                while (true)
                {
                    Thread.Sleep(200);
                    // trying to move the piece 1 row down
                    if (!tryMove(curPiece, curX + 1, curY))
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
                }
            }
            Console.WriteLine("   Restart: Y/N");
            var key = Console.ReadKey(true);
            switch (key.Key)
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

        private static void NextBlock(string[][] nextPiece)
        {
            int row = 0;
            
            string[][] matrix = new string[4][];
            for (int i = 0; i < matrix.Length; i++)
            {
                matrix[i] = new string[6];
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    matrix[i][j] = string.Empty;
                }
            }
            int clearRow = 0;
            foreach (var lines in matrix)
            {
                Console.SetCursorPosition(32, 11 + clearRow);
                Console.Write(string.Join(" ", lines));
                clearRow++;
            }

            foreach (var line in nextPiece)
            {
                Console.SetCursorPosition(34, 11 + row);
                Console.Write(string.Join("", line));
                row++;
            }
        }

        private static void SettingPieceInMatrix()
        {
            for (int row = 0; row < curPiece.Length; row++)
            {
                for (int col = 0; col < curPiece[row].Length; col++)
                {
                    matrix[curX + row][curY + col] = curPiece[row][col];
                }
            }
        }

        private static void FillMatrix()
        {
            for (int row = 0; row < matrix.Length; row++)
            {
                matrix[row] = new string[24];
                for (int col = 0; col < matrix[row].Length; col++)
                {
                    matrix[row][col] = " ";
                }
            }
        }

        private static void PrintMatrix()
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                Console.SetCursorPosition(1, i + 1);
                Console.WriteLine(string.Join("", matrix[i]));
            }
        }

        private static bool tryMove(string[][] piece, int newX, int newY)
        {
            if (!Collision(piece, newX, newY)) return false;

            // deleting previous position
            for (int row = 0; row < piece.Length; row++)
            {
                for (int col = 0; col < piece[row].Length; col++)
                {
                    matrix[curX + row][curY + col] = " ";
                }
            }

            curX = newX;
            curY = newY;

            // adding piece on the new position
            SettingPieceInMatrix();
            
            return true;
        }

        private static bool Collision(string[][] piece, int newX, int newY)
        {
            // check if end of frame
            if (newX + piece.GetLength(0) > matrix.GetLength(0))
            {
                return false;
            }

            // collision - needs fixing
            for (int row = 0; row < piece.Length; row++)
            {
                for (int col = 0; col < piece[row].Length; col++)
                {
                    if (matrix[newX + piece.Length - 1][newY + col] != " ")
                        return false;
                }
            }

            return true;
        }

        private static string[][] PickRandomBlock(Dictionary<string, string[][]> blocks, string[] names)
        {
            int index = rnd.Next(0,7);
            var block = blocks[names[index]];
            return block;
        }   
    }
}
