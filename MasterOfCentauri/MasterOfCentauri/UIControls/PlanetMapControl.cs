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
        private readonly ContentController _content;
        private readonly SpriteBatch _spritebatch;
        private readonly SpriteFont _font;
        private readonly Texture2D _black;
        private readonly Texture2D _testTexture;
        private static int buildingCellSize = 128;

        public PlanetMapViewModel ViewData { get; set; }

        public PlanetMapControl(IServiceProvider services)
        {
            _content = (ContentController)services.GetService(typeof(ContentController));
            _spritebatch = (SpriteBatch)services.GetService(typeof(SpriteBatch));
            _black = _content.GetContent<Texture2D>(@"StarFields\black");
            _font = _content.GetContent<SpriteFont>("Fonts/SpriteFont1");
            _testTexture = _content.GetContent<Texture2D>("SystemMap/Planet1");

            Name = "PlanetMap";
            ClipContent = true;
        }

        protected override void OnCustomRendering()
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
            var offset = CalculateBuildingCellOffset(cells);
            for (int x = 0; x <= cells.GetUpperBound(0) ; x++)
            {
                for (int y = 0; y <= cells.GetUpperBound(1); y++)
                {
                    Vector2 cellPosition = GetCellPosition(offset,x,y);
                    _spritebatch.Draw(_testTexture, cellPosition, Color.White);
                }
            }
        }

        private Vector2 CalculateBuildingCellOffset(BuildingCellViewModel[,] cells)
        {
            var totalWidth = (cells.GetUpperBound(0)+1) * buildingCellSize;
            var totalHeight = (cells.GetUpperBound(1)+1) * buildingCellSize;
            var centerPoint = new Rectangle(0,0, (int)ActualWidth, (int)ActualHeight).Center;
            
            var offset = new Vector2(centerPoint.X, centerPoint.Y) - (new Vector2(totalWidth, totalHeight)/2);

            return offset;
        }

        private Vector2 GetCellPosition(Vector2 offset, int x, int y)
        {
            return offset + new Vector2(x, y) * buildingCellSize;
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