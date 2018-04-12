using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;
using WpfApp1;

namespace TheUndergroundTower.OtherClasses
{
    /// <summary>
    /// Represents items such as weapons, armours, jewelery, clothing.
    /// </summary>
    public class Item : GameObject
    {

        #region Properties

        /// <summary>
        /// The weight of the item.
        /// </summary>
        private double _weight;
        public double Weight
        {
            get { return _weight; }
            set
            {
                if (value >= 0)
                    _weight = value;
            }
        }

        /// <summary>
        /// The worth of an item.
        /// </summary>
        private double _value;
        public double Value
        {
            get { return _value; }
            set
            {
                if (value >= 0)
                    _value = value;
            }
        }

        /// <summary>
        /// Can the user sell this item?
        /// </summary>
        private bool _unsellableItem;
        public bool UnsellableItem
        {
            get { return _unsellableItem; }
            set { _unsellableItem = value; }
        }

        private bool _isGameEnderItem;
        public bool IsGameEnderItem
        {
            get { return _isGameEnderItem; }
            set { _isGameEnderItem = value; }
        }


        #endregion

        public Item() { }

        public Item(XmlNode item)
        {
            Item itemObject = new Item()
            {
                Name = item.Attributes["Name"].Value,
                Description = item.ChildNodes[0].FirstChild.Value,
                _index = Convert.ToInt32(item.ChildNodes[1].FirstChild.Value),
                _isGameEnderItem = Convert.ToBoolean(item.ChildNodes[2].FirstChild.Value)
            };
            GameData.POSSIBLE_ITEMS.Add(itemObject);
        }

        public Item(Item newItem)
        {
            switch (newItem.GetType().Name)
            {
                case "Weapon":
                    new Weapon(newItem as Weapon);
                    break;
                case "Armor":
                    new Armor(newItem as Armor);
                    break;
                case "Item":
                    _isGameEnderItem = newItem.IsGameEnderItem;
                    Description = newItem.Description;
                    Name = newItem.Name;
                    _index = newItem._index;
                    Image = CreateTile.GetImageFromTileset(newItem._index);
                    break;
                default: break;
            }
        }

        //Factory method. Required to return proper polymorphic object.
        public static Item Create(Item newItem)
        {
            Item retVal = null;
            switch (newItem.GetType().Name)
            {
                case "Weapon":
                    retVal = new Weapon(newItem as Weapon);
                    break;
                case "Armor":
                    retVal = new Armor(newItem as Armor);
                    break;
                default: break;
            }
            return retVal;
        }

        public override ImageSource GetImage()
        {
            return Image;
        }

    }
}
