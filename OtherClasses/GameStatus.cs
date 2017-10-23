using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Creatures;

namespace WpfApp1
{
    public static class GameStatus
    {
        /// <summary>
        /// All the creatures that currently exist in the game world
        /// </summary>
        public static List<Creature> CREATURES { get; set; }
        public static Player PLAYER { get; set; }
        public static Random rand = new Random(DateTime.Now.Millisecond);

    }
}
