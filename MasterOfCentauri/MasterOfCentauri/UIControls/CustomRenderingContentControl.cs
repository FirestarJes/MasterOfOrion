using DigitalRune.Game.UI.Controls;
using DigitalRune.Game.UI.Rendering;
using MasterOfCentauri.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterOfCentauri.UIControls
{
    public class CustomRenderingContentControl : ContentControl
    {
        protected override void OnRender(UIRenderContext context)
        {
            IUIRenderer renderer = Screen.Renderer;
            renderer.EndBatch();
            GraphicsDevice graphicsDevice = renderer.GraphicsDevice;

            Rectangle originalViewport = graphicsDevice.Viewport.Bounds;
            var viewport = new Rectangle((int)ActualX, (int)ActualY, (int)ActualWidth, (int)ActualHeight);
            if (viewport.Width == 0 || viewport.Height == 0)
                return;
            viewport = Rectangle.Intersect(originalViewport, viewport);


            using (new ViewportScope(graphicsDevice, new Viewport(viewport)))
            {
                OnCustomRendering();
            }

            base.OnRender(context);

        }

        protected virtual void OnCustomRendering() {}
    }
}