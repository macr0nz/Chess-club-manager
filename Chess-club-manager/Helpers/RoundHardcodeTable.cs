using System.Collections.Generic;

namespace Chess_club_manager.Helpers
{
    public static class RoundHardcodeTable
    {
        public static int GetToursCountByPlayers(int playersCount)
        {
            if (playersCount == 0){ return 0; }
            if (playersCount == 1){ return 0; }
            if (playersCount == 2){ return 1; }
            if (playersCount == 3 || playersCount == 4){ return 3; }
            if (playersCount == 5 || playersCount == 6){ return 5; }
            if (playersCount == 7 || playersCount == 8){ return 7; }
            if (playersCount == 9 || playersCount == 10){ return 9; }
            if (playersCount == 11 || playersCount == 12){ return 11; }
            if (playersCount == 13 || playersCount == 14){ return 13; }
            if (playersCount == 15 || playersCount == 16){ return 15; }

            return 0;
        }


        public static List<TourGameIndex> GetGamesIndexesByTourNumber(int tourNumber, int toursCount)
        {
            var items = new List<TourGameIndex>();

            //if (toursCount == 3)
            //{
            //    if (tourNumber == 1)
            //    {
            //        items.Add(new TourGameIndex{ First = 1, Second = 4 });
            //    }
            //}


            return items;
        }




    }

    public class TourGameIndex
    {
        public int First { get; set; }
        public int Second { get; set; }
    }
}