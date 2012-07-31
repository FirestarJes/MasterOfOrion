using System;
using System.Collections.Generic;
using System.Linq;
using MasterOfCentauri.Managers;
using MasterOfCentauri.Model;
using MasterOfCentauri.Model.PlanetMap;
using MasterOfCentauri.Model.SystemMap;
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
using MasterOfCentauri.GameScreens;


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
        private readonly ConsoleManager _consoleManager;
        private readonly ContentController _content;
        private readonly EventManager _eventManager;

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
            _consoleManager = (ConsoleManager)game.Services.GetService(typeof(ConsoleManager));
            _content = (ContentController)game.Services.GetService(typeof(ContentController));
            _eventManager = (EventManager) game.Services.GetService(typeof (EventManager));
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
            var test = new Canvas()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };

            var maprender = new UIControls.GalaxyMapControl(Game.Services)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            var minimap = new UIControls.MinimapControl(Game.Services);
            var miniMapWindow = new MiniMapWindow()
            {
                Height = 200,
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Name = "MiniMap"
            };
            var controlButtonPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var testButton = new Button()
            {
                Height = 40,
                Width = 200
            };
            controlButtonPanel.Children.Add(testButton);
            miniMapWindow.Content = minimap;
            test.Children.Add(maprender);
            test.Children.Add(controlButtonPanel);
            test.Children.Add(miniMapWindow);
            
            
            var mainTabPanel = new TabControl
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            var systemMap = new UIControls.SystemMapControl(Game.Services)
            {
                Opacity = 5,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                ViewData = new SystemMapViewModel() { Rings = new[]
                                                                  {
                                                                      new SystemMapRingViewModel() { Planets = new[]
                                                                                                                   {
                                                                                                                       new SystemMapPlanetViewModel() { Name = "Planet 1", Scale = 1},
                                                                                                                       new SystemMapPlanetViewModel() { Name = "Planet 1 - Moon", Scale = 0.4f},
                                                                                                                   }},
                                                                      new SystemMapRingViewModel() { Planets = new[]
                                                                                                                   {
                                                                                                                       new SystemMapPlanetViewModel() { Name = "Planet 2", Scale = 0.5f},
                                                                                                                   }},
                                                                      new SystemMapRingViewModel() { Planets = new[]
                                                                                                                   {
                                                                                                                       new SystemMapPlanetViewModel() { Name = "Planet 3", Scale = 0.7f},
                                                                                                                       new SystemMapPlanetViewModel() { Name = "Planet 3 - Moon 1", Scale = 0.3f},
                                                                                                                       new SystemMapPlanetViewModel() { Name = "Planet 3 - Moon 2", Scale = 0.3f},
                                                                                                                   }},
                                                                      new SystemMapRingViewModel() { Planets = new[]
                                                                                                                   {
                                                                                                                       new SystemMapPlanetViewModel() { Name = "Planet 4", Scale = 0.2f},
                                                                                                                   }},
                                                                      new SystemMapRingViewModel() { Planets = new[]
                                                                                                                   {
                                                                                                                       new SystemMapPlanetViewModel() { Name = "Planet 5", Scale = 0.9f},
                                                                                                                   }},
                                                                      new SystemMapRingViewModel() { Planets = new[]
                                                                                                                   {
                                                                                                                       new SystemMapPlanetViewModel() { Name = "Planet 6", Scale = 0.6f},
                                                                                                                   }},
                                                                      new SystemMapRingViewModel() { Planets = new[]
                                                                                                                   {
                                                                                                                       new SystemMapPlanetViewModel() { Name = "Planet 7", Scale = 0.3f},
                                                                                                                   }},
                                                                      new SystemMapRingViewModel() { Planets = new[]
                                                                                                                   {
                                                                                                                       new SystemMapPlanetViewModel() { Name = "Planet 8", Scale = 0.5f},
                                                                                                                   }},
                                                                      new SystemMapRingViewModel() { Planets = new[]
                                                                                                                   {
                                                                                                                       new SystemMapPlanetViewModel() { Name = "Planet 9", Scale = 0.8f},
                                                                                                                       new SystemMapPlanetViewModel() { Name = "Planet 9 - Moon 1", Scale = 0.2f},
                                                                                                                       new SystemMapPlanetViewModel() { Name = "Planet 9 - Moon 2", Scale = 0.2f},
                                                                                                                       new SystemMapPlanetViewModel() { Name = "Planet 9 - Moon 3", Scale = 0.2f},
                                                                                                                       new SystemMapPlanetViewModel() { Name = "Planet 9 - Moon 4", Scale = 0.3f},
                                                                                                                   }}
                                                                  }}
            };

            var planetMap = new PlanetMapControl(Game.Services)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                ViewData = new PlanetMapViewModel()
                               {
                                   Name = "Earth",
                                   BuildingCells = new[,]
                                                       {
                                                           { new BuildingCellViewModel(), new BuildingCellViewModel(), new BuildingCellViewModel() },
                                                           { new BuildingCellViewModel(), new BuildingCellViewModel(), new BuildingCellViewModel() },
                                                           { new BuildingCellViewModel(), new BuildingCellViewModel(), new BuildingCellViewModel() }
                                                       }
                               }
            };

            mainTabPanel.Items.Add(new TabItem() { Content = new TextBlock() { Text = "Star map" }, TabPage = test });
            mainTabPanel.Items.Add(new TabItem() { Content = new TextBlock() { Text = "System map" }, TabPage = systemMap });
            mainTabPanel.Items.Add(new TabItem() { Content = new TextBlock() { Text = "Planet map" }, TabPage = planetMap });

            _console = new ConsoleWindow(_consoleManager);

            StartMenuScreen start = new StartMenuScreen(Game.Services);
            start.Init();
            _screen.Children.Add(start);


            //_screen.Children.Add(mainTabPanel);
            //_screen.Children.Add(_console);
            //_screen.Children.Add(miniMapWindow);
            //_screen.Children.Add(maprender);

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

            _eventManager.Execute(EventType.Update);

            base.Update(gameTime);
        }



        public override void Draw(GameTime gameTime)
        {
            _screen.Draw(gameTime);

            _eventManager.Execute(EventType.Frame);

            base.Draw(gameTime);
        }
    }
}
