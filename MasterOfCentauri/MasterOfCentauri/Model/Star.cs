using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MasterOfCentauri.Model
{
    class Star
    {
        private string _name;
        private int _x, _y;
        private Rectangle _boundingBox;

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

        public string Name
        {
            get { return "Star"; }
            set { _name = value; }
        }

         public Rectangle BoundingBox
        {
            get
            {
                if (_boundingBox == null)
                {
                    _boundingBox = new Rectangle(X, Y, 20, 20);
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
