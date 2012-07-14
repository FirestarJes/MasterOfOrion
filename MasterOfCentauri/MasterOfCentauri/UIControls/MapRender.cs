using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DigitalRune.Game.UI;
using DigitalRune.Game.UI.Controls;
using DigitalRune.Game.UI.Rendering;
using MasterOfCentauri.Managers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using DigitalRune.Game.Input;
using DigitalRune.Mathematics.Algebra;

namespace MasterOfCentauri.UIControls
{
    class MapRender: DigitalRune.Game.UI.Controls.UIControl  
    {
        SpriteBatch _spritebatch;
        Texture2D _black;
        float camX, camY;
        float scrollX, scrollY;
        private Texture2D _textureBackground, _textureParallax, _textureParallax2, testTexture;
        private SpriteFont arialFont;
        private float _parallax1SpeedMod, _parallax2SpeedMod;
        private Camera.Camera2D cam;
        private Vector2 _mousePos;
        private Vector2 _mousePosMoved;
        private readonly IInputService _inputService;
        private ContentController _content;

    public MapRender(IServiceProvider services)
    {
      Name = "GameViewControl";
      MinWidth = 400;
      MinHeight = 300;
      HorizontalAlignment = HorizontalAlignment.Stretch;
      VerticalAlignment = VerticalAlignment.Stretch;
      _spritebatch =  (SpriteBatch)services.GetService(typeof(SpriteBatch));
      _inputService = (IInputService)services.GetService(typeof(IInputService));
      _content = (ContentController)services.GetService(typeof(ContentController));
      cam = new Camera.Camera2D((ConsoleManager)services.GetService(typeof(ConsoleManager)));

      _parallax1SpeedMod = 1.5f;
      _parallax2SpeedMod = 2f;
      cam.MaxZoom = 2.0f;
      cam.MinZoom = 0.1f;
      cam.MaxX = 1000;
      cam.MaxY = 720;
      cam.MinX = 0;
      cam.MinY = 0;
     
    }
    protected override void OnLoad()
    {
        _textureBackground = _content.GetContent<Texture2D>(@"StarFields\starfield1");
        _textureParallax = _content.GetContent<Texture2D>(@"StarFields\starfield2");
        _textureParallax2 = _content.GetContent<Texture2D>(@"StarFields\starfield3");
        testTexture = _content.GetContent<Texture2D>(@"StarFields\test");
        arialFont = _content.GetContent<SpriteFont>(@"Fonts\Spritefont1");
        _black = _content.GetContent<Texture2D>(@"StarFields\black");
        base.OnLoad();
    }

    protected override void OnUpdate(TimeSpan deltaTime)
    {
        base.OnUpdate(deltaTime);
    }

    protected override void OnRender(UIRenderContext context)
    {
        IUIRenderer renderer = Screen.Renderer;
        GraphicsDevice graphicsDevice = renderer.GraphicsDevice;

        cam.ViewPortHeight = (int)ActualHeight;
        cam.ViewPortWidth = (int)ActualWidth;

        if (_black == null)
        {
           
        }

        Rectangle originalViewport = graphicsDevice.Viewport.Bounds;
        Rectangle viewport = new Rectangle((int)ActualX, (int)ActualY, (int)ActualWidth, (int)ActualHeight);
        if (viewport.Width == 0 || viewport.Height == 0)
            return;
        viewport = Rectangle.Intersect(originalViewport, viewport);

        renderer.EndBatch();
        
        graphicsDevice.Viewport = new Viewport(viewport);

        _spritebatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
        _spritebatch.Draw(_black, new Rectangle(0, 0, (int)ActualWidth, (int)ActualHeight), Color.White);
        _spritebatch.Draw(_textureBackground, new Rectangle(0,0, (int)ActualWidth, (int)ActualHeight), new Rectangle((int)(1 * (int)-scrollX), (int)(1 * (int)-scrollY), _textureBackground.Width, _textureBackground.Height), Color.White);
        _spritebatch.Draw(_textureParallax, new Rectangle(0, 0, (int)ActualWidth, (int)ActualHeight), new Rectangle((int)(_parallax1SpeedMod * (int)-scrollX), (int)(_parallax1SpeedMod * (int)-scrollY), _textureParallax.Width, _textureParallax.Height), Color.White);
        _spritebatch.Draw(_textureParallax2, new Rectangle(0, 0, (int)ActualWidth, (int)ActualHeight), new Rectangle((int)(_parallax2SpeedMod * (int)-scrollX), (int)(_parallax2SpeedMod * (int)-scrollY), _textureParallax2.Width, _textureParallax2.Height), Color.White);
        _spritebatch.End();

        _spritebatch.Begin(SpriteSortMode.BackToFront , BlendState.AlphaBlend, null, null, null, null, cam.get_transformation());
        _spritebatch.Draw(testTexture, new Rectangle(0, 0, testTexture.Width, testTexture.Height), Color.White);
        _spritebatch.Draw(testTexture, new Rectangle(1000, 0, testTexture.Width, testTexture.Height), Color.White);
        _spritebatch.End();

        graphicsDevice.Viewport = new Viewport(originalViewport);

        base.OnRender(context);

    }

    protected override void OnHandleInput(DigitalRune.Game.UI.Controls.InputContext context)
        {
            //onlyhandle input if not other control has handled it.
            if (!_inputService.IsMouseOrTouchHandled && context.IsMouseOver )
            {

                float MouseWorldX = (context.MousePosition.X - cam.ViewPortWidth * 0.5f + (cam.ViewPortWidth * 0.5f + cam.Pos.X) * cam.Zoom) /
                cam.Zoom;

                float MouseWorldY = (context.MousePosition.Y - cam.ViewPortHeight * 0.5f + (cam.ViewPortHeight * 0.5f + cam.Pos.Y) * cam.Zoom) /
                        cam.Zoom;

                float MouseWorldXMoved = ((context.MousePosition.X - context.MousePositionDelta.X) - cam.ViewPortWidth * 0.5f + (cam.ViewPortWidth * 0.5f + cam.Pos.X) * cam.Zoom) /
                cam.Zoom;

                float MouseWorldYMoved = ((context.MousePosition.Y - context.MousePositionDelta.Y) - cam.ViewPortHeight * 0.5f + (cam.ViewPortHeight * 0.5f + cam.Pos.Y) * cam.Zoom) /
                        cam.Zoom;

               _mousePos = new Vector2(MouseWorldX, MouseWorldY);
               _mousePosMoved = new Vector2(MouseWorldXMoved, MouseWorldYMoved);
               var _mousePosMovedDelta = _mousePos - _mousePosMoved;

                //Here we can handle all checks against world coordinates.
                if (_inputService.MouseWheelDelta != 0)
                    cam.Zoom += _inputService.MouseWheelDelta > 1 ? 0.1f : -0.1f; 
                if (_inputService.IsDown(MouseButtons.Left))
                {
                    if (cam.Move(-_mousePosMovedDelta))
                    {
                        scrollX += context.MousePositionDelta.X;
                        scrollY += context.MousePositionDelta.Y;
                    }
                }
            }
            
            base.OnHandleInput(context);
        }

    }
}
