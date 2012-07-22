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
        protected int _camViewportWidth;
        protected int _camViewportHeight;
        protected int _worldWidth;
        protected int _worldHeight;
        public Rectangle? _limits;
        public Rectangle _bounds;

        public Camera2D(ConsoleManager console, bool addToConsole)
        {
            _zoom = 1.0f;
            _minZoom = 0.1f;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
            if (addToConsole)
                console.HookInto(this);
        }

        public Rectangle? Limits
        {
            set
            {
                _limits = value;
                validatePosition();
            }
        }


        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < _minZoom) _zoom = _minZoom; if (_zoom > _maxZoom) _zoom = _maxZoom; validateZoom(); validatePosition(); } // Negative zoom will flip image
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
            return validatePosition();
        }
        // Get set position
        public Vector2 Pos
        {
            get { return _pos; }
            set
            {
                _pos = value;
                validateZoom();
                validatePosition();
            }
        }

        public int CamViewPortWidth
        {
            get { return _camViewportWidth; }
            set { _camViewportWidth = value; }
        }

        public int CamViewPortHeight
        {
            get { return _camViewportHeight ; }
            set { _camViewportHeight = value; }
        }

        public int CamWorldWidth
        {
            get { return _worldWidth; }
            set { _worldWidth = value; }
        }

        public int CamWorldHeight
        {
            get { return _worldHeight; }
            set { _worldHeight = value; }
        }

        public Matrix ViewMatrix
        {
            get
            {
                return getTransformation();
            }
        }

        public float getCamScaleX()
        {
            float scaleToFitWidth = (float)CamViewPortWidth / (float)CamWorldWidth;
            return scaleToFitWidth;
        }

        public float getCamScaleY()
        {
            float scaleToFitHeight = (float)CamViewPortHeight / (float)CamWorldHeight;
            return scaleToFitHeight;
        }

        private void validateZoom()
        {
            if (_limits.HasValue)
            {
                float minZoomX = (float)CamWorldWidth / _limits.Value.Width;
                float minZoomY = (float)CamWorldHeight / _limits.Value.Height;
                _zoom = MathHelper.Max(_zoom, MathHelper.Max(minZoomX, minZoomY));
            }
        }


        private bool validatePosition()
        {
            if (_limits.HasValue)
            {
                Vector2 cameraWorldMin = Vector2.Transform(Vector2.Zero, Matrix.Invert(ViewMatrix));
                Vector2 cameraSize = new Vector2(CamWorldWidth, CamWorldHeight) / _zoom;
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

        public float getWorldViewY()
        {
             Vector2 cameraWorldMin = Vector2.Transform(Vector2.Zero, Matrix.Invert(ViewMatrix));
             return cameraWorldMin.Y; 
        }

        public float getWorldViewX()
        {
            Vector2 cameraWorldMin = Vector2.Transform(Vector2.Zero, Matrix.Invert(ViewMatrix));
            return cameraWorldMin.X;
        }

        public float getWorldViewWidth()
        {
            Vector2 cameraSize = new Vector2(CamWorldWidth, CamWorldHeight) / _zoom;
            return cameraSize.X;
        }

        public float getWorldviewHeight()
        {
            Vector2 cameraSize = new Vector2(CamWorldWidth, CamWorldHeight) / _zoom;
            return cameraSize.Y;
        }

        public Rectangle getWorldBounds()
        {
            _bounds = new Rectangle((int)getWorldViewX(), (int)getWorldViewY(), (int)getWorldViewWidth(), (int)getWorldviewHeight());  
            return _bounds; 
        }

        public Matrix getTransformation()
        {

            _transform =       
              Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(CamWorldWidth * 0.5f, CamWorldHeight * 0.5f, 0)) ;
            return _transform;
        }

        public Matrix getScale()
        {
            Matrix scale = Matrix.CreateScale(getCamScaleX(), getCamScaleY(), 1f);
            return scale;
        }

        public IEnumerable<ConsoleCommand> Commands { get { return Enumerable.Empty<ConsoleCommand>(); } }
        public string Name { get { return "Camera"; } }
        public event EventHandler RemoveCommands;
    }
}
