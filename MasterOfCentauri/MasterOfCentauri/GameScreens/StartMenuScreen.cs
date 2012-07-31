using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DigitalRune.Game.UI.Controls;
using DigitalRune.Game.UI.Rendering;
using MasterOfCentauri.Managers;
using Microsoft.Xna.Framework.Graphics;
using MasterOfCentauri.UIControls.Base;

namespace MasterOfCentauri.GameScreens
{
    class StartMenuScreen: GameScreenBase 
    {
        Button _newGameButton;
        Button _exitButton;
        Image _background;
        SpacedStackPanel _menuItems;
        Canvas _layout;

        public StartMenuScreen(IServiceProvider services) : base(services) { }
        
        public void Init()
        {
            _layout = new Canvas()
            {
                HorizontalAlignment = DigitalRune.Game.UI.HorizontalAlignment.Stretch,
                VerticalAlignment = DigitalRune.Game.UI.VerticalAlignment.Stretch
            };
            

            _background = new Image()
            {
                HorizontalAlignment = DigitalRune.Game.UI.HorizontalAlignment.Stretch,
                VerticalAlignment = DigitalRune.Game.UI.VerticalAlignment.Stretch,
                Texture = _content.GetContent<Texture2D>("UI/StartScreenBG")
            };

            _menuItems = new SpacedStackPanel()
            {
                HorizontalAlignment = DigitalRune.Game.UI.HorizontalAlignment.Center, 
                VerticalAlignment = DigitalRune.Game.UI.VerticalAlignment.Center
                
            };

            _newGameButton = new Button()
            {
                Width = 160,
                Height = 30,
                Content = new TextBlock() {
                    Text = "New Game"
                }
            };

            _exitButton = new Button()
            {
                Width = 160,
                Height = 30,
                Content = new TextBlock() {
                    Text = "Exit"
                }
            };

            _menuItems.Children.Add(_newGameButton);
            _menuItems.Children.Add(_exitButton);

            _layout.Children.Add(_background);
            _layout.Children.Add(_menuItems);
            this.Content = _layout;
        }
    }

    
}
