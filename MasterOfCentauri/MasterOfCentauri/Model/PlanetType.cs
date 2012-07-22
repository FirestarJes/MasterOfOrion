using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterOfCentauri.Model
{
    class PlanetType
    {
        private string _id;
        private string _name;
        private string _flavor;
        private bool _isUnique;
        private int _basePopMin;
        private int _basePopMax;
        private float _baseProdPerPopMin;
        private float _baseProdPerPopMax;
        private float _baseGrowthMin;
        private float _baseGrowthMax;
        private Dictionary<string, float> _attributeProbabilities;
        private List<string> _textures;

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

        public string FlavorText
        {
            get { return _flavor; }
            set { _flavor = value; }
        }

        public bool IsUnique
        {
            get { return _isUnique; }
            set { _isUnique = value; }
        }

        public int BasePopulationMin
        {
            get { return _basePopMin; }
            set { _basePopMin = value; }
        }
        public int BasePopulationMax
        {
            get { return _basePopMax; }
            set { _basePopMax = value; }
        }

        public float BaseProductionPerPopulationMin
        {
            get { return _baseProdPerPopMin; }
            set { _baseProdPerPopMin = value; }
        }

        public float BaseProductionPerPopulationMax
        {
            get { return _baseProdPerPopMax; }
            set { _baseProdPerPopMax = value; }
        }

        public float BaseGrowthMin
        {
            get { return _baseGrowthMin; }
            set { _baseGrowthMin = value; }
        }

        public float BaseGrowthMax
        {
            get { return _baseGrowthMax; }
            set { _baseGrowthMax = value; }
        }

        public List<string> Textures
        {
            get { return _textures; }
            set { _textures = value; }
        }

        public Dictionary<string, float> AttributeProbabilities
        {
            get { return _attributeProbabilities; }
            set { _attributeProbabilities = value; }
        }
    }
}
