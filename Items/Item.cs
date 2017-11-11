using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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

        /// <summary>
        /// The visual representation of the creature.
        /// </summary>
        private ImageSource _image;
        public ImageSource Image { get => _image; set => _image = value; }

        #endregion

        
        public override ImageSource GetImage()
        {
            return Image;
        }

    }
}
