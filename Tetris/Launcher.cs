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
        public static List<KeyValuePair<string, int>> highScores = new List<KeyValuePair<string, int>>();
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
        private static bool HasMusic = true;

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

            if (HasMusic) sp.PlayLooping();

            string[] menuItems = new string[6]
            {
                "PLAY",
                "HIGHSCORES",
                "MUSIC ON",
                "HELP",
                "CREDITS",
                "EXIT"
            };
            int pageSize = 6;
            int pointer = 1;
            while (true)
            {
                if (HasMusic) menuItems[2] = "MUSIC ON";
                else menuItems[2] = "MUSIC OFF";

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
                    Console.CursorLeft = 10;
                    Console.WriteLine(item);
                }
                 

                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        ShowOtherMenu(pointer, sp);
                        break;
                    case ConsoleKey.UpArrow:
                        if (pointer > 1)
                        {
                            pointer--;
                        }
                        else if (pointer <= 1)
                        {
                            pointer = 6;
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
            }
            else if (currentSelection == 2)
            {
                DrawHighScores();
            }
            else if (currentSelection == 3)
            {
                if (!HasMusic)
                {
                    sp.PlayLooping();
                    HasMusic = true;
                }
                else
                {
                    sp.Stop();
                    HasMusic = false;
                }
            }
            else if (currentSelection == 4)
            {
                DrawHelp();
            }
            else if (currentSelection == 5)
            {
                DrawCredits();
            }
            else if (currentSelection == 6)
            {
                sp.Stop();
                Environment.Exit(0);
            }
        }

        private static void DrawCredits()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(TetrisSign);

            string[] about = new string[]
            {
                "SoftUni Team Project",
                "Console Tetris v0.01",
                "",
                "People worked on the project:",
                "",
                "Aleksandar Angelov",
                "Martin Todorov",
                "Emil Mihaylov",
                "Vladislav Mitrov",
                "Nikolay Bonev"

            };
            Console.CursorTop = 9;
            foreach (var item in about)
            {
                Console.CursorLeft = (Console.WindowHeight / 2);
                Console.WriteLine(item);
            }
            

            Console.SetCursorPosition(Console.WindowWidth / 2 - 15, 23);
            Console.WriteLine("Press any key to get back...");
            Console.ReadKey();
        }

        private static void DrawHighScores()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(TetrisSign);
            string scores = "HIGH SCORES";
            Console.SetCursorPosition(Console.WindowWidth / 2 - scores.Length / 2, 9);
            Console.Write(scores);
            int counter = 1;
            Console.CursorTop = 11;
            foreach (var score in highScores.OrderByDescending(x => x.Value))
            {
                Console.CursorLeft = 14;
                Console.WriteLine(string.Format($"{counter}.\t{score.Value}\t{score.Key}"));
                counter++;
            }
            Console.SetCursorPosition(Console.WindowWidth / 2 - 15, 23);

            Console.WriteLine("Press any key to get back...");
            Console.ReadKey();
        }

        public static void FillHighScores(int score, string name)
        {
            highScores.Add(new KeyValuePair<string, int>(name, score));
        }

        private static void DrawHelp()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            
            Console.WriteLine(TetrisSign);
            Console.SetCursorPosition(10, 7);
            Console.WriteLine("Controls:");

            string[] controls = new string[5];
            controls[0] = "[Left Arrow]     Slide Left";
            controls[1] = "[Right Arrow]    Slide Right";
            controls[2] = "[Down Arrow]     Fall block faster";
            controls[3] = "[Z]              Rotate left";
            controls[4] = "[X]              Rotate right";
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
            if (HasMusic) sp.PlayLooping();

            Console.CursorVisible = false;
            Console.Clear();
            
            //Main field vertical lines
            for (int height = 1; height <= 22; height++)
            {
                Console.SetCursorPosition(0, height);
                Console.Write(VerticalLine);
                Console.SetCursorPosition(11, height);
                Console.Write(VerticalLine);
            }
            //Main field corners
            Console.SetCursorPosition(0, 0);
            Console.Write(TopLeftCorner);
            Console.SetCursorPosition(11, 0);
            Console.Write(TopRightCorner);
            Console.SetCursorPosition(0, 23);
            Console.Write(BottomLeftCorner);
            Console.SetCursorPosition(11, 23);
            Console.Write(BottomRightCorner);

            //Main field horizontal lines
            Console.SetCursorPosition(1, 0);
            for (int width = 0; width <= 9; width++)
            {
                Console.Write(HorizontalLine);
            }
            Console.SetCursorPosition(1, 23);
            for (int width = 0; width <= 9; width++)
            {
                Console.Write(HorizontalLine);
            }
            
            //Next block vertical lines
            for (int height = 0; height < 6; height++)
            {
                Console.SetCursorPosition(36, 9 + height);
                Console.Write(VerticalLine);
                Console.SetCursorPosition(45, 9 + height);
                Console.Write(VerticalLine);
            }

            //Next block corners + horizontal lines
            Console.SetCursorPosition(36, 9);
            Console.Write(TopLeftCorner);
            for (int width = 0; width < 8; width++)
            {
                Console.Write(HorizontalLine);
            }
            Console.Write(TopRightCorner);
            Console.SetCursorPosition(36, 15);
            Console.Write(BottomLeftCorner);
            for (int width = 0; width < 8; width++)
            {
                Console.Write(HorizontalLine);
            }
            Console.Write(BottomRightCorner);
            
            //Next Block text
            Console.SetCursorPosition(39, 10);
            Console.Write("NEXT");

            //Score field corners + horizontal lines
            Console.SetCursorPosition(33, 0);
            Console.Write(TopLeftCorner);
            for (int width = 0; width < 14; width++)
            {
                Console.Write(HorizontalLine);
            }
            Console.Write(TopRightCorner);
            Console.SetCursorPosition(33, 3);
            Console.Write(BottomLeftCorner);
            for (int width = 0; width < 14; width++)
            {
                Console.Write(HorizontalLine);
            }
            Console.Write(BottomRightCorner);

            //Score field vertical lines
            Console.SetCursorPosition(30, 1);
            for (int heitgh = 0; heitgh < 2; heitgh++)
            {
                Console.SetCursorPosition(33, 1 + heitgh);
                Console.Write(VerticalLine);
                Console.SetCursorPosition(48, 1 + heitgh);
                Console.Write(VerticalLine);
            }
            //Score field text
            Console.SetCursorPosition(38, 1);
            Console.Write("SCORES");

            //Level field corners + horizontal lines
            Console.SetCursorPosition(30, 4);
            Console.Write(TopLeftCorner);
            for (int width = 0; width < 7; width++)
            {
                Console.Write(HorizontalLine);
            }
            Console.Write(TopRightCorner);

            Console.SetCursorPosition(30, 8);
            Console.Write(BottomLeftCorner);
            for (int i = 0; i < 7; i++)
            {
                Console.Write(HorizontalLine);
            }
            Console.Write(BottomRightCorner);

            //Level field vertical lines
            for (int height = 0; height < 3; height++)
            {
                Console.SetCursorPosition(30, 5 + height);
                Console.Write(VerticalLine);
                Console.SetCursorPosition(38, 5 + height);
                Console.Write(VerticalLine);
            }
            //Level field text
            Console.SetCursorPosition(32, 5);
            Console.Write("LEVEL");

            //Lines field corners + horizontal lines
            Console.SetCursorPosition(43, 4);
            Console.Write(TopLeftCorner);
            for (int width = 0; width < 7; width++)
            {
                Console.Write(HorizontalLine);
            }
            Console.Write(TopRightCorner);

            Console.SetCursorPosition(43, 8);
            Console.Write(BottomLeftCorner);
            for (int i = 0; i < 7; i++)
            {
                Console.Write(HorizontalLine);
            }
            Console.Write(BottomRightCorner);

            //Lines field vertical lines
            for (int height = 0; height < 3; height++)
            {
                Console.SetCursorPosition(43, 5 + height);
                Console.Write(VerticalLine);
                Console.SetCursorPosition(51, 5 + height);
                Console.Write(VerticalLine);
            }
            //Lines field text
            Console.SetCursorPosition(45, 5);
            Console.Write("LINES");

            //Are you ready text
            Console.SetCursorPosition(3, 10);
            Console.WriteLine("Ready?");
            Console.CursorLeft = 3;
            Console.Write("(Y/N)? ");
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
