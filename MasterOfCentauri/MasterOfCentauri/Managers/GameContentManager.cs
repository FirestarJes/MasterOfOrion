using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterOfCentauri.Managers
{
    /// <summary>
    /// This class contains all the different list of game objects (planettypes, startypes etc.)
    /// </summary>
    class GameContentManager
    {
        private List<Model.StarType> _starTypes;

        public GameContentManager()
        {
            _starTypes = new List<Model.StarType>();
        }
    }


}
