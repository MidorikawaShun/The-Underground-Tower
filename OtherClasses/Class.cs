using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WpfApp1;

namespace TheUndergroundTower.OtherClasses
{
    public class Class : GameObject
    {

        #region Properties

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

        private bool _isCaster;
        public bool IsCaster
        {
            get { return _isCaster; }
            set { _isCaster = value; }
        }

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
        #endregion

        public Class(XmlNode newClass)
        {
            Name = newClass.Attributes["Name"].Value;
            foreach (XmlNode privacyType in newClass)
            {
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
                    foreach (XmlNode item in privacyType.ChildNodes[0])
                    {
                        switch (item.Name)
                        {
                            case "Weapon":
                                new Weapon(item);
                                break;
                            case "Armor":
                                new Armor(item);
                                break;
                        }
                    }
                }
            }
            GameData.CLASSES.Add(this);
        }
    }

}

