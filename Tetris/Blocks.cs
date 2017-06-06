using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public static class Blocks
    {
        public static Dictionary<string, string[][]> createBlocks()
        {
            Dictionary<string, string[][]> blocks = new Dictionary<string, string[][]>();

            string[][] o = new string[2][];
            o[0] = new string[2] { "o", "o" };
            o[1] = new string[2] { "o", "o" };
            blocks.Add("o", o);

            string[][] i = new string[4][];
            i[0] = new string[1] { "o" };
            i[1] = new string[1] { "o" };
            i[2] = new string[1] { "o" };
            i[3] = new string[1] { "o" };
            blocks.Add("i", i);

            string[][] s = new string[2][];
            s[0] = new string[3] { "", "o", "o" };
            s[1] = new string[3] { "o", "o", "" };
            blocks.Add("s", s);

            string[][] z = new string[2][];
            z[0] = new string[3] { "o", "o", "" };
            z[1] = new string[3] { "", "o", "o" };
            blocks.Add("z", z);

            string[][] l = new string[3][];
            l[0] = new string[2] { "o", "" };
            l[1] = new string[2] { "o", "" };
            l[2] = new string[2] { "o", "o" };
            blocks.Add("l", l);

            string[][] j = new string[3][];
            j[0] = new string[2] { "", "o" };
            j[1] = new string[2] { "", "o" };
            j[2] = new string[2] { "o", "o" };
            blocks.Add("j", j);

            string[][] t = new string[2][];
            t[0] = new string[3] { "o", "o", "o" };
            t[1] = new string[3] { "", "o", "" };
            blocks.Add("t", t);

            return blocks;
        }
    }
}
