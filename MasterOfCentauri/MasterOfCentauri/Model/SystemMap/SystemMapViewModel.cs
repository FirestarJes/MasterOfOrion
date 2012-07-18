using System.Linq;

namespace MasterOfCentauri.Model.SystemMap
{
    public class SystemMapViewModel
    {
        public SystemMapRingViewModel[] Rings { get; set; }

        public override string ToString()
        {
            return "SystemMapViewModel\n   " + string.Join(",\n", Rings.Select(x => x.ToString())).Replace("\n", "\n   ");
        }
    }
}