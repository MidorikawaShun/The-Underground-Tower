﻿using System;
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
        #region Properties
        /// <summary>
        /// Can you walk through this tile?
        /// </summary>
        private bool _walkable;
        public bool Walkable { get => _walkable; set => _walkable = value; }

        /// <summary>
        /// Can you see this tile?
        /// </summary>
        private bool _visible;
        public bool Visible { get => _visible; set => _visible = value; }

        /// <summary>
        /// Can you see through this tile?
        /// </summary>
        private bool _seethrough;
        public bool Seethrough { get => _seethrough; set => _seethrough = value; }

        /// <summary>
        /// The visual representation of the tile.
        /// </summary>
        private ImageSource _image;
        public ImageSource Image { get => _image; set => _image = value; }

        private List<GameObject> _objects;
        public List<GameObject> Objects { get => _objects;  set => _objects = value; }

        #endregion

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
            _index = Convert.ToInt32(tile.ChildNodes[3].FirstChild.Value);
            _image = CreateTile.GetImageFromTileset(_index);
            if (GameData.POSSIBLE_TILES == null) GameData.POSSIBLE_TILES = new List<Tile>();
            //save the created tile into the list of all tiles that can be created and put on a map
            GameData.POSSIBLE_TILES.Add(this);
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
            _image = tile.Image;
            //save the created tile in the list of actual tiles present in the game
            GameStatus.TILES.Add(this); 
        }

    }
}
