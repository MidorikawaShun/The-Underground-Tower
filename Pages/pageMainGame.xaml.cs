﻿using System;
using System.Collections.Generic;

using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TheUndergroundTower.OtherClasses;
using TheUndergroundTower.Pathfinding;
using WpfApp1;
using WpfApp1.Creatures;
using WpfApp1.GameProperties;

namespace TheUndergroundTower.Pages
{
    /// <summary>
    /// Interaction logic for pageMainGame.xaml
    /// </summary>
    public partial class pageMainGame : Page
    {

        public pageMainGame()
        {
            InitializeComponent();
            GameData.InitializeTiles();
            GameStatus.MAPS = new List<Map>();

            CreateDisplay();
            Player p = GameStatus.PLAYER = new Player();
            p.Location = GetCreatureStartLocation();
            Room r = GameStatus.CURRENT_MAP.FindRoomByCoordinate(p.Location);
            GameStatus.CURRENT_MAP.Tiles[p.Location.Item1, p.Location.Item2].Objects = new List<GameObject>();
            GameStatus.CURRENT_MAP.Tiles[p.Location.Item1, p.Location.Item2].Objects.Add(p);
            RefreshScreen();



            //zeroZero.Source = GameData.POSSIBLE_TILES.FirstOrDefault().Image;
        }

        //create a map and fill it with empty image elements
        private void CreateDisplay()
        {
            Map map = new Map();
            GameStatus.CURRENT_MAP = map;
            //add map to the list of maps
            GameStatus.MAPS.Add(map);
            List<List<Image>> imageLists = new List<List<Image>>();
            //fill the map with empty images
            for (int i = 0; i < Definitions.WINDOW_X_SIZE; i++)
            {
                imageLists.Add(new List<Image>());
                for (int j = 0; j < Definitions.WINDOW_Y_SIZE; j++)
                {
                    Image img = new Image();
                    img.Width = img.Height = 50;
                    imageLists.Last().Add(img);
                    //XAMLMap.Children.Add(img);
                }
            }
            imageLists.Reverse();
            foreach (List<Image> list in imageLists)
                foreach (Image img in list)
                    XAMLMap.Children.Add(img);
        }

        /// <summary>
        /// Returns a valid starting location for a creature.
        /// </summary>
        /// <returns>A tuple with the creatures starting location.</returns>
        public Tuple<int, int, int> GetCreatureStartLocation()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            //Choose a random room on the map
            Room startRoom = GameStatus.CURRENT_MAP.Rooms[rand.Next(GameStatus.CURRENT_MAP.Rooms.Count())];
            int xPos = rand.Next(startRoom.TopLeft.Item1 + 1, startRoom.TopLeft.Item1 + startRoom.XSize);
            int yPos = rand.Next(startRoom.BottomLeft.Item2 + 1, startRoom.BottomLeft.Item2 + startRoom.YSize);
            return new Tuple<int, int, int>(xPos, yPos, GameStatus.MAPS.IndexOf(GameStatus.CURRENT_MAP));
        }

        //Move the map in accordance with player's movement.
        private void RefreshScreen()
        {
            int xpos = GameStatus.PLAYER.Location.Item1 - 5;
            int ypos = GameStatus.PLAYER.Location.Item2 - 5;
            int y = 0;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    bool tileExists = false;
                    if ((xpos + i) >= 0 && (ypos + j) >= 0 && (xpos + i) < GameStatus.CURRENT_MAP.XSize && (ypos + j) < GameStatus.CURRENT_MAP.YSize) //Make sure indices are in range of array
                    {
                        Tile tile = GameStatus.CURRENT_MAP.Tiles[xpos + i, ypos + j];
                        if (tile != null)
                        {
                            ImageSource overlayedImage = tile.Image;
                            if (tile.Objects != null && tile.Objects.Count > 0) //check whether there are items on the tile
                            {

                                int n = tile.Objects.Count();
                                for (int x = 0; x < n; x++)
                                {
                                    GameObject obj = tile.Objects[x];
                                    overlayedImage = CreateTile.Overlay(overlayedImage, obj.GetImage());
                                }

                            }
                            (XAMLMap.Children[y] as Image).Source = overlayedImage;
                            tileExists = true;
                        }
                    }
                    if (!tileExists)//if tile doesn't exist, create black image instead
                    {
                        (XAMLMap.Children[y] as Image).Source = CreateBlackImage();
                    }
                }
            }

            //((Image)XAMLMap.Children[XSize + 1]).Source = CreateTile.Overlay(map.Tiles[5, 5].Image, p.Image);

        }

        private BitmapSource CreateBlackImage()
        {
            return BitmapSource.Create(
                                         32, 32,
                                         96, 96,
                                         PixelFormats.Indexed1,
                                         new BitmapPalette(new List<System.Windows.Media.Color>() { System.Windows.Media.Colors.Black }),
                                         new byte[512],
                                         16);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += HandleKeyPress;
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            Player p = GameStatus.PLAYER;

            GameStatus.CURRENT_MAP.Tiles[p.Location.Item1, p.Location.Item2].Objects.Remove(p);
            Tile tile = null;
            string str = e.Key.ToString(); //get the string value of pressed key
            switch (str)
            {
                case "Up":
                    {
                        p.Location = new Tuple<int, int, int>(p.Location.Item1 - 1, p.Location.Item2, p.Location.Item3);
                        tile = GameStatus.CURRENT_MAP.Tiles[p.Location.Item1, p.Location.Item2];
                        break;
                    }
                case "Down":
                    {
                        p.Location = new Tuple<int, int, int>(p.Location.Item1 + 1, p.Location.Item2, p.Location.Item3);
                        tile = GameStatus.CURRENT_MAP.Tiles[p.Location.Item1, p.Location.Item2];
                        break;
                    }
                case "Left":
                    {
                        p.Location = new Tuple<int, int, int>(p.Location.Item1, p.Location.Item2 - 1, p.Location.Item3);
                        tile = GameStatus.CURRENT_MAP.Tiles[p.Location.Item1, p.Location.Item2];
                        break;
                    }
                case "Right":
                    {
                        p.Location = new Tuple<int, int, int>(p.Location.Item1, p.Location.Item2 + 1, p.Location.Item3);
                        tile = GameStatus.CURRENT_MAP.Tiles[p.Location.Item1, p.Location.Item2];
                        break;
                    }
                default:
                    {
                        return;
                    }
            }
            /*
            if (e.Key.Equals(Key.Up)) //if the user presses up arrow, we want to take the player's position and decrement Y coordinate
            {
                p.Location = new Tuple<int, int, int>(p.Location.Item1-1, p.Location.Item2, p.Location.Item3);
                tile = GameStatus.CURRENT_MAP.Tiles[p.Location.Item1, p.Location.Item2];
            }
            if (e.Key.Equals(Key.Down)) //increment Y coordinate
            {
                p.Location = new Tuple<int, int, int>(p.Location.Item1+1, p.Location.Item2, p.Location.Item3);
                tile = GameStatus.CURRENT_MAP.Tiles[p.Location.Item1, p.Location.Item2];
            }
            if (e.Key.Equals(Key.Right)) //increment X coordinate
            {
                p.Location = new Tuple<int, int, int>(p.Location.Item1, p.Location.Item2+1, p.Location.Item3);
                tile = GameStatus.CURRENT_MAP.Tiles[p.Location.Item1, p.Location.Item2];
            }
            if (e.Key.Equals(Key.Left)) //decrement X coordinate
            {
                p.Location = new Tuple<int, int, int>(p.Location.Item1, p.Location.Item2-1, p.Location.Item3);
                tile = GameStatus.CURRENT_MAP.Tiles[p.Location.Item1, p.Location.Item2];
            }
            */
            if (tile.Objects == null) tile.Objects = new List<GameObject>();
            tile.Objects.Add(p);
            RefreshScreen();
        }

        private void MainGamePage_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
