using System;
using DigitalRune.Game.UI.Controls;
using DigitalRune.Mathematics.Algebra;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MasterOfCentauri.UIControls
{
    public class Planet : Image
    {
        protected readonly float Scale;
        protected ContentManager Content;
        
        public Planet(IServiceProvider services, string texture, float scale)
        {
            Scale = scale;
            Content = ((ContentManager)services.GetService(typeof(ContentManager)));
            Texture = Content.Load<Texture2D>(texture);
            RenderScale = new Vector2F(Scale);
        }

        public float CenterX
        {
            get
            {
                return X + (Texture.Width/2f)*Scale;
            } 
            set
            {
                X = value - Texture.Width / 2f * Scale;
            }
        }
        public float CenterY
        {
            get
            {
                return Y + Texture.Height / 2f * Scale;
            }
            set
            {
                Y = value - Texture.Height / 2f * Scale;
            }
        }

    }
}