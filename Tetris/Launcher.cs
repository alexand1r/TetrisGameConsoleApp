using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tetris
{
    public class Launcher
    {
        public static void Main()
        {
            ConsoleSize();
            MainMenu();
            //DrawBorder();
        }

        private static void ConsoleSize()
        {
            Console.WindowHeight = 25;
            Console.BufferHeight = 25;
            Console.WindowWidth = 40;
            Console.BufferWidth = 40;
        }

        private static void MainMenu()
        {
            Console.SetCursorPosition(0, 1);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(new string('-', 40));
            Console.Write(new string('-', 17));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("TETRIS");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(new string('-', 17));
            Console.WriteLine(new string('-', 40));
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, Console.WindowHeight / 3);
            Console.Write(new string(' ', (Console.WindowWidth / 2 - 2)));
            Console.WriteLine("PLAY\r\n");
            Console.Write(new string(' ', (Console.WindowWidth / 2 - 2)));
            Console.WriteLine("HELP\r\n");
            Console.Write(new string(' ', (Console.WindowWidth / 2 - 2)));
            Console.WriteLine("EXIT\r\n");
        }
        private static void DrawBorder()
        {
            Console.Clear();
            Console.SetCursorPosition(0,0);
            Console.Write("\u2554");
            Console.SetCursorPosition(25,0);
            Console.Write("\u2557");
            for (int height = 1; height <= 22; height++)
            {
                Console.SetCursorPosition(0, height);
                Console.Write("\u2551");
                Console.SetCursorPosition(25, height);
                Console.Write("\u2551");
            }
            Console.SetCursorPosition(0, 23);
            Console.Write("\u255A");
            Console.SetCursorPosition(25, 23);
            Console.Write("\u255D");

            Console.SetCursorPosition(1, 0);
            for (int width = 0; width <=23; width++)
            {
                Console.Write("\u2550");
            }
            Console.SetCursorPosition(1, 23);
            for (int width = 0; width <= 23; width++)
            {
                Console.Write("\u2550");
            }
            Console.WriteLine();
        }
    }
}
