using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WpfApp1.Utilities;

namespace TheUndergroundTower.OtherClasses
{
    public abstract class GameObject
    {

        private static List<GameObject> _gameObjects = new List<GameObject>();
        private static int _objectCounter = 1;

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

        private string _id;
        public string ID
        {
            get { return _id; }
            private set { _id = value; }
        }

        #endregion
        #region Constructors
        public GameObject()
        {
            ID = "_" + _objectCounter++;
            _gameObjects.Add(this);
        }

        public GameObject(string name, string description)
        {
            Name = name;
            Description = description;
            ID = "_" + _objectCounter++;
            _gameObjects.Add(this);
        }

        #endregion
        

        public static GameObject GetByID(string ID)
        {
            return _gameObjects.Where(x => x.ID.Equals(ID)).SingleOrDefault();
        }

    }
}
