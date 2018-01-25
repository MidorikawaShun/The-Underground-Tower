using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WpfApp1;

namespace TheUndergroundTower.OtherClasses
{
    /// <summary>
    /// Represents an armor object that the player can wear.
    /// </summary>
    public class Armor : Item
    {

        #region Properties

        /// <summary>
        /// How harder to hit does this make the player.
        /// </summary>
        private int _armorBonus;
        public int ArmorBonus
        {
            get { return _armorBonus; }
            set { _armorBonus = value; }
        }

        /// <summary>
        /// If the armour is held in a hand. Like shields.
        /// </summary>
        private bool _heldInHand;
        public bool HeldInHand
        {
            get { return _heldInHand; }
            set { _heldInHand = value; }
        }

        private string _type;
        public string Type
        {
            get { return _type; }
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
            Description = armor.ChildNodes[0].FirstChild.Value;
            ArmorBonus = Convert.ToInt32(armor.ChildNodes[1].FirstChild.Value);
            HeldInHand = Convert.ToBoolean(armor.ChildNodes[2].FirstChild.Value);
            Weight = Convert.ToDouble(armor.ChildNodes[3].FirstChild.Value);
            Value = Convert.ToDouble(armor.ChildNodes[4].FirstChild.Value);
            UnsellableItem = Convert.ToBoolean(armor.ChildNodes[5].FirstChild.Value);
            _index = Convert.ToInt32(armor.ChildNodes[6].FirstChild.Value);
            _type = armor.ChildNodes[7].FirstChild.Value;
            if (!GameData.POSSIBLE_ITEMS.Any(x => x.Name.Equals(this.Name)))
                GameData.POSSIBLE_ITEMS.Add(this);
        }

        public Armor(Armor armor)
        {
            Name = armor.Name;
            Description = armor.Description;
            ArmorBonus = armor.ArmorBonus;
            HeldInHand = armor.HeldInHand;
            Weight = armor.Weight;
            Value = armor.Value;
            UnsellableItem = armor.UnsellableItem;
            _index = armor._index;
            Image = CreateTile.GetImageFromTileset(_index);
            _type = armor.Type;
        }

        #endregion

    }
}
