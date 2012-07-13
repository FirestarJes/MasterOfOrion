using System;
using System.Collections.Generic;
using System.Linq;
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
            };

            mainTabPanel.Items.Add(new TabItem() { Content = new TextBlock() { Text = "Star map" }, TabPage = test });
            mainTabPanel.Items.Add(new TabItem() { Content = new TextBlock() { Text = "Planet map" }, TabPage = planetMap });


            var paneltest = new Canvas
            {
                Margin = new Vector4F(8),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 120,
                Height = 120,
                
                Background = new Color(255, 25, 25, 25)

            };
            //paneltest.Children.Add(buttontest);
            _screen.Children.Add(mainTabPanel);
            //_screen.Children.Add(paneltest);
            
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
            // TODO: Add your update code here


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _screen.Draw(gameTime);
            
            base.Draw(gameTime);
        }
    }
}
