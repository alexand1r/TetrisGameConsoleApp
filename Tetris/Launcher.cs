using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Security.Cryptography;
using System.Threading;
using System.Timers;

namespace Tetris
{ 
    public class Launcher
    {
        public static void Main()
        {
            ConsoleSize();
            MainMenu();
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
            string[] menuItems = new string[3]
            {
                "PLAY",
                "HELP",
                "EXIT"
            };
            int pageSize = 3;
            int pointer = 1;
            while (true)
            {
                Console.CursorVisible = false;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;

                Console.Clear();
                Console.SetCursorPosition(0, 1);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(new string('-', 40));
                Console.Write(new string('-', 17));
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("TETRIS");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(new string('-', 17));
                Console.WriteLine(new string('-', 40));
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Green;

                int current = 1;
                foreach (var item in menuItems)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    if (current == pointer)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    current++;
                    Console.CursorLeft = 17;
                    Console.WriteLine(item);
                }

                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        ShowOtherMenu(pointer);
                        break;
                    case ConsoleKey.UpArrow:
                        if (pointer > 1)
                        {
                            pointer--;
                        }
                        else if (pointer <= 1)
                        {
                            pointer = 3;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (pointer < pageSize)
                        {
                            pointer++;
                        }
                        else if (pointer >= pageSize)
                        {
                            pointer = 1;
                        }
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        private static void ShowOtherMenu(int currentSelection)
        {
            if (currentSelection == 1)
            {
                DrawBorder();
                Tetris.StartGame();
            }
            else if(currentSelection == 2)
            {
                DrawHelp();
            }
            else if (currentSelection == 3)
            {
                Environment.Exit(0);
            }
        }

        private static void DrawHelp()
        {
            throw new NotImplementedException();
        }

        public static void DrawBorder()
        {
            Console.Clear();
            Console.SetCursorPosition(0,0);
            Console.Write("\u2554");
            Console.SetCursorPosition(25, 0);
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
            Console.ReadLine();
        }
    }
}
