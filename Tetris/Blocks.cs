using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public static class Blocks
    {
        private static string block = "■";
        public static Dictionary<string, string[][]> createBlocks()
        {
            Dictionary<string, string[][]> blocks = new Dictionary<string, string[][]>();

            string[][] o = new string[2][];
            o[0] = new string[2] { block, block };
            o[1] = new string[2] { block, block };
            blocks.Add("o", o);

            string[][] i = new string[4][];
            i[0] = new string[1] { block };
            i[1] = new string[1] { block };
            i[2] = new string[1] { block };
            i[3] = new string[1] { block };
            blocks.Add("i", i);

            string[][] s = new string[2][];
            s[0] = new string[3] { "", block, block };
            s[1] = new string[3] { block, block, "" };
            blocks.Add("s", s);

            string[][] z = new string[2][];
            z[0] = new string[3] { block, block, "" };
            z[1] = new string[3] { "", block, block };
            blocks.Add("z", z);

            string[][] l = new string[3][];
            l[0] = new string[2] { block, "" };
            l[1] = new string[2] { block, "" };
            l[2] = new string[2] { block, block };
            blocks.Add("l", l);

            string[][] j = new string[3][];
            j[0] = new string[2] { "", block };
            j[1] = new string[2] { "", block };
            j[2] = new string[2] { block, block };
            blocks.Add("j", j);

            string[][] t = new string[2][];
            t[0] = new string[3] { block, block, block };
            t[1] = new string[3] { "", block, "" };
            blocks.Add("t", t);

            return blocks;
        }
    }
}
