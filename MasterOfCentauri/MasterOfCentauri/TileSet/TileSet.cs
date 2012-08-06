using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MasterOfCentauri.TileSet
{
    public class TileSet
    {
        private readonly int _tileSize;
        private ITile[,] _tiles;
        private int _xUpperBound;
        private int _yUpperBound;
        private Vector2 _offset;

        public TileSet(int tileSize)
        {
            _tileSize = tileSize;
        }

        public void SetOffset(Vector2 offset)
        {
            _offset = offset;
        }

        public void Draw()
        {
            foreach(var tile in GetTiles())
            {
                tile.Value.Draw(tile.Key);
            }
        }

        public IEnumerable<KeyValuePair<Vector2, ITile>> GetTiles()
        {
            for(int x=0;x<=_xUpperBound;x++)
            {
                for(int y=0;y<=_yUpperBound; y++)
                {
                    yield return new KeyValuePair<Vector2, ITile>(new Vector2(y, x) * _tileSize + _offset, _tiles[x, y]);
                }
            }
        }

        public void SetData(ITile[,] tiles)
        {
            _tiles = tiles;
            _xUpperBound = _tiles.GetUpperBound(0);
            _yUpperBound = _tiles.GetUpperBound(1);
        }
    }
}