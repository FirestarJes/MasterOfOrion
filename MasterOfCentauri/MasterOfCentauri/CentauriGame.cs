using System;
using System.Collections.Generic;
using System.Linq;
using MasterOfCentauri.Model;
using MasterOfCentauri.UIControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using DigitalRune.Game.Input;
using DigitalRune.Game.UI;
using DigitalRune.Animation;
using DigitalRune.Game.UI.Controls;
using DigitalRune.Game.UI.Rendering;
using DigitalRune.Mathematics.Algebra;


namespace MasterOfCentauri
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class CentauriGame : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private readonly IInputService _inputService;
        private readonly IUIService _uiService;
        private readonly IAnimationService _animationService;
        private readonly SpriteBatch _spritebatch;
        // The UI screen renders our controls, such as text labels, buttons, etc.
        private UIScreen _screen;
        private ConsoleWindow _console;

        public CentauriGame(Game game)
            : base(game)
        {
            _inputService = (IInputService)game.Services.GetService(typeof(IInputService));
            _uiService = (IUIService)game.Services.GetService(typeof(IUIService));
            _animationService = (IAnimationService)game.Services.GetService(typeof(IAnimationService));
            _spritebatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Load content for this game component
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: Add your initialization code here
            var content = new ContentManager(Game.Services, "NeoforceTheme");
            Theme theme;
            theme = content.Load<Theme>("ThemeRed");

            // Create a UI renderer, which uses the theme info to renderer UI controls.
            var renderer = new UIRenderer(Game, theme);

            // Create a UIScreen and add it to the UI service. The screen is the root of 
            // the tree of UI controls. Each screen can have its own renderer. 
            _screen = new UIScreen("Default", renderer)
            {
                // Make the screen transparent.
                Background = new Color(0, 0, 0, 0),

            };

            var buttontest = new Button
            {
                Width = 80,
                Height = 30,
                X = 10,
                Y = 80
            };

            var test = new UIControls.MapRender(Game.Services)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = new Color(100, 56, 56, 56)
            };


            var mainTabPanel = new TabControl
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            var planetMap = new UIControls.PlanetMapControl(Game.Services)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                ViewData = new PlanetMapViewModel() { Rings = new[]
                                                                  {
                                                                      new PlanetMapRingViewModel() { Planets = new[]
                                                                                                                   {
                                                                                                                       new PlanetMapPlanetViewModel() { Name = "Planet 1", Scale = 1},
                                                                                                                       new PlanetMapPlanetViewModel() { Name = "Planet 1 - Moon", Scale = 0.4f},
                                                                                                                   }},
                                                                      new PlanetMapRingViewModel() { Planets = new[]
                                                                                                                   {
                                                                                                                       new PlanetMapPlanetViewModel() { Name = "Planet 2", Scale = 0.5f},
                                                                                                                   }},
                                                                      new PlanetMapRingViewModel() { Planets = new[]
                                                                                                                   {
                                                                                                                       new PlanetMapPlanetViewModel() { Name = "Planet 3", Scale = 0.7f},
                                                                                                                       new PlanetMapPlanetViewModel() { Name = "Planet 3 - Moon 1", Scale = 0.3f},
                                                                                                                       new PlanetMapPlanetViewModel() { Name = "Planet 3 - Moon 2", Scale = 0.3f},
                                                                                                                   }},
                                                                      new PlanetMapRingViewModel() { Planets = new[]
                                                                                                                   {
                                                                                                                       new PlanetMapPlanetViewModel() { Name = "Planet 4", Scale = 0.2f},
                                                                                                                   }},
                                                                      new PlanetMapRingViewModel() { Planets = new[]
                                                                                                                   {
                                                                                                                       new PlanetMapPlanetViewModel() { Name = "Planet 5", Scale = 0.9f},
                                                                                                                   }},
                                                                      new PlanetMapRingViewModel() { Planets = new[]
                                                                                                                   {
                                                                                                                       new PlanetMapPlanetViewModel() { Name = "Planet 6", Scale = 0.6f},
                                                                                                                   }},
                                                                      new PlanetMapRingViewModel() { Planets = new[]
                                                                                                                   {
                                                                                                                       new PlanetMapPlanetViewModel() { Name = "Planet 7", Scale = 0.3f},
                                                                                                                   }},
                                                                      new PlanetMapRingViewModel() { Planets = new[]
                                                                                                                   {
                                                                                                                       new PlanetMapPlanetViewModel() { Name = "Planet 8", Scale = 0.5f},
                                                                                                                   }},
                                                                      new PlanetMapRingViewModel() { Planets = new[]
                                                                                                                   {
                                                                                                                       new PlanetMapPlanetViewModel() { Name = "Planet 9", Scale = 0.8f},
                                                                                                                       new PlanetMapPlanetViewModel() { Name = "Planet 9 - Moon 1", Scale = 0.2f},
                                                                                                                       new PlanetMapPlanetViewModel() { Name = "Planet 9 - Moon 2", Scale = 0.2f},
                                                                                                                       new PlanetMapPlanetViewModel() { Name = "Planet 9 - Moon 3", Scale = 0.2f},
                                                                                                                       new PlanetMapPlanetViewModel() { Name = "Planet 9 - Moon 4", Scale = 0.3f},
                                                                                                                   }}
                                                                  }}
            };

            mainTabPanel.Items.Add(new TabItem() { Content = new TextBlock() { Text = "Star map" }, TabPage = test });
            mainTabPanel.Items.Add(new TabItem() { Content = new TextBlock() { Text = "Planet map" }, TabPage = planetMap });

            _console = new ConsoleWindow();
            
            _screen.Children.Add(mainTabPanel);
            _screen.Children.Add(_console);

            // Add the screen to the UI service.
            _uiService.Screens.Add(_screen);

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (_inputService.IsPressed(Keys.Insert, false))
            {
                _console.IsVisible = !_console.IsVisible;
                _inputService.IsKeyboardHandled = true;
            }
            base.Update(gameTime);
        }



        public override void Draw(GameTime gameTime)
        {
            _screen.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
