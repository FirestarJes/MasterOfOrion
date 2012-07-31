using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DigitalRune.Game.UI.Controls;
using MasterOfCentauri.Managers;

namespace MasterOfCentauri.GameScreens
{
    class GameScreenBase: ContentControl 
    {
        protected ContentController _content;
        protected GameManager _gameManager;

        public GameScreenBase(IServiceProvider services)
        {
            _content = (ContentController)services.GetService(typeof(ContentController));
            _gameManager = (GameManager)services.GetService(typeof(GameManager));
        }
    }
}
