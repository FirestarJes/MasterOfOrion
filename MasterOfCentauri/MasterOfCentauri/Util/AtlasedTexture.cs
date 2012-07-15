using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MasterOfCentauri.Util
{
    class AtlasedTexture
    {
        string _textureName;
        Vector2 _textureCoords;


        public string TextureName
        {
            get { return _textureName; }
            set { _textureName = value; }
        }

        public Vector2 TextureCoords
        {
            get { return _textureCoords; }
            set { _textureCoords = value; }
        }
    }
}
