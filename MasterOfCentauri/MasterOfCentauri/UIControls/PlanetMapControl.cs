using System;
using System.Collections.Generic;
using System.Linq;
using DigitalRune.Game.UI.Consoles;
using DigitalRune.Game.UI.Controls;
using MasterOfCentauri.Managers;
using MasterOfCentauri.Model.PlanetMap;
using Microsoft.Xna.Framework;

namespace MasterOfCentauri.UIControls
{
    public class PlanetMapControl : ContentControl, IConsoleCommandHost
    {
        private readonly IServiceProvider _services;
        private ContentController _content;
        Canvas _panel;

        public PlanetMapViewModel ViewData { get; set; }

        public PlanetMapControl(IServiceProvider services)
        {
            _services = services;
            _content = (ContentController)services.GetService(typeof(ContentController));
            
            Name = "PlanetMap";
            Background = Color.Black;
            ClipContent = true;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            _panel = new Canvas { HorizontalAlignment = DigitalRune.Game.UI.HorizontalAlignment.Stretch, VerticalAlignment = DigitalRune.Game.UI.VerticalAlignment.Stretch };
            Content = _panel;
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            if(RemoveCommands != null)
                RemoveCommands(this, EventArgs.Empty);
        }

        public void LoadContent()
        {
            _panel.Children.Clear();
        }

        public IEnumerable<ConsoleCommand> Commands { get { return Enumerable.Empty<ConsoleCommand>(); } }
        public event EventHandler RemoveCommands;
    }
}