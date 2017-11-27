using System;
using System.Xml;
using TheUndergroundTower.OtherClasses;
using WpfApp1.GameProperties;

namespace WpfApp1
{
    /// <summary>
    /// Describes the Races a player character can belong to. They change player stats 
    /// for better or worse.
    /// </summary>
    public class Race : GameObject
    {

        #region Properties

        /// <summary>
        /// Shortcut to access each stat individually.
        /// </summary>
        private int[] _stats = new int[Definitions.NUMBER_OF_CHARACTER_STATS];
        public int this[int number]
        {
            get { return _stats[number]; }
            set { _stats[number] = value; }
        }

        /// <summary>
        /// Determines the amount of damage your character deals in melee combat.
        /// The higher your strength, the more damage you deal.
        /// </summary>
        public int Strength
        {
            get { return _stats[0]; }
            set { _stats[0] = value; }
        }

        /// <summary>
        /// Determines the amount of damage you deal in ranged combat.
        /// The higher your dexterity, the more damage you deal. 
        /// </summary>
        public int Dexterity
        {
            get { return _stats[1]; }
            set { _stats[1] = value; }
        }

        /// <summary>
        /// Increases your maximum HP (hit points).
        /// The higher your constitution, the more HP you have.
        /// </summary>
        public int Constitution
        {
            get { return _stats[2]; }
            set { _stats[2] = value; }
        }

        /// <summary>
        /// The higher this stat, the higher your damage with spells.
        /// </summary>
        public int Intelligence
        {
            get { return _stats[3]; }
            set { _stats[3] = value; }
        }

        /// <summary>
        /// Increases your chance of finding traps and hidden doors.
        /// </summary>
        public int Perception
        {
            get { return _stats[4]; }
            set { _stats[4] = value; }
        }

        /// <summary>
        /// Affects the prices of items in shops.
        /// The higher your charisma, the lower the price and the higher the price of things you sell.
        /// </summary>
        public int Charisma
        {
            get { return _stats[5]; }
            set { _stats[5] = value; }
        }

        /// <summary>
        /// Determines when a character gets a turn.
        /// </summary>
        private int _speed;
        public int Speed
        {
            get { return _speed; }
            set
            {
                if (value>=0)
                    _speed = value;
            }
        }

        /// <summary>
        /// Determines how quickly your hunger meter drops.
        /// </summary>
        private int _survival;
        public int Survival
        {
            get { return _survival; }
            set { _survival = value; }
        }

        #endregion
        #region Constructors
        /// <summary>
        /// Create a Race from an XmlNode and add it to GameData.RACES
        /// </summary>
        /// <param name="race">The XmlNode for the race to be created.</param>
        public Race(XmlNode race)
        {
            Name = race.Attributes["Name"].Value;
            foreach (XmlNode privacyType in race)
            {
                //The public properties that the player can see when creating a character.
                if (privacyType.Name.Equals("Public"))
                {
                    Description = privacyType.ChildNodes[0].FirstChild.Value;
                    XmlNode stats = privacyType.ChildNodes[1];
                    for (int i = 0; i < Definitions.NUMBER_OF_CHARACTER_STATS; i++)
                    {
                        string stringVal = stats.ChildNodes[i].FirstChild.Value;
                        this[i] = Convert.ToInt32(stringVal);
                    }
                }
                //The private properties.
                else
                {
                    Speed = Convert.ToInt32(privacyType.ChildNodes[0].Value);
                }
            }
            GameData.POSSIBLE_RACES.Add(this);
        }

        /// <summary>
        /// Regular constructor.
        /// </summary>
        public Race() { }
        #endregion
    }
}