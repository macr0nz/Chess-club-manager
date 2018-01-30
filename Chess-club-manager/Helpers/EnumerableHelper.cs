using System;
using System.Collections.Generic;

namespace Chess_club_manager.Helpers
{
    public static class EnumerableHelper
    {
        private static readonly Random Rand = new Random();
        public static void RandomizeOrder<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}