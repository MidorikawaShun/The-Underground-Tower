using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheUndergroundTower.OtherClasses;
using WpfApp1.GameProperties;

namespace WpfApp1.Creatures
{
    /// <summary>
    /// The object that represents the player character.
    /// </summary>
    public class Player : Creature
    {
        #region Properties
        /// <summary>
        /// The player's race.
        /// </summary>
        private Race _playerRace;
        public Race PlayerRace { get => _playerRace; set => _playerRace = value; }

        /// <summary>
        /// The player's career.
        /// </summary>
        private Career _playerCareer;
        public Career PlayerCareer { get => _playerCareer; set => _playerCareer = value; }

        /// <summary>
        /// The player's strength, dexterity, constitution, etc
        /// </summary>
        private int[] _playerStats;
        public int this[int number]
        {
            get { return _playerStats[number]; }
            set { _playerStats[number] = value; }
        }

        /// <summary>
        /// The items that the player has on him.
        /// </summary>
        private List<Item> _inventory;
        public List<Item> Inventory
        {
            get { return _inventory; }
            private set
            {
                if (value != null)
                    _inventory = value;
            }
        }

        /// <summary>
        /// The items that the player has equipped/is wearing/is holding.
        /// </summary>
        private Item[] _equipment;
        public Item[] Equipment
        {
            get { return _equipment; }
            private set
            {
                if (value != null && value.Length==Definitions.NUM_OF_EQUIPMENT_SLOTS)
                    _equipment = value;
            }
        }

        /// <summary>
        /// How much light the player is emitting.
        /// </summary>
        private int _lightRadius;
        public int LightRadius
        {
            get { return _lightRadius; }
            set
            {
                if (value >= 0)
                    _lightRadius = value;
            }
        }

        public const int _imageIndex = 146;
      
       
        #endregion
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Player() : base(_imageIndex)
        {
            _playerStats = new int[Definitions.NUMBER_OF_CHARACTER_STATS];
            _lightRadius = 5;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Sets the player's race.
        /// </summary>
        /// <param name="chosenRace">The race the player will have</param>
        public void SetRace(Race chosenRace)
        {
            Speed = chosenRace.Speed;
        }

        /// <summary>
        /// Sets the player's career.
        /// </summary>
        /// <param name="chosenCareer">The career the player will have</param>
        public void SetCareer(Career chosenCareer)
        {
            Inventory = chosenCareer.StartingInventory;
            MeleeSkill = chosenCareer.MeleeSkill;
            MagicSkill = chosenCareer.MagicSkill;
            RangedSkill = chosenCareer.RangedSkill;
            IsCaster = chosenCareer.IsCaster;
            Equipment = new Item[Definitions.NUM_OF_EQUIPMENT_SLOTS];
        }
        #endregion
    }
}
