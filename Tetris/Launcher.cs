using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Security.Cryptography;
using System.Threading;
using System.Timers;
using System.Media;
using System.IO;

namespace Tetris
{ 
    public class Launcher
    {
        private const string TetrisSign = @" 
 ______   ______    ______   ______    __    ______
/\__  _\ /\  ___\  /\__  _\ /\  == \  /\ \  /\  ___\
\/_/\ \/ \ \  __\  \/_/\ \/ \ \  __<  \ \ \ \ \___  \
   \ \_\  \ \_____\   \ \_\  \ \_\ \_\ \ \_\ \/\_____\
    \/_/   \/_____/    \/_/   \/_/ /_/  \/_/  \/_____/";
        private const string VerticalLine = "\u2551";
        private const string HorizontalLine = "\u2550";
        private const string TopLeftCorner = "\u2554";
        private const string TopRightCorner = "\u2557";
        private const string BottomLeftCorner = "\u255A";
        private const string BottomRightCorner = "\u255D";
        public static void Main()
        {
            ConsoleSize();
            MainMenu();
        }

        private static void ConsoleSize()
        {
            Console.WindowHeight = 25;
            Console.BufferHeight = 25;
            Console.WindowWidth = 55;
            Console.BufferWidth = 55;
        }

        public static void MainMenu()
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
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(TetrisSign);

                int current = 1;
                Console.CursorTop = 10;
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
                    Console.CursorLeft = 25;
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
                        Environment.Exit(0);
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
                //Tetris.StartGame();
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

            Console.SetCursorPosition(Console.WindowWidth / 2 - 7, 0);
            Console.WriteLine("HOW TO PLAY ");
            Console.WriteLine(TetrisSign);
            Console.SetCursorPosition(10, 8);
            Console.WriteLine("Controls:");

            string[] controls = new string[4];
            controls[0] = "[Left Arrow]     Slide Left";
            controls[1] = "[Right Arrow]    Slide Right";
            controls[2] = "[Down Arrow]     Fall block faster";
            controls[3] = "[Spacebar]       Rotate to 90°";
            foreach (var control in controls)
            {
                Console.CursorLeft = 10;
                Console.WriteLine(control);
            }
            Console.SetCursorPosition(0, 14);
            string instructions = "Tetris is played on a 24 by 22 grid called the Matrix. Shapes called Blocks fall from the top of the Matrix and come to rest at the bottom.Only one Block falls at a time.At first the Tetriminos fall rather slowly; as the game progresses they will fall faster and faster.The primary way to score points in Tetris is to clear lines by manipulating the pieces so that they fill horizontal row within the Matrix.";
            Console.WriteLine(instructions);
            Console.SetCursorPosition(Console.WindowWidth / 2 - 15, 23);

            Console.WriteLine("Press any key to get back...");
            Console.ReadKey();
        }

        public static void DrawBorder()
        {
            SoundPlayer sp = new SoundPlayer();
            sp.SoundLocation = "../../Sounds/ingameSound.wav";
            sp.PlayLooping();

            Console.CursorVisible = false;
            Console.Clear();
            
            //Main field vertical lines
            for (int height = 1; height <= 22; height++)
            {
                Console.SetCursorPosition(0, height);
                Console.Write(VerticalLine);
                Console.SetCursorPosition(25, height);
                Console.Write(VerticalLine);
            }
            //Main field corners
            Console.SetCursorPosition(0, 0);
            Console.Write(TopLeftCorner);
            Console.SetCursorPosition(25, 0);
            Console.Write(TopRightCorner);
            Console.SetCursorPosition(0, 23);
            Console.Write(BottomLeftCorner);
            Console.SetCursorPosition(25, 23);
            Console.Write(BottomRightCorner);

            //Main field horizontal lines
            Console.SetCursorPosition(1, 0);
            for (int width = 0; width <= 23; width++)
            {
                Console.Write(HorizontalLine);
            }
            Console.SetCursorPosition(1, 23);
            for (int width = 0; width <= 23; width++)
            {
                Console.Write(HorizontalLine);
            }
            
            //Next block vertical lines
            Console.SetCursorPosition(30,9);
            for (int height = 0; height < 6; height++)
            {
                Console.SetCursorPosition(30, 9 + height);
                Console.Write(VerticalLine);
                Console.SetCursorPosition(39, 9 + height);
                Console.Write(VerticalLine);
            }

            //Next block corners + horizontal lines
            Console.SetCursorPosition(30, 9);
            Console.Write(TopLeftCorner);
            for (int width = 0; width < 8; width++)
            {
                Console.Write(HorizontalLine);
            }
            Console.Write(TopRightCorner);
            Console.SetCursorPosition(30, 15);
            Console.Write(BottomLeftCorner);
            for (int width = 0; width < 8; width++)
            {
                Console.Write(HorizontalLine);
            }
            Console.Write(BottomRightCorner);
            
            //Next Block text
            Console.SetCursorPosition(33, 10);
            Console.Write("NEXT");

            //Are you ready text
            Console.SetCursorPosition(6, 10);
            Console.WriteLine("Are you ready?");
            Console.CursorLeft = 6;
            Console.Write("(Y/N)?  ");
            string playerDecision = Console.ReadLine();

            if (playerDecision.ToLower().Equals("y"))
            {
                Tetris.StartGame();
            }
            else
            {
                MainMenu();
            }
        }
    }
}
