using System;
using System.Collections.Generic;
using DigitalRune.Game.UI.Consoles;
using DigitalRune.Game.UI.Controls;
using MasterOfCentauri.Managers;

namespace MasterOfCentauri.UIControls
{
    public class PlanetMapControl : ContentControl, IConsoleCommandHost
    {
        protected override void OnUnload()
        {
            if(RemoveCommands != null)
                RemoveCommands(this, EventArgs.Empty);
            
            base.OnUnload();
        }

        public IEnumerable<ConsoleCommand> Commands { get; private set; }
        public event EventHandler RemoveCommands;
    }
}