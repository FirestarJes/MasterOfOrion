using System.Linq;

namespace MasterOfCentauri.Model.SystemMap
{
    public class SystemMapRingViewModel
    {
        public SystemMapPlanetViewModel[] Planets { get; set; }

        public override string ToString()
        {
            return "Planets:\n   " + string.Join(",\n", Planets.Select(x => x.ToString())).Replace("\n", "\n   ");
        }
    }
}