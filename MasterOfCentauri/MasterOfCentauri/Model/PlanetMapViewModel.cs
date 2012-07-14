namespace MasterOfCentauri.Model
{
    public class PlanetMapViewModel
    {
        public PlanetMapRingViewModel[] Rings { get; set; } 
    }

    public class PlanetMapRingViewModel
    {
        public PlanetMapPlanetViewModel[] Planets { get; set; }
    }

    public class PlanetMapPlanetViewModel
    {
        public string Name { get; set; }
        public float Scale { get; set; }
    }
}