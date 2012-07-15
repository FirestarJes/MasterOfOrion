using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework;

namespace MasterOfCentauri.Managers
{
    class ContentController
    {
        GraphicsDevice _graph;
        ContentManager _content;
        Dictionary<string, object> _contentDic;
        Dictionary<string, Util.TextureAtlas> _textureAtlases;
        List<Util.TextureAtlas> _starTextureAtlases;
        List<Util.TextureAtlas> _gasTextureAtlases;
        


        public ContentController(GraphicsDevice graph, ContentManager content)
        {
            _graph = graph;
            _content = content;
            _contentDic = new Dictionary<string, object>();
            _starTextureAtlases = new List<Util.TextureAtlas>();
            _gasTextureAtlases = new List<Util.TextureAtlas>();
        }

        public T GetContent<T>(string contentName)
        {
            if (_contentDic.ContainsKey(contentName))
            {
                return (T)_contentDic[contentName];
            }
            else
            {
                T cont = _content.Load<T>(contentName);
                _contentDic.Add(contentName, cont);
                return (T)_contentDic[contentName];
            }
        }

        public Util.TextureAtlas getTextureAtlas(string atlasName)
        {
            if (_textureAtlases.ContainsKey(atlasName))
            {
                return _textureAtlases[atlasName];
            }
            else
            {
                _textureAtlases.Add(atlasName, LoadTextureAtlas(atlasName));
                return _textureAtlases[atlasName];
            }
        }


        private Util.TextureAtlas LoadTextureAtlas(string atlasName)
        {
            Util.TextureAtlas atlas = new Util.TextureAtlas();

            StreamReader sr = new StreamReader(_content.RootDirectory + "\\TextureAtlases\\galaxy.atlas");
            atlas.AtlasCoords = new Dictionary<string, Rectangle>();
            atlas.AtlasTexture = GetContent<Texture2D>(atlasName);
            string line = sr.ReadLine();
            do
            {
                string textureName = line.Split(' ')[0];
                atlas.AtlasCoords.Add(textureName, new Rectangle(int.Parse(line.Split(' ')[1].Split(';')[0]), int.Parse(line.Split(' ')[1].Split(';')[1]), 256, 256));

                line = sr.ReadLine();
            }
            while (!string.IsNullOrEmpty(line));
            return atlas;
        }

        public void LoadStarTextureAtlas(string atlasName)
        {
            Util.TextureAtlas atlas = new Util.TextureAtlas();

            StreamReader sr = new StreamReader(_content.RootDirectory + "\\TextureAtlases\\" + atlasName + ".atlas");
            atlas.AtlasCoords = new Dictionary<string, Rectangle>();
            atlas.AtlasTexture = GetContent<Texture2D>(@"Stars\" + atlasName);
            atlas.Name = atlasName;
            string line = sr.ReadLine();
            do
            {
                string textureName = line.Split(' ')[0];
                atlas.AtlasCoords.Add(textureName, new Rectangle(int.Parse(line.Split(' ')[1].Split(';')[0]), int.Parse(line.Split(' ')[1].Split(';')[1]), 256, 256));

                line = sr.ReadLine();
            }
            while (!string.IsNullOrEmpty(line));
            _starTextureAtlases.Add(atlas);
        }

        public void LoadGasTextureAtlas(string atlasName)
        {
            Util.TextureAtlas atlas = new Util.TextureAtlas();

            StreamReader sr = new StreamReader(_content.RootDirectory + "\\TextureAtlases\\" + atlasName + ".atlas");
            atlas.AtlasCoords = new Dictionary<string, Rectangle>();
            atlas.AtlasTexture = GetContent<Texture2D>(atlasName);
            string line = sr.ReadLine();
            do
            {
                string textureName = line.Split(' ')[0];
                atlas.AtlasCoords.Add(textureName, new Rectangle(int.Parse(line.Split(' ')[1].Split(';')[0]), int.Parse(line.Split(' ')[1].Split(';')[1]), 256, 256));

                line = sr.ReadLine();
            }
            while (!string.IsNullOrEmpty(line));
            _gasTextureAtlases.Add(atlas);
        }

        public void GenerateStarsAtlasFromFolder(string folder)
        {
            throw new NotImplementedException();
        }

        public Util.TextureAtlas getStarAtlasFromTextureName(string starTextureName)
        {
            foreach (Util.TextureAtlas atlas in _starTextureAtlases)
            {
                if (atlas.AtlasCoords.ContainsKey(starTextureName))
                {
                    return atlas; 
                }
            }
            return null;
        }
    }
}
