using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using DigitalRune.Game.UI.Controls;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using DigitalRune.Game.UI.Rendering;

namespace MasterOfCentauri.UIControls
{
    class PlanetMapControl : DigitalRune.Game.UI.Controls.ContentControl
    {
        SpriteBatch _spritebatch;
        private SpriteFont arialFont;
        private Texture2D sun;
        private ContentManager _content;
        Texture2D _black;
        UIControl _sunControl;
        Panel panel;

        public PlanetMapControl(IServiceProvider services)
        {
            Name = "PlanetMapControl";
            _spritebatch = (SpriteBatch)services.GetService(typeof(SpriteBatch));
            _content = ((ContentManager)services.GetService(typeof(ContentManager)));
            Background = Color.Black;
            ClipContent = true;
        }

        protected override void OnLoad()
        {
            arialFont = _content.Load<SpriteFont>(@"Fonts\Spritefont1");
            sun = _content.Load<Texture2D>(@"PlanetView\Sun");

            panel = new Canvas { HorizontalAlignment = DigitalRune.Game.UI.HorizontalAlignment.Stretch, VerticalAlignment = DigitalRune.Game.UI.VerticalAlignment.Stretch };
            _sunControl = new Image() { Texture = sun, X = -sun.Width / 2, Y = -sun.Height / 2 };
            Content = panel;
            panel.Children.Add(_sunControl);
            base.OnLoad();
        }

        protected override void OnUpdate(TimeSpan deltaTime)
        {
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
