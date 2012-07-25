using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterOfCentauri.Model
{
    static class Constants
    {
        //Constants
        public static int STAR_WIDTH = 64; //This is the stars width including halo in worldunits
        public static int MinDistanceBetweenStars = STAR_WIDTH * 2; //in worldunits so that stars aren't placed to close
        public static int MaxAttemptsToPlaceStar = 100;

        public static List<int> _galaxySizes = new List<int>() { 512, 1024, 2048, 4096, 8192 };
        public static List<int> _starIntervals = new List<int>() { 80, 100, 150, 300, 500 };
        public static List<int> _sectorIntervals = new List<int>() { 4, 6, 10, 12, 16 };
    }
}
