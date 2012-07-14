using System;
using DigitalRune.Game.Input;
using DigitalRune.Game.UI;
using DigitalRune.Game.UI.Controls;
using DigitalRune.Mathematics.Algebra;
using MasterOfCentauri.Model;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MasterOfCentauri.UIControls
{
    public class Planet : ContentControl
    {
        protected readonly float Scale;
        protected ContentManager ContentManager;
        protected Image ImageControl;

        public EventHandler Clicked;

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

        protected override void OnHandleInput(InputContext context)
        {
            base.OnHandleInput(context);

            IsClicked = false;

            if (InputService.IsDown(MouseButtons.Left) && IsMouseOver && IsDown == false)
            {
                InputService.IsMouseOrTouchHandled = true;
                IsClicked = true;
            }

            IsDown = InputService.IsDown(MouseButtons.Left);

            if (IsClicked && Clicked != null)
                Clicked(this, EventArgs.Empty);
        }
        protected bool IsClicked { get; set; }
        protected bool IsDown { get; set; }

    }
}