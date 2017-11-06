using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheUndergroundTower.OtherClasses;
using TheUndergroundTower.Pathfinding;

namespace WpfApp1
{
    /// <summary>
    /// Possible game elements that can be used.
    /// </summary>
    public static class GameData
    {

        // Contains a list of the races, careers, difficulties,
        // tower depths and tiles that the game has (reads from XML).

        public static List<Race> POSSIBLE_RACES { get; set; }
        public static List<Career> POSSIBLE_CAREERS { get; set; }
        public static List<Difficulty> POSSIBLE_DIFFICULTIES { get; set; }
        public static List<TowerDepth> POSSIBLE_TOWER_DEPTHS { get; set; }
        public static List<Tile> POSSIBLE_TILES { get; set; }

        //Creates the lists above from XML files.

        public static void InitializeRaces()
        {
            POSSIBLE_RACES = new List<Race>();
            Utilities.Xml.PopulateRaces();
        }

        public static void InitializeCareer()
        {
            POSSIBLE_CAREERS = new List<Career>();
            Utilities.Xml.PopulateCareers();
        }

        public static void InitializeDifficulties()
        {
            POSSIBLE_DIFFICULTIES = new List<Difficulty>();
            Utilities.Xml.PopulateDifficulties();
        }

        public static void InitializeTowerDepths()
        {
            POSSIBLE_TOWER_DEPTHS = new List<TowerDepth>();
            Utilities.Xml.PopulateTowerDepths();
        }

        public static void InitializeTiles()
        {
            POSSIBLE_TILES = new List<Tile>();
            Utilities.Xml.PopulateTiles();
        }

    }
}
