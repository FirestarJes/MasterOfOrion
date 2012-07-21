using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework

;namespace MasterOfCentauri.Model
{
    class GalaxyDecoration
    {
        private string _decorationTexture;
        private Vector2 _position;
        private int _width, _height;
        private float _rotation;

        public GalaxyDecoration()
        {
            _position = new Vector2(0, 0);
            _width = 0;
            _height = 0;
            _rotation = 0;
            _decorationTexture = string.Empty; 
        }

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

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public string TextureName
        {
            get { return _decorationTexture; }
            set { _decorationTexture = value; }
        }
    }
}
