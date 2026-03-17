using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Util.Extensions
{
    public static class IntExtensions
    {
        public static int GetRandom(int limit)
        {
            Random rnd = new Random();
            return rnd.Next(1, limit);  // creates a number between 1 and 12
        }
    }
}
