using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterOfCentauri.Model
{
    class StarType
    {
        private string _name;
        private string _textureName;
        private string _id;
        private string _flavor;
        private Dictionary<string, float> _planetProbabilities;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string TextureName
        {
            get { return _textureName; }
            set { _textureName = value; }
        }

        public string FlavorText
        {
            get { return _flavor; }
            set { _flavor = value; }
        }

        public Dictionary<string, float> PlanetProbabilities
        {
            get { return _planetProbabilities; }
            set { _planetProbabilities = value; }
        }
    }
}
