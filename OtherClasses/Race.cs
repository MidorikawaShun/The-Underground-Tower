using System;
using System.Xml;
using TheUndergroundTower.OtherClasses;

namespace WpfApp1
{
    /// <summary>
    /// Describes the Races a player character can belong to. They change player stats 
    /// for better or worse.
    /// </summary>
    public class Race : GameObject
    {

        #region Properties
        
        //Shortcut to access each stat individually.
        private int[] _stats = new int[Definitions.NUMBER_OF_CHARACTER_STATS];
        public int this[int number]
        {
            get { return _stats[number]; }
            set { _stats[number] = value; }
        }

        public int Strength
        {
            get { return _stats[0]; }
            set { _stats[0] = value; }
        }

        public int Dexterity
        {
            get { return _stats[1]; }
            set { _stats[1] = value; }
        }

        public int Constitution
        {
            get { return _stats[2]; }
            set { _stats[2] = value; }
        }

        public int Intelligence
        {
            get { return _stats[3]; }
            set { _stats[3] = value; }
        }

        public int Wisdom
        {
            get { return _stats[4]; }
            set { _stats[4] = value; }
        }

        public int Charisma
        {
            get { return _stats[5]; }
            set { _stats[5] = value; }
        }

        private int _movement;
        public int Movement
        {
            get { return _movement; }
            set { _movement = value; }
        }

        private int _speed;
        public int Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        private int _survival;
        public int Survival
        {
            get { return _survival; }
            set { _survival = value; }
        }

        /// <summary>
        /// Determines how far a creature can see.
        /// </summary>
        private int _sightRadius;
        public int SightRadius
        {
            get { return _speed; }
            set
            {
                if (value >= 0)
                    _sightRadius = value;
            }
        }

        /// <summary>
        /// Determines if, and how far a creature can see without a light source.
        /// </summary>
        private int _darkVision;
        public int DarkVision
        {
            get { return _darkVision; }
            set
            {
                if (value >= 0)
                    _darkVision = value;
            }
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
                else
                {
                    Movement = Convert.ToInt32(privacyType.ChildNodes[0].Value);
                    Speed = Convert.ToInt32(privacyType.ChildNodes[1].Value);
                    SightRadius = Convert.ToInt32(privacyType.ChildNodes[2].Value);
                    DarkVision = Convert.ToInt32(privacyType.ChildNodes[3].Value);
                }
            }
            GameData.RACES.Add(this);
        }

        public Race() { }
        #endregion
    }
}