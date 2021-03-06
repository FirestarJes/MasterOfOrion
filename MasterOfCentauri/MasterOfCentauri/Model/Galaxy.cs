﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterOfCentauri.Model
{
    class Galaxy
    {
        protected int _numStars;
        protected int _width;
        protected int _height;
        private List<GalaxySector> _sectors;
        private List<GalaxyDecoration> _decorations;


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


        public int StarsCount
        {
            get { return _numStars; }
            set { _numStars = value; }
        }

        public List<GalaxySector> Sectors
        {
            get { return _sectors; }
            set { _sectors = value; }
        }

        public List<GalaxyDecoration> Decorations
        {
            get { return _decorations; }
            set { _decorations = value; }
        }
        
    }
}
