namespace MasterOfCentauri.UIControls
{
    public class SystemMapPlanetClicked
    {
        public Planet Planet { get; private set; }

        public SystemMapPlanetClicked(Planet planet)
        {
            Planet = planet;
        }
    }
}