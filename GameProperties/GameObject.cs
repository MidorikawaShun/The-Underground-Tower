using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TheUndergroundTower.Pathfinding;

namespace TheUndergroundTower.OtherClasses
{
    /// <summary>
    /// GameObjects are any non-WPF element that we use in the game.
    /// </summary>
    public abstract class GameObject
    {
        /// <summary>
        /// The list of all the gameobjects that currently exist in the game.
        /// Used so it's possible to get objects by their ID.
        /// </summary>
        private static List<GameObject> _gameObjects = new List<GameObject>();
        private static int _objectCounter = 1;

        #region Properties

        /// <summary>
        /// The name of the GameObject.
        /// </summary>
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The description of the GameObject.
        /// </summary>
        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// The ID of the GameObject.
        /// </summary>
        private string _id;
        public string ID
        {
            get { return _id; }
            private set { _id = value; }
        }

        private ImageSource _image;
        public ImageSource Image { get; set; }

        protected int _index;

        /// <summary>
        /// Coordinates of the creature/item and map the creature/item is on.
        /// Z is which map
        /// </summary>
        private int _x,_y,_z;
        public int X { get => _x; set { if (value >= 0) _x = value; } }
        public int Y { get => _y; set { if (value >= 0) _y = value; } }
        public int Z { get => _z; set { if (value >= 0) _z = value; } }
        #endregion
        #region Constructors
        /// <summary>
        /// Gives the object an ID and adds it to the list.
        /// </summary>
        public GameObject()
        {
            ID = "_" + _objectCounter++;
            _gameObjects.Add(this);
        }

        /// <summary>
        /// Constructor that gets name and description
        /// </summary>
        /// <param name="name">The object's name</param>
        /// <param name="description">The object's description</param>
        public GameObject(string name, string description)
        {
            Name = name;
            Description = description;
            ID = "_" + _objectCounter++;
            _gameObjects.Add(this);
        }


        #endregion
        
        /// <summary>
        /// Gets a GameObject by its ID.
        /// </summary>
        /// <param name="ID">The ID of the GameObject we want to find.</param>
        /// <returns>GameObject with the parameter ID.</returns>
        public static GameObject GetByID(string ID)
        {
            return _gameObjects.Where(x => x.ID.Equals(ID)).SingleOrDefault();
        }

        public virtual ImageSource GetImage()
        {
            return BitmapSource.Create(
                                         32, 32,
                                         96, 96,
                                         PixelFormats.Indexed1,
                                         new BitmapPalette(new List<System.Windows.Media.Color>() { System.Windows.Media.Colors.Transparent }),
                                         new byte[512],
                                         16);
        }

    }
}
