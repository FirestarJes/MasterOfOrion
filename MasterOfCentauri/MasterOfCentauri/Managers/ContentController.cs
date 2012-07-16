using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework;
using sspack;

namespace MasterOfCentauri.Managers
{
    class ContentController
    {
        GraphicsDevice _graph;
        ContentManager _content;
        Dictionary<string, object> _contentDic;
        List<Util.TextureAtlas> _starTextureAtlases;
        List<Util.TextureAtlas> _gasTextureAtlases;
        
        const string _BASE_MOD_DIR = "mods";

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

        public void LoadContent()
        {
            LoadStarTextures();
        }
        
        private void LoadStarTextures()
        {
            ImagePacker packer = new ImagePacker();
            List<string> files = new List<string>();
            Dictionary<string, Rectangle> map = new Dictionary<string, Rectangle>();
            System.Drawing.Bitmap packedImage = new System.Drawing.Bitmap(Constants.DefaultMaximumSheetWidth, Constants.DefaultMaximumSheetHeight); 
            foreach (string file in Directory.EnumerateFiles(Path.Combine(_content.RootDirectory, "base", "stars"), "*.png"))
            {
                files.Add(file);
            }
            if (Directory.Exists(Path.Combine(_content.RootDirectory, _BASE_MOD_DIR, "testmod", "stars")))
            {
                foreach (string file in Directory.EnumerateFiles(Path.Combine(_content.RootDirectory, _BASE_MOD_DIR, "testmod", "stars"), "*.png"))
                {
                    files.Add(file);
                }
            }
          
            packer.PackImage(files, false, true, Constants.DefaultMaximumSheetWidth, Constants.DefaultMaximumSheetHeight, Constants.DefaultImagePadding, true, out packedImage, out map);
            Util.TextureAtlas atlas = new Util.TextureAtlas();
            atlas.AtlasCoords = map;
            atlas.Name = "Stars";
            int bufferSize = packedImage.Height * packedImage.Width * 4; 
            System.IO.MemoryStream memoryStream =  new System.IO.MemoryStream(bufferSize);  
	        packedImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);  
	 
	        // Creates a texture from IO.Stream - our memory stream  
	        Texture2D texture = Texture2D.FromStream(_graph, memoryStream);
            atlas.AtlasTexture = texture;
            _starTextureAtlases.Add(atlas);
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
