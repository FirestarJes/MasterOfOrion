using System;
using System.Diagnostics;
using DigitalRune.Mathematics.Algebra;
using Microsoft.Xna.Framework.Graphics;
using DigitalRune.Game.UI.Controls;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace MasterOfCentauri.UIControls
{
    class PlanetMapControl : ContentControl
    {
        Texture2D _sun;
        Texture2D _ring;
        Texture2D _planet;
        ContentManager _content;
        UIControl _sunControl;
        UIControl _ringControl;
        UIControl _ringControl2;
        UIControl _ringControl3;
        UIControl _ringControl4;
        UIControl _planet1Control;
        UIControl _planet2Control;
        UIControl _planet3Control;
        UIControl _planet4Control;
        Canvas _panel;
        private float radius1 = 700f;
        private float radius2 = 700f * 0.8f;
        private float radius3 = 700f * 0.6f;
        private float radius4 = 700f * 0.4f;
        private float degrees = 45f;

        public PlanetMapControl(IServiceProvider services)
        {
            Name = "PlanetMapControl";
            _content = ((ContentManager)services.GetService(typeof(ContentManager)));
            Background = Color.Black;
            ClipContent = true;
        }

        protected override void OnLoad()
        {
            _sun = _content.Load<Texture2D>(@"PlanetView\Sun");
            _ring = _content.Load<Texture2D>(@"PlanetView\PlanetRing");
            _planet = _content.Load<Texture2D>(@"PlanetView\Planet1");


            _panel = new Canvas { HorizontalAlignment = DigitalRune.Game.UI.HorizontalAlignment.Stretch, VerticalAlignment = DigitalRune.Game.UI.VerticalAlignment.Stretch };
            _sunControl = new Image() { Texture = _sun, X = -_sun.Width / 2, Y = -_sun.Height / 2 };
            _ringControl = new Image() { Texture = _ring };
            _ringControl2 = new Image() { Texture = _ring, RenderScale = new Vector2F(0.8f)  };
            _ringControl3 = new Image() { Texture = _ring, RenderScale = new Vector2F(0.6f) };
            _ringControl4 = new Image() { Texture = _ring, RenderScale = new Vector2F(0.4f) };
            _planet1Control = new Image() { Texture = _planet, X = radius1, Y = radius1, Opacity = 1 };
            _planet2Control = new Image() { Texture = _planet, X = radius2, Y = radius2, Opacity = 1 };
            _planet3Control = new Image() { Texture = _planet, X = radius3, Y = radius3, Opacity = 1 };

            _planet4Control = new Image() { Texture = _planet, X = GetX(degrees, radius4), Y = GetY(degrees, radius4), Opacity = 1 };

            Content = _panel;
            _panel.Children.Add(_sunControl);
            _panel.Children.Add(_ringControl);
            _panel.Children.Add(_ringControl2);
            _panel.Children.Add(_ringControl3);
            _panel.Children.Add(_ringControl4);
            _panel.Children.Add(_planet1Control);
            _panel.Children.Add(_planet2Control);
            _panel.Children.Add(_planet3Control);
            _panel.Children.Add(_planet4Control);


            base.OnLoad();
        }

        private float GetY(float degrees, float radius4)
        {
            return (float)Math.Cos(MathHelper.ToRadians(degrees)) * radius4 - _planet.Height / 2;
        }

        private float GetX(float degrees, float radius4)
        {
            return (float)Math.Sin(MathHelper.ToRadians(degrees)) * radius4 - _planet.Width / 2;
        }

        protected override void OnUpdate(TimeSpan deltaTime)
        {
            if (_planet1Control.IsMouseOver)
            {
                Debug.WriteLine("SHOW DIALOG");
            }

            //degrees += 0.1f;
            _planet4Control.X = GetX(degrees + 10, radius4);
            _planet4Control.Y = GetY(degrees + 10, radius4);
            _planet3Control.X = GetX(degrees - 10, radius3);
            _planet3Control.Y = GetY(degrees - 10, radius3);
            _planet2Control.X = GetX(degrees + 5, radius2);
            _planet2Control.Y = GetY(degrees + 5, radius2);
            _planet1Control.X = GetX(degrees, radius1);
            _planet1Control.Y = GetY(degrees, radius1);

            base.OnUpdate(deltaTime);
        }

        protected override void OnRender(UIRenderContext context)
        {
            base.OnRender(context);
        }

        protected override void OnHandleInput(DigitalRune.Game.UI.Controls.InputContext context)
        {
            base.OnHandleInput(context);
        }
    }
}
