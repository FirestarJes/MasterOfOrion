using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MasterOfCentauri.Util;

namespace MasterOfCentauri.Managers
{
    /// <summary>
    /// This class contains all the different list of game objects (planettypes, startypes etc.)
    /// </summary>
    class GameContentManager
    {
        private List<Model.StarType> _starTypes;
        private List<Model.PlanetType> _planetTypes;
        private List<string> _starNames;
        private List<string> _notUsedNames;
        private readonly ContentController _content;
        MersenneRandom rand = new MersenneRandom();


        public GameContentManager(IServiceProvider services)
        {
            _starTypes = new List<Model.StarType>();
            _starNames = new List<string>();
            _notUsedNames = new List<string>();
            _content = (ContentController)services.GetService(typeof(ContentController));
        }

        public Model.StarType GetRandomStarType()
        {
            return _starTypes[rand.Next(_starTypes.Count)];
        }


        public void ResetUsedNames()
        {
            _notUsedNames.AddRange(_starNames);
        }

        public string GetRandomStarName()
        {
            string name = _notUsedNames[rand.Next(_notUsedNames.Count)];
            _notUsedNames.Remove(name);
            return name;
        }

        public void LoadGameContent()
        {
            XDocument startypes = XDocument.Load(System.IO.Path.Combine(_content.ContentDir , @"base\xml\startypes.xml"));
            _starTypes = (from c in startypes.Element("StarTypes").Descendants("StarType")
                           select new Model.StarType()
                           {
                               TextureName = c.Element("Graphics").Value,
                               ID = c.Element("ID").Value,
                               Name = c.Element("Name").Value,
                               FlavorText = c.Element("Flavor").Value,
                               PlanetProbabilities = (from d in c.Element("PlanetProbabilities").Descendants("PlanetProbability") select new KeyValuePair<string, float>(d.Element("PlanetTypeID").Value, float.Parse(d.Element("Probability").Value))).ToDictionary( o => o.Key, o => o.Value) 
                           }).ToList<Model.StarType>();
            
            XDocument planetTypes = XDocument.Load(System.IO.Path.Combine(_content.ContentDir, @"base\xml\planettypes.xml"));
            _planetTypes = (from c in planetTypes.Element("PlanetTypes").Descendants("PlanetType")
                          select new Model.PlanetType()
                          {
                              Textures = (from d in c.Element("Textures").Descendants("Texture") select d.Value).ToList<string>(),
                              ID = c.Element("ID").Value,
                              Name = c.Element("Name").Value,
                              FlavorText = c.Element("Flavor").Value,
                              IsUnique =bool.Parse(c.Element("IsUnique").Value),
                              BasePopulationMin = int.Parse(c.Element("BasePopulation").Element("Min").Value),
                              BasePopulationMax = int.Parse(c.Element("BasePopulation").Element("Max").Value),
                              BaseProductionPerPopulationMin = float.Parse(c.Element("BaseProductionPerPop").Element("Min").Value),
                              BaseProductionPerPopulationMax = float.Parse(c.Element("BaseProductionPerPop").Element("Max").Value),
                              BaseGrowthMin = float.Parse(c.Element("BasePopGrowth").Element("Min").Value),
                              BaseGrowthMax = float.Parse(c.Element("BasePopGrowth").Element("Max").Value),
                              AttributeProbabilities = (from d in c.Element("AttributeProbabilites").Descendants("AttributeProbability") select new KeyValuePair<string, float>(d.Element("AttributeID").Value, float.Parse(d.Element("Probability").Value))).ToDictionary(o => o.Key, o => o.Value)
                          }).ToList<Model.PlanetType>();

            System.IO.StreamReader nameReader = new System.IO.StreamReader(System.IO.Path.Combine(_content.ContentDir, @"base\Text\#SystemNames.txt"));
             
            while (!nameReader.EndOfStream)
            {
                string name = nameReader.ReadLine();
                _starNames.Add(name);
                _notUsedNames.Add(name);
            }
 
        }
    }


}
