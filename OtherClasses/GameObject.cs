using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheUndergroundTower.OtherClasses
{
    public abstract class GameObject
    {

        #region Properties

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        #endregion
        #region Constructors
        public GameObject() { }
        public GameObject(string name, string description)
        {
            Name = name;
            Description = description;
        }

        #endregion

    }
}
