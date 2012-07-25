using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterOfCentauri.Model
{
    class GalaxySetup
    {
        int _width, _height, _sectors, _stars, _arms;
        string _densityMap;


        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public int Sectors
        {
            get { return _sectors; }
            set { _sectors = value; } 
        }

        public int Stars
        {
            get { return _stars; }
            set { _stars = value; }
        }

        public int Arms
        {
            get { return _arms; }
            set { _arms = value; }
        }

        public string DensityMap
        {
            get { return _densityMap; }
            set { _densityMap = value; }
        }
    }
}
