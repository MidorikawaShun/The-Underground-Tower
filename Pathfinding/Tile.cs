using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheUndergroundTower.OtherClasses;
using System.Drawing;
using System.Xml;
using WpfApp1;
using System.Windows.Media;

namespace TheUndergroundTower.Pathfinding
{
    /// <summary>
    /// Class for floors and walls
    /// </summary>
    public class Tile : GameObject
    {
        private bool _walkable, _visible, _seethrough;
        private List<GameObject> _gameObjects;
        private ImageSource _image;
        private int _index;
        
        /// <summary>
        /// constructor for creating an initial tile object from XML
        /// </summary>
        /// <param name="tile"></param>
        public Tile(XmlNode tile)
        {
            Name = tile.Attributes["Name"].Value;
            Description = tile.ChildNodes[0].FirstChild.Value;
            _walkable = Convert.ToBoolean(tile.ChildNodes[2].FirstChild.Value); 
            _visible = true;
            _seethrough = Convert.ToBoolean(tile.ChildNodes[1].FirstChild.Value); 
            _gameObjects = new List<GameObject>();
            _index = Convert.ToInt32(tile.ChildNodes[3].FirstChild.Value);
            _image = CreateTile.GetImageFromTileset(_index);
            if (GameData.TILES == null) GameData.TILES = new List<Tile>();
            GameData.TILES.Add(this);
        }

        /// <summary>
        /// copy constructor to multiply tiles
        /// </summary>
        /// <param name="tile"></param>
        public Tile(Tile tile)
        {
            Name = tile.Name;
            Description = tile.Description;
            _walkable = tile.Walkable;
            _visible = tile.Visible;
            _seethrough = tile.Seethrough;
            _gameObjects = new List<GameObject>();
            _image = tile.Image;
          
        }

        public bool Walkable { get => _walkable; set => _walkable = value; }
        public bool Visible { get => _visible; set => _visible = value; }
        public bool Seethrough { get => _seethrough; set => _seethrough = value; }
        public ImageSource Image { get => _image; set => _image = value; }


    }
}
