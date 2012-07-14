using System;
using DigitalRune.Game.UI;
using DigitalRune.Game.UI.Controls;
using DigitalRune.Mathematics.Algebra;
using MasterOfCentauri.Model;
using MasterOfCentauri.UIControls.Base;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MasterOfCentauri.Managers;

namespace MasterOfCentauri.UIControls
{
    public class Planet : ClickableContentControl
    {
        protected readonly float Scale;
        private ContentController _content;
        protected Image ImageControl;

        public Planet(IServiceProvider services, PlanetMapPlanetViewModel viewModel)
        {
            ViewModel = viewModel;
            Scale = ViewModel.Scale;

            ImageControl = new Image();

            _content = (ContentController)services.GetService(typeof(ContentController));
            ImageControl.Texture = _content.GetContent<Texture2D>(@"PlanetView\Planet1");
            Content = ImageControl;
            RenderScale = new Vector2F(Scale);
            
        }

        public PlanetMapPlanetViewModel ViewModel { get; set; }

        public float CenterX
        {
            get
            {
                return X + (ImageControl.Texture.Width / 2f) * Scale;
            } 
            set
            {
                X = value - ImageControl.Texture.Width / 2f * Scale;
            }
        }
        public float CenterY
        {
            get
            {
                return Y + ImageControl.Texture.Height / 2f * Scale;
            }
            set
            {
                Y = value - ImageControl.Texture.Height / 2f * Scale;
            }
        }
    }
}