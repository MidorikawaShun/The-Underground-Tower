using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using WpfApp1;
using static WpfApp1.Utilities;

namespace TheUndergroundTower.OtherClasses
{
    /// <summary>
    /// Represents a weapon object that the player can wield.
    /// </summary>
    public class Weapon : Item
    {

        #region Properties

        /// <summary>
        /// The weapon's bonus to helping the player hit enemies.
        /// </summary>
        private double _hitBonus;
        public double HitBonus
        {
            get { return _hitBonus; }
            set
            {
                if (value >= 0)
                    _hitBonus = value;
            }
        }

        /// <summary>
        /// The minimum and maximum damage the weapon can deal, before adjustments
        /// according to player stats.
        /// </summary>
        private string _damageRange;
        public string DamageRange
        {
            get { return _damageRange; }
            set
            {
                try
                {
                    string[] splitValue = value.Split('-');
                    if (splitValue.Length != 2)
                        throw new Exception($"DamageRange of weapon {Name} is incorrectly formatted! Must contain only two numbers, divided by a single '-' and no spaces!");
                    int minDamage = Convert.ToInt32(splitValue[0]);
                    int maxDamage = Convert.ToInt32(splitValue[1]);
                    if (minDamage > maxDamage)
                        throw new Exception("DamageRange of weapon {Name} has a minimum value higher than it's max value!");
                    _damageRange = value;
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex, $"DamageRange of weapon {Name} not in correct format! Correct format is \"[smaller number]-[higher number]\"");
                }
            }
        }

        /// <summary>
        /// Does the weapon require the use of two hands?
        /// </summary>
        private bool _twoHanded;
        public bool TwoHanded
        {
            get { return _twoHanded; }
            set { _twoHanded = value; }
        }

        #endregion
        #region Constructors

        /// <summary>
        /// Empty Weapon constructor.
        /// </summary>
        public Weapon() { }

        /// <summary>
        /// Creates a Weapon item from an XML node.
        /// </summary>
        /// <param name="weapon">The XML node containing the Weapon XML structure.</param>
        public Weapon(XmlNode weapon)
        {
            Name = weapon.Attributes["Name"].Value;
            Description = weapon.ChildNodes[0].FirstChild.Value;
            HitBonus = Convert.ToDouble(weapon.ChildNodes[1].FirstChild.Value);
            DamageRange = weapon.ChildNodes[2].FirstChild.Value;
            TwoHanded = Convert.ToBoolean(weapon.ChildNodes[3].FirstChild.Value);
            Weight = Convert.ToDouble(weapon.ChildNodes[4].FirstChild.Value);
            Value = Convert.ToDouble(weapon.ChildNodes[5].FirstChild.Value);
            UnsellableItem = Convert.ToBoolean(weapon.ChildNodes[6].FirstChild.Value);
            _index = Convert.ToInt32(weapon.ChildNodes[7].FirstChild.Value);
            GameData.POSSIBLE_ITEMS = GameData.POSSIBLE_ITEMS ?? new List<Item>();
            if (!GameData.POSSIBLE_ITEMS.Any(x => x.Name.Equals(this.Name)))
                GameData.POSSIBLE_ITEMS.Add(this);
        }

        public Weapon(Weapon weapon)
        {
            Name = weapon.Name;
            Description = weapon.Description;
            HitBonus = weapon.HitBonus;
            DamageRange = weapon.DamageRange;
            TwoHanded = weapon.TwoHanded;
            Weight = weapon.Weight;
            Value = weapon.Value;
            UnsellableItem = weapon.UnsellableItem;
            _index = weapon._index;
            Image = CreateTile.GetImageFromTileset(_index);
        }
        
        #endregion

    }
}
