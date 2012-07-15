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
    class MapRender : DigitalRune.Game.UI.Controls.UIControl
    {
        SpriteBatch _spritebatch;
        Texture2D _black;
        float scrollX, scrollY;
        private Texture2D _textureBackground, _textureParallax, _textureParallax2, testTexture;
        private SpriteFont arialFont;
        private float _parallax1SpeedMod, _parallax2SpeedMod;
        private Camera.Camera2D cam;
        private Vector2 _mousePos;
        private Vector2 _mousePosMoved;
        private readonly IInputService _inputService;
        private ContentController _content;
        private GalaxyManager _galaxyManager;
        private Model.Galaxy _testGalaxy;
        private string teststring;

        public MapRender(IServiceProvider services)
        {
            Name = "GameViewControl";
            MinWidth = 400;
            MinHeight = 300;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            _spritebatch = (SpriteBatch)services.GetService(typeof(SpriteBatch));
            _inputService = (IInputService)services.GetService(typeof(IInputService));
            _content = (ContentController)services.GetService(typeof(ContentController));
            _galaxyManager = (GalaxyManager)services.GetService(typeof(GalaxyManager));
            cam = new Camera.Camera2D((ConsoleManager)services.GetService(typeof(ConsoleManager)));

            _parallax1SpeedMod = 1.5f;
            _parallax2SpeedMod = 2f;
            cam.MaxZoom = 2.0f;
            cam.MinZoom = 0.1f;
            cam.Limits = new Rectangle(-3000, -3000, 16000, 16000);
            cam.Pos = new Vector2(5000, 5000);
            _testGalaxy = _galaxyManager.GenerateSpiralGalaxy(4, 10000, 10000, 8000);
            //_testGalaxy = _galaxyManager.GenerateIrregularGalaxy(300, 1000);
            cam.Zoom = 1.0f;
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

            Rectangle originalViewport = graphicsDevice.Viewport.Bounds;
            Rectangle viewport = new Rectangle((int)ActualX, (int)ActualY, (int)ActualWidth, (int)ActualHeight);
            if (viewport.Width == 0 || viewport.Height == 0)
                return;
            viewport = Rectangle.Intersect(originalViewport, viewport);

            renderer.EndBatch();

            graphicsDevice.Viewport = new Viewport(viewport);

            _spritebatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            _spritebatch.Draw(_black, new Rectangle(0, 0, (int)ActualWidth, (int)ActualHeight), Color.White);
            _spritebatch.Draw(_textureBackground, new Rectangle(0, 0, (int)ActualWidth, (int)ActualHeight), new Rectangle((int)(1 * (int)-scrollX), (int)(1 * (int)-scrollY), _textureBackground.Width, _textureBackground.Height), Color.White);
            _spritebatch.Draw(_textureParallax, new Rectangle(0, 0, (int)ActualWidth, (int)ActualHeight), new Rectangle((int)(_parallax1SpeedMod * (int)-scrollX), (int)(_parallax1SpeedMod * (int)-scrollY), _textureParallax.Width, _textureParallax.Height), Color.White);
            _spritebatch.Draw(_textureParallax2, new Rectangle(0, 0, (int)ActualWidth, (int)ActualHeight), new Rectangle((int)(_parallax2SpeedMod * (int)-scrollX), (int)(_parallax2SpeedMod * (int)-scrollY), _textureParallax2.Width, _textureParallax2.Height), Color.White);
            _spritebatch.DrawString(arialFont, "mousepos in world coords: " + (int)_mousePos.X + " (X) - " + (int)_mousePos.Y + " (Y)", Vector2.Zero, Color.White);
            _spritebatch.DrawString(arialFont, "delta: " + (int)_mousePosMoved.X + " (X) - " + (int)_mousePosMoved.Y + " (Y)", new Vector2(0, 20), Color.White);
            _spritebatch.DrawString(arialFont, "test: " + teststring , new Vector2(0, 40), Color.White);
            _spritebatch.End();

            //Render Gasses

            //Render Stars
            RenderStars();

            graphicsDevice.Viewport = new Viewport(originalViewport);

            base.OnRender(context);

        }

        private void RenderStars()
        {
            //render the current sectors being displayed
            _spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cam.get_transformation());
            Texture2D starTexture;
            foreach (Model.Star star in _testGalaxy.Stars)
            {
                Util.TextureAtlas atlas = _content.getStarAtlasFromTextureName(star.StarTexture);
                if (atlas != null)
                {
                    starTexture = atlas.AtlasTexture; 
                    _spritebatch.Draw(starTexture, star.BoundingBox, atlas.AtlasCoords[star.StarTexture] ,Color.White);
                }
                
            }
            Util.TextureAtlas atlas2 = _content.getStarAtlasFromTextureName(@"stars\red.png");
            starTexture = _content.GetContent<Texture2D>(@"starfields/test");
            _spritebatch.Draw(starTexture, new Rectangle(0, 0, 20, 20), Color.White);
            _spritebatch.End();
        }

        protected override void OnHandleInput(DigitalRune.Game.UI.Controls.InputContext context)
        {
            //onlyhandle input if not other control has handled it.
            
            if (!_inputService.IsMouseOrTouchHandled && IsMouseOver)
            {
                Vector2 TransformedMousePos = Vector2.Transform(new Vector2(context.MousePosition.X - ActualX, context.MousePosition.Y - ActualY)  ,Matrix.Invert(cam.get_transformation()));
                Vector2 TransformedMouseDeltaPos = Vector2.Transform(new Vector2((context.MousePosition.X - context.MousePositionDelta.X) - ActualX, (context.MousePosition.Y - context.MousePositionDelta.Y) - ActualY), Matrix.Invert(cam.get_transformation()));

                _mousePos = new Vector2(TransformedMousePos.X, TransformedMousePos.Y);
                _mousePosMoved = TransformedMouseDeltaPos;
                var _mousePosMovedDelta = TransformedMousePos - TransformedMouseDeltaPos;

                //Here we can handle all checks against world coordinates.
                if (_inputService.MouseWheelDelta != 0)
                {
                   
                    cam.Zoom += _inputService.MouseWheelDelta > 1 ? 0.1f : -0.1f;
                    
                }   
                if (_inputService.IsDown(MouseButtons.Left))
                {
                    foreach (Model.Star star in _testGalaxy.Stars)
                    {
                        if (star.BoundingBox.Intersects(new  Rectangle((int)TransformedMousePos.X, (int)TransformedMousePos.Y, 1, 1)))
                        {
                            teststring = "Clicked Star";
                            cam.Pos = new Vector2(star.X, star.Y) ;
                            break;
                        }
                        else
                        {
                            teststring = "empty";
                        }
                    }
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
