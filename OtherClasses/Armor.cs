using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TheUndergroundTower.OtherClasses
{
    public class Armor : Item
    {

        #region Properties

        private int _armorBonus;
        public int ArmorBonus
        {
            get { return _armorBonus; }
            set { _armorBonus = value; }
        }

        private bool _heldInHand;
        public bool HeldInHand
        {
            get { return _heldInHand; }
            set { _heldInHand = value; }
        }

        #endregion
        #region Constructors

        /// <summary>
        /// Empty Armor constructor.
        /// </summary>
        public Armor() { }

        /// <summary>
        /// Creates an Armor item from an XML node.
        /// </summary>
        /// <param name="armor">The XML node that contains the Armor XML structure.</param>
        public Armor(XmlNode armor)
        {
            Name = armor.Attributes["Name"].Value;
            Description = armor.ChildNodes[0].Value;
            ArmorBonus = Convert.ToInt32(armor.ChildNodes[1].Value);
            HeldInHand = Convert.ToBoolean(armor.ChildNodes[2].Value);
            Weight = Convert.ToDouble(armor.ChildNodes[3].Value);
            Value = Convert.ToDouble(armor.ChildNodes[4].Value);
            UnsellableItem = Convert.ToBoolean(armor.ChildNodes[5].Value);
        }

        #endregion

    }
}
