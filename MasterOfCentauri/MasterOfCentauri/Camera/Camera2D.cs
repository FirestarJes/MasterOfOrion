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
        public Rectangle? _limits;

        public Camera2D(ConsoleManager console)
        {
            _zoom = 1.0f;
            _minZoom = 0.1f;
            _rotation = 0.0f;
            _pos = Vector2.Zero;

            console.HookInto(this);
        }

        public Rectangle? Limits
        {
            set
            {
                _limits = value;
                ValidatePosition();
            }
        }


        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < _minZoom) _zoom = _minZoom; ValidateZoom(); ValidatePosition(); } // Negative zoom will flip image
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

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        // Auxiliary function to move the camera
        public bool Move(Vector2 amount)
        {
            _pos += amount;
            return ValidatePosition();
        }
        // Get set position
        public Vector2 Pos
        {
            get { return _pos; }
            set
            {
                _pos = value;
                ValidateZoom();
                ValidatePosition();
            }
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

        public Matrix ViewMatrix
        {
            get
            {
                return get_transformation();
            }
        }

        private void ValidateZoom()
        {
            if (_limits.HasValue)
            {
                float minZoomX = (float)ViewPortWidth / _limits.Value.Width;
                float minZoomY = (float)ViewPortHeight / _limits.Value.Height;
                _zoom = MathHelper.Max(_zoom, MathHelper.Max(minZoomX, minZoomY));
            }
        }


        private bool ValidatePosition()
        {
            if (_limits.HasValue)
            {
                Vector2 cameraWorldMin = Vector2.Transform(Vector2.Zero, Matrix.Invert(ViewMatrix));
                Vector2 cameraSize = new Vector2(ViewPortWidth, ViewPortHeight) / _zoom;
                Vector2 limitWorldMin = new Vector2(_limits.Value.Left, _limits.Value.Top);
                Vector2 limitWorldMax = new Vector2(_limits.Value.Right, _limits.Value.Bottom);
                Vector2 positionOffset = _pos - cameraWorldMin;
                _pos = Vector2.Clamp(cameraWorldMin, limitWorldMin, limitWorldMax - cameraSize) + positionOffset;
                if (cameraWorldMin.X < limitWorldMin.X)
                    return false;
                if (cameraWorldMin.Y < limitWorldMin.Y)
                    return false;
                if (cameraWorldMin.X > limitWorldMax.X - cameraSize.X)
                    return false;
                if (cameraWorldMin.Y > limitWorldMax.Y - cameraSize.Y)
                    return false;
            }
            return true;
        }

        public Matrix get_transformation()
        {

            _transform =       
              Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(ViewPortWidth * 0.5f, ViewPortHeight * 0.5f, 0)) ;
            return _transform;
        }

        public IEnumerable<ConsoleCommand> Commands { get { return Enumerable.Empty<ConsoleCommand>(); } }
        public string Name { get { return "Camera"; } }
        public event EventHandler RemoveCommands;
    }
}
