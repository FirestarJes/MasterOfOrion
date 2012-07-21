using System;
using System.Collections.Generic;
using System.Linq;
using DigitalRune.Game.UI.Consoles;
using MasterOfCentauri.Managers;
using MasterOfCentauri.Model.PlanetMap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterOfCentauri.UIControls
{
    public class PlanetMapControl : CustomRenderingContentControl, IConsoleCommandHost
    {
        private readonly IServiceProvider _services;
        private ContentController _content;
        private SpriteBatch _spritebatch;
        private SpriteFont _font;
        private Texture2D _black;
        private Texture2D _testTexture;

        public PlanetMapViewModel ViewData { get; set; }

        public PlanetMapControl(IServiceProvider services)
        {
            _services = services;
            _content = (ContentController)services.GetService(typeof(ContentController));
            _spritebatch = (SpriteBatch)services.GetService(typeof(SpriteBatch));
            _black = _content.GetContent<Texture2D>(@"StarFields\black");
            _font = _content.GetContent<SpriteFont>("Fonts/SpriteFont1");
            _testTexture = _content.GetContent<Texture2D>("SystemMap/Planet1");

            Name = "PlanetMap";
            ClipContent = true;
        }

        public override void OnCustomRendering()
        {
            _spritebatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);

            ClearScreen();
            DrawPlanetName();
            DrawBuildingCells();

            _spritebatch.End();
        }

        private void DrawPlanetName()
        {
            _spritebatch.DrawString(_font, ViewData.Name, Vector2.Zero, Color.White);
        }

        private void ClearScreen()
        {
            _spritebatch.Draw(_black, new Rectangle(0, 0, (int) ActualWidth, (int) ActualHeight), Color.White);
        }

        private void DrawBuildingCells()
        {
            var cells = ViewData.BuildingCells;
            for (int x = 0; x <= cells.GetUpperBound(0) ; x++)
            {
                for (int y = 0; y <= cells.GetUpperBound(1); y++)
                {
                    Vector2 cellPosition = GetCellPosition(x,y);
                    _spritebatch.Draw(_testTexture, cellPosition, Color.White);
                }
            }
        }

        private Vector2 GetCellPosition(int x, int y)
        {
            return new Vector2(128,128) + new Vector2(x, y)*128;
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            if(RemoveCommands != null)
                RemoveCommands(this, EventArgs.Empty);
        }

        public IEnumerable<ConsoleCommand> Commands { get { return Enumerable.Empty<ConsoleCommand>(); } }
        public event EventHandler RemoveCommands;
    }
}