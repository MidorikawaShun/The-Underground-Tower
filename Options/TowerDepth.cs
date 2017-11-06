using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WpfApp1;

namespace TheUndergroundTower.OtherClasses
{
    /// <summary>
    /// The object describing the possible tower depths
    /// </summary>
    public class TowerDepth : GameObject
    {
        private int _minimumFloors;
        public int MinimumFloors { get => _minimumFloors; set => _minimumFloors = value; }

        private int _maximumFloors;
        public int MaximumFloors { get => _maximumFloors; set => _maximumFloors = value; }

        private double _floorSizeMultiplier;
        public double FloorSizeMultiplier { get => _floorSizeMultiplier; set => _floorSizeMultiplier = value; }

        public TowerDepth(XmlNode towerDepth)
        {
            Name = towerDepth.Attributes["Name"].Value;
            Description = towerDepth.ChildNodes[0].FirstChild.Value;
            MinimumFloors = Convert.ToInt32(towerDepth.ChildNodes[1].FirstChild.Value);
            MaximumFloors = Convert.ToInt32(towerDepth.ChildNodes[2].FirstChild.Value);
            FloorSizeMultiplier = Convert.ToDouble(towerDepth.ChildNodes[3].FirstChild.Value);
            GameData.POSSIBLE_TOWER_DEPTHS.Add(this);
        }

    }
}
