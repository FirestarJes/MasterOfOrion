using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MasterOfCentauri.Model
{
    class GalaxySector
    {
        private int _x, _y, _width, _height;
        
        private Rectangle _boundingBox;
        private List<Star> _stars;

        public List<Star> Stars
        {
            get { return _stars; }
            set { _stars = value; }
        }

        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height ; }
            set { _height  = value; }
        }

        public Rectangle BoundingBox
        {
            get
            {
                if (_boundingBox == null)
                {
                    _boundingBox = new Rectangle(X, Y, Width, Height);
                }
                return _boundingBox; 
            }
            set
            {
                _boundingBox = value;
            }
        }

        
    }
}
