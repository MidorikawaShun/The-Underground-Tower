using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TheUndergroundTower.OtherClasses;
using TheUndergroundTower.Pathfinding;

namespace WpfApp1.Creatures
{
    public abstract class Creature : GameObject
    {
        #region Properties

        /// <summary>
        /// Determines how much damage the creature can take before being destroyed.
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
        /// Determines how quickly a creature can perform actions that do not involve movement.
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

        /// <summary>
        /// Determines how proficient the user is with melee attacks. The higher this stat, the likelier
        /// the user is to hit his opponent.
        /// </summary>
        private double _meleeSkill;
        public double MeleeSkill
        {
            get { return _meleeSkill; }
            set
            {
                if (value > 0)
                    _meleeSkill = value;
            }
        }

        /// <summary>
        /// Determines how proficient the user is with ranged, non-magical attacks. The higher this stat, the 
        /// likelier the user is to hit his opponent.
        /// </summary>
        private double _rangedSkill;
        public double RangedSkill
        {
            get { return _rangedSkill; }
            set
            {
                if (value > 0)
                    _rangedSkill = value;
            }
        }

        /// <summary>
        /// Determines how proficient the user is with magic. The higher the stat, the likelier the user is
        /// in succeeding to cast the spell.
        /// </summary>
        private double _magicSkill;
        public double MagicSkill
        {
            get { return _magicSkill; }
            set
            {
                if (value > 0)
                    _magicSkill = value;
            }
        }

        /// <summary>
        /// Determines if a creature can cast spells.
        /// </summary>
        private bool _isCaster;
        public bool IsCaster { get; set; }

        /// <summary>
        /// The visual representation of the creature.
        /// </summary>
        private ImageSource _image;
        public ImageSource Image { get => _image; set => _image = value; }
        
        private int _index;
        public int Index { get => _index; set => _index = value; }

        #endregion

        /// <summary>
        /// Constructor for the Creature class.
        /// </summary>
        public Creature(int index)
        {
            _hp = 1;
            _speed = 100;
            Image = CreateTile.GetImageFromTileset(index);
            GameStatus.CREATURES.Add(this);
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Creature() { }

        /// <summary>
        /// Check if a creature is able to move into the supplied coordinate, and move if possible.
        /// </summary>
        /// <param name="coord">The coordinate you want the creature to move to.</param>
        public void MoveTo(MapCoord coord,Map map)
        {
            if (map.Tiles[coord.X, coord.Y].Walkable == false)
                return;
            else
                Location = new FullCoord(coord.X, coord.Y, Location.Z);
        }

        public override ImageSource GetImage()
        {
            return Image;
        }

    }
}
