using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public static List<Creature> CREATURES { get; set; }

        /// <summary>
        /// The player object.
        /// </summary>
        public static Player PLAYER { get; set; }

        /// <summary>
        /// All the maps that have been created.
        /// </summary>
        public static List<Map> MAPS { get; set; }

        /// <summary>
        /// The map the player is currently on.
        /// </summary>
        public static Map CURRENT_MAP { get; set; }

        /// <summary>
        /// All the Tiles that exist in the game.
        /// </summary>
        public static List<Tile> TILES { get; set; }

        /// <summary>
        /// A Random object. Exists so we only have one and it is initialized
        /// in the beginning.
        /// </summary>
        public static Random RANDOM = new Random(DateTime.Now.Millisecond);

    }
}
