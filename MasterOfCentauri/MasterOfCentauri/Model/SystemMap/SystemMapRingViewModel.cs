using System.Linq;

namespace MasterOfCentauri.Model.SystemMap
{
    public class PlanetMapRingViewModel
    {
        public PlanetMapPlanetViewModel[] Planets { get; set; }

        public override string ToString()
        {
            return "Planets:\n   " + string.Join(",\n", Planets.Select(x => x.ToString())).Replace("\n", "\n   ");
        }
    }
}