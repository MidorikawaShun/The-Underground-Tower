using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheUndergroundTower.Creatures;
using TheUndergroundTower.OtherClasses;
using WpfApp1.GameProperties;
using Microsoft.VisualBasic;
using TheUndergroundTower.Windows.MetaMenus;
using TheUndergroundTower.Pathfinding;
using static WpfApp1.GameProperties.Definitions;
using static TheUndergroundTower.Options.Sound;

namespace WpfApp1.Creatures
{
    /// <summary>
    /// The object that represents the player character.
    /// </summary>
    public class Player : Creature
    {

        private const int NUM_OF_INVENTORY_SLOTS = 25;

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
                if (value != null && value.Length == Definitions.NUM_OF_EQUIPMENT_SLOTS)
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

        private int _level;
        public int Level
        {
            get { return _level; }
            set
            {
                if (value >= 0)
                    _level = value;
            }
        }

        private int _experience;
        public int Experience
        {
            get { return _experience; }
            set
            {
                if (value >= 0)
                    _experience = value;
            }
        }

        private int _neededExperience;
        public int NeededExperience
        {
            get { return _neededExperience; }
            set
            {
                if (value >= 0)
                    _neededExperience = value;
            }
        }

        public const int _imageIndex = 631;

        public int DefenseSkill;
        public int Protection
        {
            get
            {
                return _equipment.Where(x => x is Armor).Cast<Armor>().Sum(x => x.ArmorBonus);
            }
        }

        public string DamageRange
        {
            get
            {
                Weapon weapon = _equipment[(int)Definitions.EnumBodyParts.RightHand] as Weapon;
                if (weapon != null) return weapon.DamageRange;
                return "1-6";
            }
        }

        #endregion
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Player() : base(_imageIndex)
        {
            _playerStats = new int[Definitions.NUMBER_OF_CHARACTER_STATS];
            _lightRadius = 5;
            DefenseSkill = 10;
            _neededExperience = 100;
            _experience = 0;
            _level = 1;
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
            _inventory = new List<Item>();
            foreach (Item item in chosenCareer.StartingInventory)
                switch (item.GetType().Name.ToString())
                {
                    case "Weapon":
                        {
                            _inventory.Add(new Weapon(item as Weapon));
                            break;
                        }
                    case "Armor":
                        {
                            _inventory.Add(new Armor(item as Armor));
                            break;
                        }
                    default:
                        {
                            //_inventory.Add(new Item(item));
                            break;
                        }
                }
            MeleeSkill = chosenCareer.MeleeSkill;
            MagicSkill = chosenCareer.MagicSkill;
            RangedSkill = chosenCareer.RangedSkill;
            IsCaster = chosenCareer.IsCaster;
            Equipment = new Item[Definitions.NUM_OF_EQUIPMENT_SLOTS];
        }

        public void LevelUp()
        {
            Random rand = GameStatus.Random;
            _neededExperience += _neededExperience * 2;
            _playerStats[rand.Next(6)] += rand.Next(3); //increase two random stats
            _playerStats[rand.Next(6)] += rand.Next(3);
            HP = MaxHP += GameLogic.DiceRoll("1d6", _playerStats[(int)Definitions.EnumCharacterStats.Constitution]);
            _level++;
        }

        public override void Attack(Creature target)
        {
            int attackScore = (int)MeleeSkill + GameLogic.Roll20(1);
            int defenseScore = (target as Monster).Defense + GameLogic.Roll20(1);
            if (attackScore > defenseScore)
            {
                PlaySound(EnumSoundFiles.SwordSlash, EnumMediaPlayers.SfxPlayer);
                //int damage = GameLogic.DiceRoll(this.DamageRange, this[(int)Definitions.EnumCharacterStats.Strength]) + _playerStats[0];
                int damage = GameLogic.RollDamage(this.DamageRange) + _playerStats[(int)Definitions.EnumCharacterStats.Strength];
                target.TakeDamage(damage);
                GameLogic.PrintToGameLog("You have hit " + target.Name + " for " + damage);
                if (target.HP <= 0)
                {
                    _experience += (target as Monster).ExperienceValue;
                    GameLogic.PrintToGameLog(target.Name + " has died!");
                    GameStatus.Score += (target as Monster).ScoreValue;
                }
            }
            else
            {
                GameLogic.PrintToGameLog("You have missed " + target.Name);
                PlaySound(EnumSoundFiles.Miss, EnumMediaPlayers.SfxPlayer);
            }
            MeleeSkill += GameLogic.DiceRoll("1d3") / 100.0;
        }

        public override void TakeDamage(int damage)
        {
            damage -= Protection;
            base.TakeDamage(damage);
            GameLogic.PrintToGameLog("You have taken " + (damage > 0 ? damage : 0) + " damage!");
        }

        public void EquipArmor(Armor armor)
        {
            switch (armor.Type)
            {
                case "Ring":
                    {
                        string userChoice = GenericWindow.Create("title", new string[] { "Left Finger", "Right Finger", "Cancel" });
                        switch (userChoice)
                        {
                            case "LeftFinger":
                                {
                                    _equipment[(int)Definitions.EnumBodyParts.LeftRing] = armor;
                                    break;
                                }
                            case "RightFinger":
                                {
                                    _equipment[(int)Definitions.EnumBodyParts.RightRing] = armor;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        break;
                    }
                case "Hat":
                    {
                        _equipment[(int)Definitions.EnumBodyParts.Hat] = armor;
                        break;
                    }
                case "Amulet":
                    {
                        _equipment[(int)Definitions.EnumBodyParts.Amulet] = armor;
                        break;
                    }
                case "Shirt":
                    {
                        _equipment[(int)Definitions.EnumBodyParts.Shirt] = armor;
                        break;
                    }
                case "Gloves":
                    {
                        _equipment[(int)Definitions.EnumBodyParts.Gloves] = armor;
                        break;
                    }
                case "Pants":
                    {
                        _equipment[(int)Definitions.EnumBodyParts.Pants] = armor;
                        break;
                    }
                case "Boots":
                    {
                        _equipment[(int)Definitions.EnumBodyParts.Boots] = armor;
                        break;
                    }
                default:
                    {

                        break;
                    }
            }
            GameLogic.PrintToGameLog("You have equipped " + armor.Name);
        }

        public void PickUp(Tile tile)
        {
            try
            {
                if (Inventory.Count() >= NUM_OF_INVENTORY_SLOTS) return;
                List<Item> items = new List<Item>();
                foreach (var item in tile.Objects.OfType<Item>())
                    items.Add(item);
                string userChoice = string.Empty;
                if (items.Count > 0)
                {
                    if (items.Count > 1)
                        userChoice = GenericWindow.Create("title", items.Select(x => x.Name).ToArray());
                    else
                        userChoice = items[0].Name.Replace(" ", "");
                    if (userChoice != null)
                    {
                        Item chosenItem = tile.Objects.Where(x => x.Name.Replace(" ", "").Equals(userChoice)).FirstOrDefault() as Item;
                        Inventory.Add(chosenItem);
                        tile.Objects.Remove(chosenItem);
                        GameLogic.PrintToGameLog("You have picked up " + chosenItem.Name);
                    }
                }
            }
            catch (Exception ex) { }
        }

        #endregion
    }
}
