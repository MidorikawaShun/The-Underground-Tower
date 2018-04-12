using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheUndergroundTower.Creatures;
using TheUndergroundTower.OtherClasses;
using TheUndergroundTower.Pathfinding;
using WpfApp1.Creatures;

namespace WpfApp1
{
    /// <summary>
    /// Things that exist in the game.
    /// </summary>
    public static class GameStatus
    {
        /// <summary>
        /// All the creatures that currently exist in the game world.
        /// </summary>
        public static List<Creature> Creatures { get; set; }

        /// <summary>
        /// The player object.
        /// </summary>
        public static Player Player { get; set; }

        /// <summary>
        /// All the maps that have been created.
        /// </summary>
        public static List<Map> Maps { get; set; }

        /// <summary>
        /// The map the player is currently on.
        /// </summary>
        public static Map CurrentMap { get; set; }

        public static Dictionary<Map, Tile> StairsDownLocations { get; set; }

        public static Dictionary<Map, Tile> StairsUpLocations { get; set; }

        /// <summary>
        /// All the Tiles that exist in the game.
        /// </summary>
        public static List<Tile> Tiles { get; set; }

        /// <summary>
        /// A Random object. Exists so we only have one and it is initialized
        /// in the beginning.
        /// </summary>
        public static Random Random { get; set; } = new Random(DateTime.Now.Millisecond);

        public static List<Monster> Monsters { get; set; }

        public static List<Item> Items { get; set; }

        public static bool GamePaused { get; set; } = false;

        public static bool GameEnded { get; set; } = false;

        public static Difficulty ChosenDifficulty { get; set; }

        public static int Score { get; set; } = 0;

        public static string FinalScore { get => (Score * ChosenDifficulty.ScoreMultiplier).ToString("#,#0"); }

        public static TowerDepth ChosenDepth { get; set; }

        public static HighScore FinalizedHighScore { get; set; }

    }
}
