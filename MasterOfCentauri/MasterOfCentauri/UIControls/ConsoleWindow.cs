using System;
using DigitalRune.Game.UI;
using DigitalRune.Game.UI.Consoles;
using DigitalRune.Game.UI.Controls;
using MasterOfCentauri.Managers;
using Microsoft.Xna.Framework.Input;

namespace MasterOfCentauri.UIControls
{
    // Displays an interactive console.
    public class ConsoleWindow : Window
    {
        public ConsoleWindow(ConsoleManager consoleManager)
        {
            Title = "Console";
            Width = 480;
            Height = 240;

            Content = consoleManager.Console;

            IsVisible = false;
            CanResize = true;
            HideOnClose = true;
        }

        protected override void OnHandleInput(InputContext context)
        {
            base.OnHandleInput(context);
        }
    }
}