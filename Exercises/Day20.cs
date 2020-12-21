using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace AdventCode.Exercises
{
    public static class Day20
    {
        public static string Data => InputFetcher.FetchInput("Day20.txt");

        public class Tile
        {
            public bool[][] Data { get; set; } = new bool[10][];

            public int GetSideValue(bool[] data)
            {
                var hash = data.GetHashCode();

                return hash;
            }
        }
    }
}
