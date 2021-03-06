﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DigitalRune.Game.UI;
using DigitalRune.Game.UI.Controls;
using DigitalRune.Game.UI.Rendering;
using MasterOfCentauri.Helpers;
using MasterOfCentauri.Managers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using DigitalRune.Game.Input;
using DigitalRune.Mathematics.Algebra;
using MasterOfCentauri.UIControls.Base;

namespace MasterOfCentauri.UIControls
{
    class GalaxyMapControl : ContentControl
    {
        private SpriteBatch _spritebatch;
        private Texture2D _textureBackground, _textureParallax, _textureParallax2, _black;
        private float _parallax1SpeedMod, _parallax2SpeedMod, scrollX, scrollY;
        private Vector2 _mousePos, _mousePosMoved;
        private ContentController _content;
        private GraphicsDevice _graphicsDevice;
        private GameManager _gameManager;
        private SpriteFont _starNameFont;
        protected bool IsClicked { get; set; }
        protected bool IsDown { get; set; }

        public GalaxyMapControl(IServiceProvider services)
        {
            Name = "Galaxy View";
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            _spritebatch = (SpriteBatch)services.GetService(typeof(SpriteBatch));
            _content = (ContentController)services.GetService(typeof(ContentController));
            _gameManager = (GameManager)services.GetService(typeof(GameManager));
            _parallax1SpeedMod = 1.5f;
            _parallax2SpeedMod = 2f;
        }
        protected override void OnLoad()
        {
            _textureBackground = _content.GetContent<Texture2D>(@"StarFields\starfield1");
            _textureParallax = _content.GetContent<Texture2D>(@"StarFields\starfield2");
            _textureParallax2 = _content.GetContent<Texture2D>(@"StarFields\starfield3");
            _black = _content.GetContent<Texture2D>(@"StarFields\black");
            _starNameFont = _content.GetContent<SpriteFont>(@"Fonts\SpriteFont1");
            base.OnLoad();
        }

        protected override void OnUpdate(TimeSpan deltaTime)
        {
            base.OnUpdate(deltaTime);
        }

        protected override void OnRender(UIRenderContext context)
        {
            IUIRenderer renderer = Screen.Renderer;
            _graphicsDevice = renderer.GraphicsDevice;


            _gameManager.GalaxyCam.CamViewPortHeight = (int)ActualHeight;
            _gameManager.GalaxyCam.CamViewPortWidth = (int)ActualWidth;

            Rectangle originalViewport = _graphicsDevice.Viewport.Bounds;
            Rectangle viewport = new Rectangle((int)ActualX, (int)ActualY, (int)ActualWidth, (int)ActualHeight);
            if (viewport.Width == 0 || viewport.Height == 0)
                return;
            viewport = Rectangle.Intersect(originalViewport, viewport);

            //We must call this to let everything the UI has currently drawn be drawn, before changing settings for spritebatch
            renderer.EndBatch();

            using (new ViewportScope(_graphicsDevice, new Viewport(viewport)))
            {

                _spritebatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
                _spritebatch.Draw(_black, new Rectangle(0, 0, (int)ActualWidth, (int)ActualHeight), Color.White);
                _spritebatch.Draw(_textureBackground, new Rectangle(0, 0, (int)ActualWidth, (int)ActualHeight), new Rectangle((int)(1 * (int)-scrollX), (int)(1 * (int)-scrollY), _textureBackground.Width, _textureBackground.Height), Color.White);
                _spritebatch.Draw(_textureParallax, new Rectangle(0, 0, (int)ActualWidth, (int)ActualHeight), new Rectangle((int)(_parallax1SpeedMod * (int)-scrollX), (int)(_parallax1SpeedMod * (int)-scrollY), _textureParallax.Width, _textureParallax.Height), Color.White);
                _spritebatch.Draw(_textureParallax2, new Rectangle(0, 0, (int)ActualWidth, (int)ActualHeight), new Rectangle((int)(_parallax2SpeedMod * (int)-scrollX), (int)(_parallax2SpeedMod * (int)-scrollY), _textureParallax2.Width, _textureParallax2.Height), Color.White);
                _spritebatch.DrawString(_starNameFont, _mousePos.X + " / " + _mousePos.Y, Vector2.Zero, Color.White);
                _spritebatch.DrawString(_starNameFont, _gameManager.GalaxyCam.Pos.X + " / " + _gameManager.GalaxyCam.Pos.Y, Vector2.Zero + new Vector2(0, 20), Color.White);
                _spritebatch.End();

                _spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, _gameManager.GalaxyCam.getTransformation() * _gameManager.GalaxyCam.getScale());
                //Render Decoration
                foreach (Model.GalaxyDecoration decor in _gameManager.Galaxy.Decorations)
                {
                    Util.TextureAtlas atlas = _content.getGalaxyDecorationAtlasFromTextureName(decor.TextureName);
                    if (atlas != null)
                    {
                        _spritebatch.Draw(atlas.AtlasTexture, new Rectangle((int)decor.Position.X, (int)decor.Position.Y, decor.Width, decor.Height), atlas.AtlasCoords[decor.TextureName], Color.White);
                    }
                }

                //Render Stars
                RenderStars();

                _spritebatch.End();

            }

            base.OnRender(context);

        }

        private void RenderStars()
        {
            Dictionary<Util.TextureAtlas, List<Model.Star>> sortedStars = new Dictionary<Util.TextureAtlas, List<Model.Star>>();
            foreach (Model.GalaxySector sec in _gameManager.Galaxy.Sectors)
            {
                if (_gameManager.GalaxyCam.getWorldBounds().Intersects(sec.BoundingBox))
                {
                    foreach (Model.Star star in sec.Stars)
                    {
                        Util.TextureAtlas atlas = _content.getStarAtlasFromTextureName(star.StarTexture);
                        if (atlas != null)
                        {
                            if (!sortedStars.ContainsKey(atlas))
                            {
                                sortedStars.Add(atlas, new List<Model.Star>());
                            }
                            sortedStars[atlas].Add(star);
                        }
                    }
                }
            }
            foreach (Util.TextureAtlas textureAtlas in sortedStars.Keys)
            {
                foreach (Model.Star star in sortedStars[textureAtlas])
                {
                    _spritebatch.Draw(textureAtlas.AtlasTexture, star.BoundingBox, textureAtlas.AtlasCoords[star.StarTexture], Color.White);
                    Vector2 textPosition = star.Position + new Vector2(32, 64);
                    Vector2 stringCenter = _starNameFont.MeasureString(star.Name) * 0.5f;
                    _spritebatch.DrawString(_starNameFont, star.Name, new Vector2(textPosition.X + 1, textPosition.Y + 1), Color.Black, 0, stringCenter, 0.6f, SpriteEffects.None, 0f);
                    _spritebatch.DrawString(_starNameFont, star.Name, textPosition, Color.RoyalBlue, 0, stringCenter, 0.6f, SpriteEffects.None, 0f);
                }
            }
        }



        protected override void OnHandleInput(DigitalRune.Game.UI.Controls.InputContext context)
        {
            //onlyhandle input if not other control has handled it.

            if (!InputService.IsMouseOrTouchHandled && IsMouseDirectlyOver)
            {
                Vector2 TransformedMousePos = Vector2.Transform(new Vector2(context.MousePosition.X - ActualX, context.MousePosition.Y - ActualY), Matrix.Invert(_gameManager.GalaxyCam.getTransformation() * _gameManager.GalaxyCam.getScale()));
                Vector2 TransformedMouseDeltaPos = Vector2.Transform(new Vector2((context.MousePosition.X - context.MousePositionDelta.X) - ActualX, (context.MousePosition.Y - context.MousePositionDelta.Y) - ActualY), Matrix.Invert(_gameManager.GalaxyCam.getTransformation() * _gameManager.GalaxyCam.getScale()));

                _mousePos = TransformedMousePos;
                _mousePosMoved = TransformedMouseDeltaPos;
                var _mousePosMovedDelta = TransformedMousePos - TransformedMouseDeltaPos;

                //Here we can handle all checks against world coordinates.
                if (InputService.MouseWheelDelta != 0)
                {
                    //_gameManager.GalaxyCam.Pos = TransformedMousePos;
                    _gameManager.GalaxyCam.Zoom += InputService.MouseWheelDelta > 1 ? 0.04f : -0.04f;
                    InputService.IsMouseOrTouchHandled = true;
                }
                if (InputService.IsDown(MouseButtons.Left))
                {
                    if (_gameManager.GalaxyCam.Move(-_mousePosMovedDelta))
                    {
                        scrollX += context.MousePositionDelta.X;
                        scrollY += context.MousePositionDelta.Y;
                        InputService.IsMouseOrTouchHandled = true;
                    }
                }
                if (InputService.IsDoubleClick(MouseButtons.Left))
                {
                    foreach (Model.GalaxySector sec in _gameManager.Galaxy.Sectors)
                    {
                        foreach (Model.Star star in sec.Stars)
                        {
                            if (star.BoundingBox.Intersects(new Rectangle((int)TransformedMousePos.X, (int)TransformedMousePos.Y, 1, 1)))
                            {
                                //teststring = "double Clicked Star";
                                break;
                            }
                            else
                            {
                                //teststring = "empty";
                            }
                        }
                        InputService.IsMouseOrTouchHandled = true;
                        return;
                    }
                }

                IsClicked = false;

                if (InputService.IsDown(MouseButtons.Left) && IsMouseOver && IsDown == false)
                {
                    InputService.IsMouseOrTouchHandled = true;
                    IsClicked = true;
                }

                IsDown = InputService.IsDown(MouseButtons.Left);

                if (IsClicked)
                {
                    foreach (Model.GalaxySector sec in _gameManager.Galaxy.Sectors)
                    {
                        foreach (Model.Star star in sec.Stars)
                        {
                            if (star.BoundingBox.Intersects(new Rectangle((int)TransformedMousePos.X, (int)TransformedMousePos.Y, 1, 1)))
                            {
                                //teststring = "Clicked Star";
                                break;
                            }
                            else
                            {
                                //teststring = "empty";
                            }
                        }
                    }

                }
            }
            base.OnHandleInput(context);

        }

    }
}
