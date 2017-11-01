using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheUndergroundTower.OtherClasses;
using TheUndergroundTower.Pathfinding;

namespace WpfApp1
{
    public static class GameData
    {
        public static List<Race> RACES { get; set; }
        public static List<Career> CAREERS { get; set; }
        public static List<Difficulty> DIFFICULTIES { get; set; }
        public static List<TowerDepth> TOWER_DEPTHS { get; set; }
        public static List<Tile> TILES { get; set; }

        public static void InitializeRaces()
        {
            RACES = new List<Race>();
            Options.Xml.PopulateRaces();
        }

        public static void InitializeCareer()
        {
            CAREERS = new List<Career>();
            Options.Xml.PopulateCareers();
        }

        public static void InitializeDifficulties()
        {
            DIFFICULTIES = new List<Difficulty>();
            Options.Xml.PopulateDifficulties();
        }

        public static void InitializeTowerDepths()
        {
            TOWER_DEPTHS = new List<TowerDepth>();
            Options.Xml.PopulateTowerDifficulties();
        }

        public static void InitializeTiles()
        {
            TILES = new List<Tile>();
            Options.Xml.PopulateTiles();
        }

    }
}
