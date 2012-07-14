using System;
using DigitalRune.Game.UI;
using DigitalRune.Game.UI.Controls;
using DigitalRune.Mathematics.Algebra;
using MasterOfCentauri.Model;
using MasterOfCentauri.UIControls.Base;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MasterOfCentauri.UIControls
{
    public class Planet : ClickableContentControl
    {
        protected readonly float Scale;
        protected ContentManager ContentManager;
        protected Image ImageControl;

        public Planet(IServiceProvider services, PlanetMapPlanetViewModel viewModel)
        {
            ViewModel = viewModel;
            Scale = ViewModel.Scale;

            ImageControl = new Image();

            ContentManager = ((ContentManager)services.GetService(typeof(ContentManager)));
            ImageControl.Texture = ContentManager.Load<Texture2D>(@"PlanetView\Planet1");
            Content = ImageControl;
            RenderScale = new Vector2F(Scale);
            
        }

        public PlanetMapPlanetViewModel ViewModel { get; set; }

        public float CenterX
        {
            get
            {
                return X + (Width / 2f) * Scale;
            } 
            set
            {
                X = value - Width / 2f * Scale;
            }
        }
        public float CenterY
        {
            get
            {
                return Y + Height / 2f * Scale;
            }
            set
            {
                Y = value - Height / 2f * Scale;
            }
        }
    }
}