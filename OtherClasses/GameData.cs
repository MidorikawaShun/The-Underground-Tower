using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheUndergroundTower.OtherClasses;

namespace WpfApp1
{
    public static class GameData
    {
        public static List<Race> RACES { get; set; }
        public static List<Class> CLASSES { get; set; }
        public static List<Difficulty> DIFFICULTIES { get; set; }
        public static List<TowerDepth> TOWER_DEPTHS { get; set; }

        public static void InitializeRaces()
        {
            RACES = new List<Race>();
            Utilities.Xml.PopulateRaces();
        }

        public static void InitializeClasses()
        {
            CLASSES = new List<Class>();
            Utilities.Xml.PopulateClasses();
        }

        public static void InitializeDifficulties()
        {
            DIFFICULTIES = new List<Difficulty>();
            Utilities.Xml.PopulateDifficulties();
        }

        public static void InitializeTowerDepths()
        {
            TOWER_DEPTHS = new List<TowerDepth>();
            Utilities.Xml.PopulateTowerDifficulties();
        }

    }
}
