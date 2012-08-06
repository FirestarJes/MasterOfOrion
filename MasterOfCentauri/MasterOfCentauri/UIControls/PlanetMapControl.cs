using System;
using System.Collections.Generic;
using System.Linq;
using DigitalRune.Game.UI.Consoles;
using MasterOfCentauri.Managers;
using MasterOfCentauri.Model.PlanetMap;
using MasterOfCentauri.TileSet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterOfCentauri.UIControls
{
    public class PlanetMapControl : CustomRenderingContentControl, IConsoleCommandHost
    {
        private readonly ContentController _content;
        private readonly ConsoleManager _consoleManager;
        private readonly SpriteBatch _spritebatch;
        private readonly SpriteFont _font;
        private readonly Texture2D _black;
        private TileSet.TileSet _tileSet;


        public PlanetMapViewModel ViewData { get; set; }
        public int BuildingCellSize { get; set; }

        public PlanetMapControl(IServiceProvider services)
        {
            _content = (ContentController)services.GetService(typeof(ContentController));
            _spritebatch = (SpriteBatch)services.GetService(typeof(SpriteBatch));
            _consoleManager = (ConsoleManager)services.GetService(typeof(ConsoleManager));
            _black = _content.GetContent<Texture2D>(@"StarFields\black");
            _font = _content.GetContent<SpriteFont>("Fonts/SpriteFont1");
            
            Name = "PlanetMap";
            ClipContent = true;
            BuildingCellSize = 128;

        }

        private ITile[,] GenerateTiles(BuildingCellViewModel[,] buildingCells)
        {
            var result = new ITile[buildingCells.GetUpperBound(0)+1,buildingCells.GetUpperBound(1)+1];

            for(int x=0;x<=buildingCells.GetUpperBound(0); x++)
            {
                for(int y=0;y<=buildingCells.GetUpperBound(1); y++)
                {
                    result[x, y] = new BuildingCellTile(_content, _spritebatch,buildingCells[x, y]);
                }
            }

            return result;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            _consoleManager.HookInto(this);
            _tileSet = new TileSet.TileSet(128);
            _tileSet.SetData(GenerateTiles(ViewData.BuildingCells));
        }

        protected override void OnCustomRendering()
        {
            _spritebatch.Begin();

            ClearScreen();
            DrawPlanetName();
            //DrawBuildingCells();

            _tileSet.SetOffset(CalculateBuildingCellOffset(ViewData.BuildingCells));
            _tileSet.Draw();

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

        private Vector2 CalculateBuildingCellOffset(BuildingCellViewModel[,] cells)
        {
            var totalWidth = (cells.GetUpperBound(0)+1) * BuildingCellSize;
            var totalHeight = (cells.GetUpperBound(1)+1) * BuildingCellSize;
            var centerPoint = new Rectangle(0,0, (int)ActualWidth, (int)ActualHeight).Center;
            
            var offset = new Vector2(centerPoint.X, centerPoint.Y) - (new Vector2(totalWidth, totalHeight)/2);

            return offset;
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

    internal class BuildingCellTile : ITile
    {
        private readonly SpriteBatch _spritebatch;
        private readonly BuildingCellViewModel _buildingCell;
        private readonly Texture2D _testTexture;
        private readonly Texture2D _testTexture2;
        
        public BuildingCellTile(ContentController content, SpriteBatch spritebatch, BuildingCellViewModel buildingCell)
        {
            _spritebatch = spritebatch;
            _buildingCell = buildingCell;
            _testTexture = content.GetContent<Texture2D>("StarFields/test");
            _testTexture2 = content.GetContent<Texture2D>("SystemMap/Planet1");
        }

        public void Draw(Vector2 position)
        {
            _spritebatch.Draw(_testTexture, new Rectangle((int)position.X, (int)position.Y, 128, 128), Color.White);
            
            if(_buildingCell.Type == CellMultiplier.ExtraEnergy)
            {
                _spritebatch.Draw(_testTexture2, new Rectangle((int)position.X, (int)position.Y, 128, 128), Color.White);
            }
        }
    }
}