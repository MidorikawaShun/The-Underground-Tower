using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheUndergroundTower.OtherClasses
{
    class Item
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {

            }
        }

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

        private bool _unsellableItem;
        public bool UnsellableItem
        {
            get { return _unsellableItem; }
            set { _unsellableItem = value; }
        }

    }
}
