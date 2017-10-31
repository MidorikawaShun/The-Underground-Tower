using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheUndergroundTower.OtherClasses;

namespace WpfApp1.Creatures
{
    public class Player : Creature
    {
        #region Properties
        private Race _playerRace;
        public Race PlayerRace { get => _playerRace; set => _playerRace = value; }

        private Class _playerClass;
        public Class PlayerClass { get => _playerClass; set => _playerClass = value; }

        private int[] _playerStats;
        public int this[int number]
        {
            get { return _playerStats[number]; }
            set { _playerStats[number] = value; }
        }

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

        #endregion
        #region Constructors
        public Player()
        {
            _playerStats = new int[Definitions.NUMBER_OF_CHARACTER_STATS];
            _lightRadius = 5;
        }
        #endregion
        #region Methods
        public void SetRace(Race chosenRace)
        {
            Movement = chosenRace.Movement;
            Speed = chosenRace.Speed;
            SightRadius = chosenRace.SightRadius;
            DarkVision = chosenRace.DarkVision;
        }

        public void SetClass(Class chosenClass)
        {
            Inventory = chosenClass.StartingInventory;
            MeleeSkill = chosenClass.MeleeSkill;
            MagicSkill = chosenClass.MagicSkill;
            RangedSkill = chosenClass.RangedSkill;
            IsCaster = chosenClass.IsCaster;
            Equipment = new Item[Definitions.NUM_OF_EQUIPMENT_SLOTS];
        }
        #endregion
    }
}
