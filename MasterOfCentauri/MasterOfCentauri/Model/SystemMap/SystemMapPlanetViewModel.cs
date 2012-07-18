namespace MasterOfCentauri.Model.SystemMap
{
    public class SystemMapPlanetViewModel
    {
        public string Name { get; set; }
        public float Scale { get; set; }

        public override string ToString()
        {
            return Name + " at scale " + Scale;
        }
    }
}