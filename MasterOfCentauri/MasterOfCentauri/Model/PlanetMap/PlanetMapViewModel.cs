﻿namespace MasterOfCentauri.Model.PlanetMap
{
    public class PlanetMapViewModel
    {
        public string Name { get; set; }
        public BuildingCellViewModel[,] BuildingCells { get; set; }
    }
}