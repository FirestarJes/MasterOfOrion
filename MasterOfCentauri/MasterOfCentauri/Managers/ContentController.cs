using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework;
using sspack;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace MasterOfCentauri.Managers
{
    class ContentController
    {
        GraphicsDevice _graph;
        ContentManager _content;
        Dictionary<string, object> _contentDic;
        List<Util.TextureAtlas> _starTextureAtlases;
        List<Util.TextureAtlas> _galaxyDecorationTextureAtlases;
        BlendState _blendColor, _blendAlpha;
        
        const string _BASE_MOD_DIR = "mods";

        public ContentController(GraphicsDevice graph, ContentManager content)
        {
            _graph = graph;
            _content = content;
            _contentDic = new Dictionary<string, object>();
            _starTextureAtlases = new List<Util.TextureAtlas>();
            _galaxyDecorationTextureAtlases = new List<Util.TextureAtlas>();

            //Multiply each color by the source alpha, and write in just the color values into the final texture
            BlendState _blendColor = new BlendState();
            _blendColor.ColorWriteChannels = ColorWriteChannels.Red | ColorWriteChannels.Green | ColorWriteChannels.Blue;

            _blendColor.AlphaDestinationBlend = Blend.Zero;
            _blendColor.ColorDestinationBlend = Blend.Zero;

            _blendColor.AlphaSourceBlend = Blend.SourceAlpha;
            _blendColor.ColorSourceBlend = Blend.SourceAlpha;

            BlendState _blendAlpha = new BlendState();
            _blendAlpha.ColorWriteChannels = ColorWriteChannels.Alpha;

            _blendAlpha.AlphaDestinationBlend = Blend.Zero;
            _blendAlpha.ColorDestinationBlend = Blend.Zero;

            _blendAlpha.AlphaSourceBlend = Blend.One;
            _blendAlpha.ColorSourceBlend = Blend.One;
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
            LoadGalaxyDecorationTextures(); 
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
            atlas.Name = "Stars" + _starTextureAtlases.Count;
            int bufferSize = packedImage.Height * packedImage.Width * 4; 
            System.IO.MemoryStream memoryStream =  new System.IO.MemoryStream(bufferSize);
            System.Drawing.Imaging.EncoderParameters test = new System.Drawing.Imaging.EncoderParameters();
            
            packedImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            Texture2D texture;
            texture = Texture2D.FromStream(_graph, memoryStream);
            PreMultiplyAlphas(texture);
            atlas.AtlasTexture = texture;
            _galaxyDecorationTextureAtlases.Add(atlas);
            _starTextureAtlases.Add(atlas);
        }

        private void LoadGalaxyDecorationTextures()
        {
            ImagePacker packer = new ImagePacker();
            List<string> files = new List<string>();
            Dictionary<string, Rectangle> map = new Dictionary<string, Rectangle>();
            System.Drawing.Bitmap packedImage = new System.Drawing.Bitmap(Constants.DefaultMaximumSheetWidth, Constants.DefaultMaximumSheetHeight);
            foreach (string file in Directory.EnumerateFiles(Path.Combine(_content.RootDirectory, "base", "galaxyDecoration"), "*.png"))
            {
                files.Add(file);
            }
            if (Directory.Exists(Path.Combine(_content.RootDirectory, _BASE_MOD_DIR, "testmod", "galaxyDecoration")))
            {
                foreach (string file in Directory.EnumerateFiles(Path.Combine(_content.RootDirectory, _BASE_MOD_DIR, "testmod", "galaxyDecoration"), "*.png"))
                {
                    files.Add(file);
                }
            }

            packer.PackImage(files, false, true, Constants.DefaultMaximumSheetWidth, Constants.DefaultMaximumSheetHeight, Constants.DefaultImagePadding, true, out packedImage, out map);
            Util.TextureAtlas atlas = new Util.TextureAtlas();
            atlas.AtlasCoords = map;
            atlas.Name = "GalaxyDecoration" + _galaxyDecorationTextureAtlases.Count ;
            int bufferSize = packedImage.Height * packedImage.Width * 4;
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(bufferSize);
            packedImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            Texture2D texture;
            texture = Texture2D.FromStream(_graph, memoryStream);
            PreMultiplyAlphas(texture);
            atlas.AtlasTexture = texture;
            _galaxyDecorationTextureAtlases.Add(atlas);
        }

        private static void PreMultiplyAlphas(Texture2D ret)
        {
            Byte4[] data = new Byte4[ret.Width * ret.Height];
            ret.GetData<Byte4>(data);
            for (int i = 0; i < data.Length; i++)
            {
                Vector4 vec = data[i].ToVector4();
                float alpha = vec.W / 255.0f;
                int a = (int)(vec.W);
                int r = (int)(alpha * vec.X);
                int g = (int)(alpha * vec.Y);
                int b = (int)(alpha * vec.Z);
                uint packed = (uint)(
                    (a << 24) +
                    (b << 16) +
                    (g << 8) +
                    r
                    );

                data[i].PackedValue = packed;
            }
            ret.SetData<Byte4>(data);
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

        public Util.TextureAtlas getGalaxyDecorationAtlasFromTextureName(string textureName)
        {
            foreach (Util.TextureAtlas atlas in _galaxyDecorationTextureAtlases)
            {
                if (atlas.AtlasCoords.ContainsKey(textureName))
                {
                    return atlas;
                }
            }
            return null;
        }
    }
}
