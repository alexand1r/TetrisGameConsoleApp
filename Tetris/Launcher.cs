using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.IO;

namespace Tetris
{
    public class Launcher
    {
        public static void Main()
        {
            ConsoleSize();
            MainMenu();
            DrawBorder();
            StartGame();
        }

        private static void StartGame()
        {
            var blocks = Blocks.createBlocks();
            string[][] matrix = new string[22][];
            for (int row = 0; row < matrix.Length; row++)
            {
                matrix[row] = new string[24];
                for (int col = 0; col < matrix[row].Length; col++)
                {
                    matrix[row][col] = ".";
                }
            }
            for (int i = 0; i < matrix.Length; i++)
            {
                Console.SetCursorPosition(1, i + 1);
                Console.WriteLine(string.Join("", matrix[i]));
            }

            var key = Console.ReadKey();
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
            SoundPlayer sp = new SoundPlayer();
            sp.SoundLocation = "../../Sounds/mainMenu.wav";
            sp.PlayLooping();
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
                        ShowOtherMenu(pointer,sp);
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

        private static void ShowOtherMenu(int currentSelection, SoundPlayer sp)
        {
            if (currentSelection == 1)
            {
                sp.Stop();
                DrawBorder();
            }
            else if(currentSelection == 2)
            {
                DrawHelp();
            }
            else if (currentSelection == 3)
            {
                sp.Stop();
                Environment.Exit(0);
            }
        }

        private static void DrawHelp()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(Console.WindowWidth / 2 - 9, 1);
            Console.WriteLine("How to play Tetris");
            Console.WriteLine(new string('=',40));
            Console.SetCursorPosition(4, 4);
            Console.WriteLine("Controls:");

            string[] controls = new string[4];
            controls[0] = "[Left Arrow]     Slide Left";
            controls[1] = "[Right Arrow]    Slide Right";
            controls[2] = "[Down Arrow]     Fall block faster";
            controls[3] = "[Spacebar]       Rotate to 90°";
            foreach (var control in controls)
            {
                Console.CursorLeft = 4;
                Console.WriteLine(control);
            }
            Console.SetCursorPosition(0, 10);
            string instructions = "Tetris is played on a 24 by 22 grid called the Matrix. Shapes called Blocks fall from the top of the Matrix and come to rest at the bottom.Only one Block falls at a time.At first the Tetriminos fall rather slowly; as the game progresses they will fall faster and faster.The primary way to score points in Tetris is to clear lines by manipulating the pieces so that they fill horizontal row within the Matrix.";
            Console.WriteLine(instructions);
            Console.SetCursorPosition(Console.WindowWidth / 2 - 15, 23);

            Console.WriteLine("Press any key to get back...");
            Console.ReadKey();
        }

        private static void DrawBorder()
        {
            SoundPlayer sp = new SoundPlayer();
            sp.SoundLocation = "../../Sounds/ingameSound.wav";
            sp.PlayLooping();

            Console.CursorVisible = false;
            Console.Clear();
            Console.SetCursorPosition(0, 0);
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
            for (int width = 0; width <= 23; width++)
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
