using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MasterOfCentauri.Managers
{
    /// <summary>
    /// This class contains all the different list of game objects (planettypes, startypes etc.)
    /// </summary>
    class GameContentManager
    {
        private List<Model.StarType> _starTypes;
        private readonly ContentController _content;

        public GameContentManager(IServiceProvider services)
        {
            _starTypes = new List<Model.StarType>();
            _content = (ContentController)services.GetService(typeof(ContentController));
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
 
        }
    }


}
