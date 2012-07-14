using System.Linq;

namespace MasterOfCentauri.Model
{
    public class PlanetMapViewModel
    {
        public PlanetMapRingViewModel[] Rings { get; set; }

        public override string ToString()
        {
            return "PlanetMapViewModel\n   " + string.Join(",\n", Rings.Select(x => x.ToString())).Replace("\n", "\n   ");
        }
    }

    public class PlanetMapRingViewModel
    {
        public PlanetMapPlanetViewModel[] Planets { get; set; }

        public override string ToString()
        {
            return "Planets:\n   " + string.Join(",\n", Planets.Select(x => x.ToString())).Replace("\n", "\n   ");
        }
    }

    public class PlanetMapPlanetViewModel
    {
        public string Name { get; set; }
        public float Scale { get; set; }

        public override string ToString()
        {
            return Name + " at scale " + Scale;
        }
    }


}