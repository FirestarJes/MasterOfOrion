using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MasterOfCentauri.Managers
{
    class GameManager
    {
        private Model.Galaxy _gameGalaxy;
        private Camera.Camera2D _galaxyCam;
        private ConsoleManager _consoleManager;
        private GalaxyManager _galaxyManager;


        #region Initialization
        public GameManager(IServiceProvider services)
        {
            //Load the services we need
            LoadServices(services);
        }

        private void LoadServices(IServiceProvider services)
        {
            _consoleManager = (ConsoleManager)services.GetService(typeof(ConsoleManager));
            _galaxyManager = (GalaxyManager)services.GetService(typeof(GalaxyManager));
        }

        #endregion

        #region Members
        public Model.Galaxy Galaxy
        {
            get { return _gameGalaxy; }
        }

        public Camera.Camera2D GalaxyCam
        {
            get { return _galaxyCam; }
        }
        #endregion

        public void NewGame()
        {

            //Generate the galaxy
            SetupGalaxy();

            //Setup the Galaxy Camera
            SetupGalaxyCamera();
        }

        private void SetupGalaxy()
        {
            _gameGalaxy = _galaxyManager.GenerateIrregularGalaxy(new Model.GalaxySetup() { Height = 2048, Width = 2048, Sectors = 16, Stars = 200});
        }

        private void SetupGalaxyCamera()
        {
            _galaxyCam = new Camera.Camera2D(_consoleManager, true);
            
            //Setup the limits, depends on the galaxy size
            _galaxyCam.MaxZoom = 1.0f;
            _galaxyCam.MinZoom = 0.07f;
            _galaxyCam.Limits = new Rectangle( 0 - (_gameGalaxy.Width / 2), 0 -( _gameGalaxy.Height / 2), _gameGalaxy.Width *2, _gameGalaxy.Height *2);
            
            //Temporary, should set position to current homeworld (not implemented yet)
            _galaxyCam.Pos = _gameGalaxy.Sectors[0].Stars[0].Position + new Vector2(Model.Constants.STAR_WIDTH / 2, Model.Constants.STAR_WIDTH / 2);
            
            _galaxyCam.CamWorldHeight = 400;
            _galaxyCam.CamWorldWidth = 640;
            _galaxyCam.Zoom = 1.0f;

        }
    }
}
