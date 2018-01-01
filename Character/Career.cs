using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WpfApp1;

namespace TheUndergroundTower.OtherClasses
{
    public class Career : GameObject
    {

        #region Properties

        /// <summary>
        /// The items that the character will start with after picking this career
        /// </summary>
        private List<Item> _startingInventory;
        public List<Item> StartingInventory
        {
            get { return _startingInventory; }
            set
            {
                if (value != null)
                    _startingInventory = value;
            }
        }

        /// <summary>
        /// Can this career cast spells
        /// </summary>
        private bool _isCaster;
        public bool IsCaster
        {
            get { return _isCaster; }
            set { _isCaster = value; }
        }

        /// <summary>
        /// Determines the chance to hit targets in close quarters combat.
        /// Increases as you fight in close quarters combat.
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
        /// Determines the chance to hit targets in ranged combat.
        /// Increases as you fight in ranged combat.
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
        /// Determines the chance of successfully casting a spell.
        /// Increases as you cast spells.
        /// </summary>
        private double _magicSkill;
        public double MagicSkill
        {
            get { return _magicSkill; }
            set
            {
                if (value >= 0)
                    _magicSkill = value;
            }
        }
        #endregion

        /// <summary>
        /// Constructor from XML of a career
        /// </summary>
        /// <param name="newCareer">The career we are trying to make</param>
        public Career(XmlNode newCareer)
        {
            //Obtains the career name
            Name = newCareer.Attributes["Name"].Value;
            _startingInventory = new List<Item>();
            foreach (XmlNode privacyType in newCareer)
            {
                //Public features are what the user can see during character creation
                if (privacyType.Name.Equals("Public"))
                {
                    Description = privacyType.ChildNodes[0].FirstChild.Value;
                    IsCaster = Convert.ToBoolean(privacyType.ChildNodes[1].FirstChild.Value);
                    MeleeSkill = Convert.ToDouble(privacyType.ChildNodes[2].FirstChild.Value);
                    RangedSkill = Convert.ToDouble(privacyType.ChildNodes[3].FirstChild.Value);
                    MagicSkill = Convert.ToDouble(privacyType.ChildNodes[4].FirstChild.Value);
                }
                else
                {
                    //Private features which contain the starting inventory
                    foreach (XmlNode item in privacyType.ChildNodes[0])
                    {
                        switch (item.Name)
                        {
                            case "Weapon":
                                _startingInventory.Add(new Weapon(item));
                                break;
                            case "Armor":
                                _startingInventory.Add(new Armor(item));
                                break;
                        }
                    }
                }
            }
            //Adds this class to the list of classes we read from XML
            GameData.POSSIBLE_CAREERS.Add(this);
        }
    }

}

