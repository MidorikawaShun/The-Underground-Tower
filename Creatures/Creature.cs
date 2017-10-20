using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Creatures
{
    public abstract class Creature
    {
        /// <summary>
        /// The number representing how much damage the creature can take before being destroyed.
        /// </summary>
        private int _hp;
        public int HP
        {
            get { return _hp; }
            set
            {
                if (value >= 0)
                    _hp = value;
            }
        }
        /// <summary>
        /// The number representing how quickly a creature can move.
        /// </summary>
        private int _movement;
        public int Movement
        {
            get { return _movement; }
            set
            {
                if (value >= 0)
                    _movement = value;
            }
        }
        /// <summary>
        /// The number representing how quickly a creature can perform actions that do not involve movement.
        /// </summary>
        private int _speed;
        public int Speed
        {
            get { return _speed; }
            set
            {
                if (value >= 0)
                    _speed = value;
            }
        }

        public string Name { get; set; }

        /// <summary>
        /// Constructor for the Creature class.
        /// </summary>
        public Creature()
        {
            _hp = 1;
            _movement = 100;
            _speed = 100;
            GameStatus.CREATURES.Add(this);
        }
    }
}
