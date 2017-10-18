using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Creatures
{
    public class Player : Creature
    {

        private Race _playerRace;
        public Race PlayerRace
        {
            get { return _playerRace; }
            set { if (value != null) _playerRace = value; }
        }

        /// <summary>
        /// Creates the player instance.
        /// </summary>
        public Player() : base()
        {

        }
    }
}
