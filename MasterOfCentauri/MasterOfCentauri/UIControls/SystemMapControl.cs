using System;
using System.Collections.Generic;
using System.Diagnostics;
using DigitalRune.Game.UI.Consoles;
using DigitalRune.Mathematics.Algebra;
using MasterOfCentauri.Managers;
using MasterOfCentauri.Model;
using Microsoft.Xna.Framework.Graphics;
using DigitalRune.Game.UI.Controls;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace MasterOfCentauri.UIControls
{
    class SystemMapControl : ContentControl, IConsoleCommandHost
    {
        private readonly IServiceProvider _services;
        Texture2D _sun;
        Texture2D _ring;
        UIControl _sunControl;
        UIControl _ringControl;
        Canvas _panel;
        private TextBlock _testLabel;
        private ConsoleManager _console;
        private ContentController _content;

        public SystemMapControl(IServiceProvider services)
        {
            _services = services;
            Name = "SystemMap";
            _content = (ContentController)services.GetService(typeof(ContentController));
            _console = ((ConsoleManager)services.GetService(typeof(ConsoleManager)));
            Background = Color.Black;
            ClipContent = true;
            BaseRadius = 1210f;
            BaseDegrees = 60;
        }

        public PlanetMapViewModel ViewData { get; set; }

        protected override void OnLoad()
        {
            _sun = _content.GetContent<Texture2D>(@"PlanetView\Sun");
            _ring = _content.GetContent<Texture2D>(@"PlanetView\PlanetRings");
            
            _panel = new Canvas { HorizontalAlignment = DigitalRune.Game.UI.HorizontalAlignment.Stretch, VerticalAlignment = DigitalRune.Game.UI.VerticalAlignment.Stretch };
            LoadContent();

            _console.HookInto(this);

            base.OnLoad();
        }

        public void LoadContent()
        {
            _panel.Children.Clear();

            _sunControl = new Image() {Texture = _sun, X = -_sun.Width/2, Y = -_sun.Height/2};
            _ringControl = new Image() {Texture = _ring, Width = 1218, Height = 1218};
            _testLabel = new TextBlock() {Y = 500, X = 100, Text = "No Planet Selected", Font = "Fonts/Arial"};

            Content = _panel;
            _panel.Children.Add(_sunControl);
            _panel.Children.Add(_ringControl);
            _panel.Children.Add(_testLabel);

            var ringNumber = 1;
            foreach (var ring in ViewData.Rings)
            {
                var scale = ((ringNumber + 1))/10f;
                var ringRadius = BaseRadius*scale;
                float baseDegrees = BaseDegrees;

                foreach (var planet in ring.Planets)
                {
                    var control = new Planet(_services, planet)
                                      {CenterX = GetX(baseDegrees, ringRadius), CenterY = GetY(baseDegrees, ringRadius)};
                    control.Clicked += Clicked;
                    _panel.Children.Add(control);

                    baseDegrees += 4;
                }

                ringNumber++;
            }
        }

        protected override void OnUnload()
        {
            if (RemoveCommands != null)
                RemoveCommands(this, EventArgs.Empty);
            base.OnUnload();
        }

        public string Test()
        {
            return "Hello world";
        }

        private void Clicked(object sender, EventArgs eventArgs)
        {
            var planet = (Planet) sender;
            _testLabel.Text = planet.ViewModel.Name;

            _console.WriteLine(planet.ViewModel.Name + " Selected");
        }

        private float GetY(float degrees, float radius)
        {
            return (float)Math.Cos(MathHelper.ToRadians(degrees)) * radius;
        }

        private float GetX(float degrees, float radius)
        {
            return (float) Math.Sin(MathHelper.ToRadians(degrees))*radius;
        }

        public IEnumerable<ConsoleCommand> Commands
        {
            get
            {
                return new ConsoleCommand[]
                           {
                               new ConsoleCommand("Test", "Test", x=> Debug.WriteLine("Testing"))
                           };
            }
        }

        public float BaseRadius { get; set; }
        public float BaseDegrees { get; set; }

        public event EventHandler RemoveCommands;
    }
}
