using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WpfApp1.Windows.MetaMenus;
using static WpfApp1.Utilities;

namespace TheUndergroundTower.OtherClasses
{
    public class Weapon : Item
    {

        #region Properties

        private double _speed;
        public double Speed
        {
            get { return _speed; }
            set
            {
                if (value > 0)
                    _speed = value;
            }
        }

        private double _hitBonus;
        public double HitBonus
        {
            get { return _hitBonus; }
            set
            {
                if (value > 0)
                    _hitBonus = value;
            }
        }

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
            Speed = Convert.ToDouble(weapon.ChildNodes[1].FirstChild.Value);
            HitBonus = Convert.ToDouble(weapon.ChildNodes[2].FirstChild.Value);
            DamageRange = weapon.ChildNodes[3].FirstChild.Value;
            TwoHanded = Convert.ToBoolean(weapon.ChildNodes[4].FirstChild.Value);
            Weight = Convert.ToDouble(weapon.ChildNodes[5].FirstChild.Value);
            Value = Convert.ToDouble(weapon.ChildNodes[6].FirstChild.Value);
            UnsellableItem = Convert.ToBoolean(weapon.ChildNodes[7].FirstChild.Value);
        }
        
        #endregion

    }
}
