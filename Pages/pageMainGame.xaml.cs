using System;
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
using TheUndergroundTower.Creatures;
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

        private List<Monster> Monsters;
        private Random _rand;

        public pageMainGame()
        {
            InitializeComponent();
            GameData.InitializeTiles();
            GameData.InitializeMonsters();
            _rand = new Random(DateTime.Now.Millisecond);
            GameStatus.MAPS = new List<Map>();
            CreateDisplay();
            Monsters = GameStatus.CREATURES.Where(x=>x is Monster && x.Location.Z==GameStatus.MAPS.IndexOf(GameStatus.CURRENT_MAP)).Select(x=>x as Monster).ToList();
            Player p = GameStatus.PLAYER = new Player();
            p.Location = GetCreatureStartLocation();
            Room r = GameStatus.CURRENT_MAP.FindRoomByCoordinate(p.Location);
            GameStatus.CURRENT_MAP.Tiles[p.Location.X, p.Location.Y].Objects = new List<GameObject>();
            GameStatus.CURRENT_MAP.Tiles[p.Location.X, p.Location.Y].Objects.Add(p);
            RefreshScreen();



            //zeroZero.Source = GameData.POSSIBLE_TILES.FirstOrDefault().Image;
        }

        //create a map and fill it with empty image elements
        private void CreateDisplay()
        {
            Map map = new Map();
            GameStatus.CURRENT_MAP = map;
            GameStatus.MAPS.Add(map);
            for (int x = 0; x < Definitions.WINDOW_X_SIZE; x++)
                for (int y = 0; y < Definitions.WINDOW_Y_SIZE; y++)
                {
                    Image img = new Image();
                    img.Height = img.Width = 50;
                    XAMLMap.Children.Add(img);
                }
        }

        /// <summary>
        /// Returns a valid starting location for a creature.
        /// </summary>
        /// <returns>A tuple with the creatures starting location.</returns>
        public FullCoord GetCreatureStartLocation()
        {
            //Choose a random room on the map
            Room startRoom = GameStatus.CURRENT_MAP.Rooms[_rand.Next(GameStatus.CURRENT_MAP.Rooms.Count())];
            int xPos = _rand.Next(startRoom.TopLeft.X + 1, startRoom.TopLeft.X + startRoom.XSize);
            int yPos = _rand.Next(startRoom.BottomLeft.Y + 1, startRoom.BottomLeft.Y + startRoom.YSize);
            return new FullCoord(xPos, yPos, GameStatus.MAPS.IndexOf(GameStatus.CURRENT_MAP));
        }

        //Move the map in accordance with player's movement.
        private void RefreshScreen()
        {
            int xpos = GameStatus.PLAYER.Location.X - 5;
            int ypos = GameStatus.PLAYER.Location.Y - 6;
            int z = 0;
            for (int y = Definitions.WINDOW_Y_SIZE - 1; y >= 0; y--)
            {
                for (int x = 0; x < Definitions.WINDOW_X_SIZE; x++)
                {
                    bool tileExists = false;
                    if ((xpos + x) >= 0 && (ypos + y) >= 0 && (xpos + x) < GameStatus.CURRENT_MAP.XSize && (ypos - y) < GameStatus.CURRENT_MAP.YSize) //Make sure indices are in range of array
                    {
                        Tile tile = GameStatus.CURRENT_MAP.Tiles[xpos + x, ypos+y];
                        if (tile != null)
                        {
                            ImageSource overlayedImage = tile.Image;
                            if (tile.Objects != null && tile.Objects.Count > 0)
                                for (int i = 0; i < tile.Objects.Count; i++)
                                    overlayedImage = CreateTile.Overlay(overlayedImage, tile.Objects[i].GetImage());
                            (XAMLMap.Children[z++] as Image).Source = overlayedImage;
                            tileExists = true;
                        }
                    }
                    if (!tileExists)
                        (XAMLMap.Children[z++] as Image).Source = CreateBlackImage();
                }
            }
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

        private Tile GetTileFromCoordinate(FullCoord coord)
        {
            return GetTileFromCoordinate(new MapCoord(coord.X, coord.Y), GameStatus.MAPS[coord.Z]);
        }

        private Tile GetTileFromCoordinate(MapCoord coord, Map map = null)
        {
            Map targetMap = map ?? GameStatus.CURRENT_MAP;
            return targetMap.Tiles[coord.X, coord.Y];
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += HandleKeyPress;
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            Player p = GameStatus.PLAYER;
            Map map = GameStatus.CURRENT_MAP;
            map.Tiles[p.Location.X, p.Location.Y].Objects.Remove(p);
            Tile oldTile = map.Tiles[p.Location.X, p.Location.Y];
            string str = e.Key.ToString(); //get the string value of pressed key
            switch (str)
            {
                case "NumPad8":
                case "Up":
                    {
                        p.MoveTo(new MapCoord(p.Location.X, p.Location.Y + 1), map);
                        oldTile.Objects = oldTile.Objects.Count() == 1 && oldTile.Objects.Contains(p) ? null : oldTile.Objects;
                        break;
                    }
                case "NumPad2":
                case "Down":
                    {
                        p.MoveTo(new MapCoord(p.Location.X, p.Location.Y - 1), map);
                        oldTile.Objects = oldTile.Objects.Count() == 1 && oldTile.Objects.Contains(p) ? null : oldTile.Objects;
                        break;
                    }
                case "NumPad4":
                case "Left":
                    {
                        p.MoveTo(new MapCoord(p.Location.X - 1, p.Location.Y), map);
                        oldTile.Objects = oldTile.Objects.Count() == 1 && oldTile.Objects.Contains(p) ? null : oldTile.Objects;
                        break;
                    }
                case "NumPad6":
                case "Right":
                    {
                        p.MoveTo(new MapCoord(p.Location.X + 1, p.Location.Y), map);
                        oldTile.Objects = oldTile.Objects.Count() == 1 && oldTile.Objects.Contains(p) ? null : oldTile.Objects;
                        break;
                    }
                case "NumPad1":
                    {
                        p.MoveTo(new MapCoord(p.Location.X - 1, p.Location.Y -1), map);
                        oldTile.Objects = oldTile.Objects.Count() == 1 && oldTile.Objects.Contains(p) ? null : oldTile.Objects;
                        break;
                    }
                case "NumPad3":
                    {
                        p.MoveTo(new MapCoord(p.Location.X + 1, p.Location.Y -1), map);
                        oldTile.Objects = oldTile.Objects.Count() == 1 && oldTile.Objects.Contains(p) ? null : oldTile.Objects;
                        break;
                    }
                case "NumPad7":
                    {
                        p.MoveTo(new MapCoord(p.Location.X - 1, p.Location.Y +1), map);
                        oldTile.Objects = oldTile.Objects.Count() == 1 && oldTile.Objects.Contains(p) ? null : oldTile.Objects;
                        break;
                    }
                case "NumPad9":
                    {
                        p.MoveTo(new MapCoord(p.Location.X + 1, p.Location.Y +1), map);
                        oldTile.Objects = oldTile.Objects.Count()==1 && oldTile.Objects.Contains(p)?null:oldTile.Objects;
                        break;
                    }
                case "NumPad5":
                    {
                        break;
                    }
                default:
                    {
                        return;
                    }
            }
            Tile tile = GetTileFromCoordinate(p.Location);
            if (tile.Objects == null) tile.Objects = new List<GameObject>();
            tile.Objects.Add(p);
            MoveMonsters();
            RefreshScreen();
        }

        public void MoveMonsters()
        {
            foreach (Monster monster in Monsters)
            {
                if (monster.AwareOfPlayer)  { }
                else BrownianMotion(monster);
            }
        }

        /// <summary>
        /// Moves a creature around by one tile randomly.
        /// </summary>
        /// <param name="creature">The creature to be moved.</param>
        public void BrownianMotion(Creature creature)
        {
            Tile currentTile = GetTileFromCoordinate(creature.Location);
            List<MapCoord> coordsChecked = new List<MapCoord>();
            while (currentTile.Objects!=null && currentTile.Objects.Contains(creature))
            {
                if (coordsChecked.Count() == 8) return; //No walkable tile to move to
                int newX = _rand.Next(-1, 2) + creature.Location.X;
                int newY = _rand.Next(-1, 2) + creature.Location.Y;
                Tile newTile = GameStatus.CURRENT_MAP.Tiles[newX, newY];
                if (coordsChecked.Where(coord => coord.X == newX && coord.Y == newY).Count()>0) continue;
                if (newTile.IsWalkable())
                {
                    currentTile.Objects.Remove(creature);
                    currentTile.Objects = currentTile.Objects.Count == 0 ? null : currentTile.Objects;
                    newTile.Objects = newTile.Objects ?? new List<GameObject>();
                    newTile.Objects.Add(creature);
                    creature.Location = new FullCoord(newX, newY, creature.Location.Z);
                }
                else coordsChecked.Add(new MapCoord(newX, newY));
            }
        }

        private void MainGamePage_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
