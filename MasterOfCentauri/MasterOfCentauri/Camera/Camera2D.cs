using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterOfCentauri.Camera
{
    class Camera2D
    {
        protected float _zoom; // Camera Zoom
        public Matrix _transform; // Matrix Transform
        public Vector2 _pos; // Camera Position
        protected float _rotation; // Camera Rotation
        protected int _viewportWidth;
        protected int _viewportHeight;

        public Camera2D()
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
        }

        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        // Auxiliary function to move the camera
        public void Move(Vector2 amount)
        {
            _pos += amount;
        }
        // Get set position
        public Vector2 Pos
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public int ViewPortWidth
        {
            get { return _viewportWidth; }
            set { _viewportWidth = value; }
        }

        public int ViewPortHeight
        {
            get { return _viewportHeight ; }
            set { _viewportHeight = value; }
        }

        public Matrix get_transformation()
        {
            _transform = Matrix.CreateTranslation(new Vector3(-ViewPortWidth * 0.5f - _pos.X, -ViewPortHeight * 0.5f - _pos.Y, 0)) * Matrix.CreateScale(
                new Vector3((_zoom * _zoom * _zoom),
                (_zoom * _zoom * _zoom), 0))
            * Matrix.CreateRotationZ(_rotation)
               * Matrix.CreateTranslation(new Vector3(
                ViewPortWidth * 0.5f, ViewPortHeight * 0.5f, 0));

            return _transform;
        }
    }
}
