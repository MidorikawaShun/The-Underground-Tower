using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TheUndergroundTower.Creatures;
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

        ///// <summary>
        ///// Check if a creature is able to move into the supplied coordinate, and move if possible.
        ///// </summary>
        ///// <param name="coord">The coordinate you want the creature to move to.</param>
        //public void MoveTo(int paramX,int paramY,Map map)
        //{
        //    Tile tile = map.Tiles[paramX, paramY];
        //    //if (!tile.IsWalkable())
        //    //    return;
        //    map.Tiles[X, Y].Objects.Remove(this);
        //    X = paramX;Y = paramY;
        //    tile.Objects = tile.Objects ?? new List<GameObject>();
        //    tile.Objects.Add(this);
        //}

        public override ImageSource GetImage()
        {
            return Image;
        }

        public bool MoveTo(int targetX,int targetY, Map map)
        {
            if (!map.InBoundsOfMap(targetX,targetY) || !map.Tiles[targetX, targetY].IsWalkable())
                return false;
            Tile oldTile = map.Tiles[X, Y];
            oldTile.Objects.Remove(this);
            if (oldTile.Objects.Count() == 0) oldTile.Objects = null;
            Tile tile = map.Tiles[targetX, targetY];
            tile.Objects = tile.Objects ?? new List<GameObject>();
            tile.Objects.Add(this);
            X = targetX; Y = targetY;
            return true;
        }

    }
}
