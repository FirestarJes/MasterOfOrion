using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DigitalRune.Game.UI.Consoles;
using MasterOfCentauri.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterOfCentauri.Camera
{
    class Camera2D : IConsoleCommandHost
    {
        protected float _zoom; // Camera Zoom
        protected float _maxZoom, _minZoom;
        public Matrix _transform; // Matrix Transform
        public Vector2 _pos; // Camera Position
        protected float _rotation; // Camera Rotation
        protected int _viewportWidth;
        protected int _viewportHeight;
        protected int _minX, _minY, _maxX, _maxY;
        

        public Camera2D(ConsoleManager console)
        {
            _zoom = 1.0f;
            _minZoom = 0.1f;
            _rotation = 0.0f;
            _pos = Vector2.Zero;

            console.HookInto(this);
        }

        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom <_minZoom) _zoom = _minZoom; } // Negative zoom will flip image
        }

        public float MaxZoom
        {
            get { return _maxZoom; }
            set { _maxZoom = value; } 
        }

        public float MinZoom
        {
            get { return _minZoom; }
            set { _minZoom = value; }
        }

        public int MinX
        {
            get { return _minX ; }
            set { _minX = value; }
        }

        public int MinY
        {
            get { return _minY; }
            set { _minY = value; }
        }


        public int MaxX
        {
            get { return _maxX; }
            set { _maxX = value; }
        }


        public int MaxY
        {
            get { return _maxY; }
            set { _maxY = value; }
        }


        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        // Auxiliary function to move the camera
        public bool Move(Vector2 amount)
        {
            _pos += amount;

            if (_pos.X > _maxX)
            {
                _pos.X = _maxX;
                return false;
            }
            if (_pos.X < _minX)
            {
                _pos.X = _minX;
                return false;
            }
            if (_pos.Y > _maxY)
            {
                _pos.Y = _maxY;
                return false;
            }
            if (_pos.Y < _minY)
            {
                _pos.Y = _minY;
                return false;
            }
            return true;
        }
        // Get set position
        public Vector2 Pos
        {
            get { return _pos; }
            set
            {
                if (value.X > _maxX)
                    value.X = _maxX;
                if (value.X < _minX)
                    value.X = _minX;
                if (value.Y > _maxY)
                    value.Y = _maxY;
                if (value.Y < _minY)
                    value.Y = _minY;
                _pos = value; }
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
                new Vector3((_zoom),
                (_zoom), 0))
            * Matrix.CreateRotationZ(_rotation)
               * Matrix.CreateTranslation(new Vector3(
                ViewPortWidth * 0.5f, ViewPortHeight * 0.5f, 0));

            return _transform;
        }

        public IEnumerable<ConsoleCommand> Commands { get { return Enumerable.Empty<ConsoleCommand>(); } }
        public string Name { get { return "Camera"; } }
        public event EventHandler RemoveCommands;
    }
}
