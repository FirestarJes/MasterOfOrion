﻿using System;
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
        private string _starTexture;
        

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
            get { return _name; }
            set { _name = value; }
        }

        public string StarTexture
        {
            get { return _starTexture; }
            set { _starTexture = value; }
        }

         public Rectangle BoundingBox
        {
            get
            {
                if (_boundingBox == null)
                {
                    _boundingBox = new Rectangle(X, Y, 256, 256);
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
