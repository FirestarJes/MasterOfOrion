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
        private Vector2  _position;
        private Rectangle _boundingBox;
        private string _starTexture;


        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
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
                    _boundingBox = new Rectangle((int)Position.X, (int)Position.Y, Model.Constants.STAR_WIDTH, Model.Constants.STAR_WIDTH);
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
