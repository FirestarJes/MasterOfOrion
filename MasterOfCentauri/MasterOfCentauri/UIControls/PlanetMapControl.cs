using System;
using System.Diagnostics;
using DigitalRune.Mathematics.Algebra;
using MasterOfCentauri.Model;
using Microsoft.Xna.Framework.Graphics;
using DigitalRune.Game.UI.Controls;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace MasterOfCentauri.UIControls
{
    class PlanetMapControl : ContentControl
    {
        private readonly IServiceProvider _services;
        Texture2D _sun;
        Texture2D _ring;
        Texture2D _planet;
        ContentManager _content;
        UIControl _sunControl;
        UIControl _ringControl;
        Canvas _panel;
        private float base_radius = 1210f;

        public PlanetMapControl(IServiceProvider services)
        {
            _services = services;
            Name = "PlanetMapControl";
            _content = ((ContentManager)services.GetService(typeof(ContentManager)));
            Background = Color.Black;
            ClipContent = true;
        }

        public PlanetMapViewModel ViewData { get; set; }

        protected override void OnLoad()
        {
            _sun = _content.Load<Texture2D>(@"PlanetView\Sun");
            _ring = _content.Load<Texture2D>(@"PlanetView\PlanetRings");
            
            _panel = new Canvas { HorizontalAlignment = DigitalRune.Game.UI.HorizontalAlignment.Stretch, VerticalAlignment = DigitalRune.Game.UI.VerticalAlignment.Stretch };
            _sunControl = new Image() { Texture = _sun, X = -_sun.Width / 2, Y = -_sun.Height / 2 };
            _ringControl = new Image() { Texture = _ring, Width=1218, Height=1218 };


            Content = _panel;
            _panel.Children.Add(_sunControl);
            _panel.Children.Add(_ringControl);

            var ringNumber = 1;
            foreach(var ring in ViewData.Rings)
            {
                var scale = ((ringNumber + 1)) / 10f;
                var ringRadius = base_radius * scale;
                float baseDegrees = 60;

                foreach(var planet in ring.Planets)
                {
                    _panel.Children.Add(new Planet(_services, @"PlanetView\Planet1", planet.Scale) { CenterX = GetX(baseDegrees, ringRadius), CenterY = GetY(baseDegrees, ringRadius) });
                    baseDegrees += 4;
                }

                ringNumber++;
            }

            base.OnLoad();
        }

        private float GetY(float degrees, float radius)
        {
            return (float)Math.Cos(MathHelper.ToRadians(degrees)) * radius;
        }

        private float GetX(float degrees, float radius)
        {
            return (float) Math.Sin(MathHelper.ToRadians(degrees))*radius;
        }
    }
}
