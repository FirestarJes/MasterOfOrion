using System;
using System.Collections.Generic;
using System.Linq;
using MasterOfCentauri.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using DigitalRune.Animation;
using DigitalRune.Game.Input;
using DigitalRune.Game.UI;

namespace MasterOfCentauri
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphicsManager;
        private SpriteBatch _spriteBatch;
        private InputManager _inputManager;
        private UIManager _uiManager;
        private AnimationManager _animationManager;
        private ConsoleManager _consoleManager;
        private ContentController _contentController;
        private GalaxyManager _galaxyManager;

        private readonly TimeSpan SampleInterval = new TimeSpan(0, 0, 0, 1);
        private TimeSpan _sampleTime;
        private float _numberOfFrames;

        public Game1()
        {
            _graphicsManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720,
            };
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
            DigitalRune.Licensing.AddSerialNumber("A4DsypZMsBsI8xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            _graphicsManager.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // The services are stored in Game.Services to make them accessible by all
            // game components.

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), _spriteBatch);

            Services.AddService(typeof(ContentManager), Content);

            // Add the input service, which manages device input, button presses, etc.
            _inputManager = new InputManager(false);
            Services.AddService(typeof(IInputService), _inputManager);

            // Add the UI service, which manages UI screens.
            _uiManager = new UIManager(this, _inputManager);
            Services.AddService(typeof(IUIService), _uiManager);

            // Add the animation service.
            _animationManager = new AnimationManager();
            Services.AddService(typeof(IAnimationService), _animationManager);

            _consoleManager = new ConsoleManager();
            Services.AddService(typeof(ConsoleManager), _consoleManager);

            _contentController = new ContentController(GraphicsDevice, Content);
            _contentController.LoadContent();
            Services.AddService(typeof(ContentController), _contentController);

            _galaxyManager = new GalaxyManager(Services);
            Services.AddService(typeof(GalaxyManager), _galaxyManager);

            // ----- Add GameComponents
            // The component that shows the individual screen.
            Components.Add(new CentauriGame(this));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
          

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var deltaTime = gameTime.ElapsedGameTime;
            if (IsActive)
            {
                // Update input manager. The input manager gets the device states and performs other work.
                _inputManager.Update(deltaTime);

                // Update UI manager. The UI manager updates all registered UIScreens and handles
                // button clicks, etc.
                _uiManager.Update(deltaTime);

                // Update game components.
                base.Update(gameTime);

                // Update the animations. The animations results are stored internally but not yet applied.
                _animationManager.Update(deltaTime);

                // Apply the animations. This method changes the animated objects.
                _animationManager.ApplyAnimations();
            }
           
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _sampleTime += gameTime.ElapsedGameTime;
            _numberOfFrames++;

            if (_sampleTime > SampleInterval)
            {
                var Text = string.Format("FPS: {0}", (int)(_numberOfFrames / (float)_sampleTime.TotalSeconds + 0.5f));
                _sampleTime = TimeSpan.Zero;
                _numberOfFrames = 0;
                Window.Title = Text;
            }

            base.Draw(gameTime);
        }
    }
}
