using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterOfCentauri.Helpers
{
    public class ViewportScope : IDisposable
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly Viewport _originalViewPort;

        public ViewportScope(GraphicsDevice graphicsDevice, Viewport viewport)
        {
            _graphicsDevice = graphicsDevice;
            _originalViewPort = graphicsDevice.Viewport;
            _graphicsDevice.Viewport = viewport;
        }

        public void Dispose()
        {
            _graphicsDevice.Viewport = _originalViewPort;

        }
    }
}