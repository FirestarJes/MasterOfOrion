using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterOfCentauri.Util
{
    class TextureAtlas
    {
        string _name;
        private Dictionary<string, Rectangle> _atlasCoords;
        private Texture2D _atlastTexture;

        public Dictionary<string, Rectangle> AtlasCoords
        {
            get { return _atlasCoords; }
            set { _atlasCoords = value; }
        }

        public Texture2D AtlasTexture
        {
            get { return _atlastTexture; }
            set { _atlastTexture = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

    }
}
