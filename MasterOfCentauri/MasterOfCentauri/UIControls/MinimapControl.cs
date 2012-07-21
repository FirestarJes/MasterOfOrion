using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using DigitalRune.Game.UI.Controls;
using Microsoft.Xna.Framework;
using DigitalRune.Game.UI;
using DigitalRune.Game.UI.Rendering;
using MasterOfCentauri.Managers;
using MasterOfCentauri.Helpers;

namespace MasterOfCentauri.UIControls
{
    class MinimapControl: UIControl 
    {
        private Texture2D _minimap, _black, _test;
        private ContentController _content;
        private GameManager _gameManager;
        

        public MinimapControl(IServiceProvider services)
        {
            Name = "Mini Map";
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            LoadServices(services);
        }

        private void LoadServices(IServiceProvider services)
        {
            _content = (ContentController)services.GetService(typeof(ContentController));
            _gameManager = (GameManager)services.GetService(typeof(GameManager));

        }

        protected override void OnLoad()
        {
            _black = _content.GetContent<Texture2D>(@"StarFields\black");
            _test = _content.GetContent<Texture2D>(@"StarFields\test");
            base.OnLoad();
        }

        protected override void OnRender(UIRenderContext context)
        {
            Screen.Renderer.EndBatch();

            Rectangle originalViewport = Screen.Renderer.GraphicsDevice.Viewport.Bounds;
            Rectangle viewport = new Rectangle((int)ActualX, (int)ActualY, (int)ActualWidth, (int)ActualHeight);
            if (viewport.Width == 0 || viewport.Height == 0)
                return;
            viewport = Rectangle.Intersect(originalViewport, viewport);

            using (new ViewportScope(Screen.Renderer.GraphicsDevice, new Viewport(viewport)))
            {
                if (_minimap == null)
                {
                    Camera.Camera2D minimapCam = new Camera.Camera2D(null, false);
                    minimapCam.CamViewPortHeight = (int)ActualHeight;
                    minimapCam.CamViewPortWidth = (int)ActualWidth;
                    minimapCam.CamWorldHeight = _gameManager.Galaxy.Height; //Should be gotten from the current Galaxy in GameManager
                    minimapCam.CamWorldWidth = _gameManager.Galaxy.Width; //Should be gotten from the current Galaxy in GameManager
                    minimapCam.Pos = new Vector2(minimapCam.CamWorldWidth / 2, minimapCam.CamWorldHeight / 2);

                    _minimap = RenderMiniMap(minimapCam, Screen.Renderer.GraphicsDevice);
                }

                RenderMinimapWithOverlay(Screen.Renderer.SpriteBatch);
            }
            base.OnRender(context);
        }

        private Texture2D RenderMiniMap(Camera.Camera2D miniCam, GraphicsDevice graphicsDevice)
        {
            SpriteBatch localSpriteBatch = Screen.Renderer.SpriteBatch; 
            RenderTarget2D mini = new RenderTarget2D(graphicsDevice, (int)ActualWidth, (int)ActualHeight);
            graphicsDevice.SetRenderTarget(mini);
            localSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, miniCam.getTransformation() * miniCam.getScale());
            Texture2D starTexture;
            localSpriteBatch.Draw(_black, new Rectangle(0, 0, miniCam.CamWorldWidth, miniCam.CamWorldHeight), Color.White);
            foreach (Model.Star star in _gameManager.Galaxy.Stars)
            {
                Util.TextureAtlas atlas = _content.getStarAtlasFromTextureName(star.StarTexture);
                if (atlas != null)
                {
                    starTexture = atlas.AtlasTexture;
                    localSpriteBatch.Draw(starTexture, star.BoundingBox, atlas.AtlasCoords[star.StarTexture], Color.White);
                }

            }
            localSpriteBatch.End();
            graphicsDevice.SetRenderTarget(null);
            return mini;
        }

        private void RenderMinimapWithOverlay(SpriteBatch localSpriteBatch)
        {
            float percX, percY, percWidth, percHeight, worldViewWidth, worldViewHeight;
            percX = (_gameManager.GalaxyCam.Pos.X - (_gameManager.GalaxyCam.getWorldViewWidth() / 2)) / _gameManager.Galaxy.Width;
            percY = (_gameManager.GalaxyCam.Pos.Y - (_gameManager.GalaxyCam.getWorldviewHeight() / 2)) / _gameManager.Galaxy.Height;
            worldViewWidth = _gameManager.GalaxyCam.getWorldViewWidth();
            worldViewHeight = _gameManager.GalaxyCam.getWorldviewHeight();
            percWidth = _gameManager.GalaxyCam.getWorldViewWidth() / _gameManager.Galaxy.Width;
            percHeight = _gameManager.GalaxyCam.getWorldviewHeight() / _gameManager.Galaxy.Height;
            localSpriteBatch.Begin();
            localSpriteBatch.Draw(_minimap, new Rectangle(0, 0, (int)ActualWidth, (int)ActualHeight), Color.White);
            localSpriteBatch.Draw(_test, new Rectangle((int)(ActualWidth * percX), (int)(ActualHeight * percY), (int)(ActualWidth * percWidth), (int)(ActualHeight * percHeight)), Color.White);
            localSpriteBatch.End();
        }
    }
}
