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
            Monsters = GameStatus.CREATURES.Where(x=>x is Monster && x.Z==GameStatus.MAPS.IndexOf(GameStatus.CURRENT_MAP)).Select(x=>x as Monster).ToList();
            Player p = GameStatus.PLAYER = new Player();
            //Tile Startpoint = GameStatus.CURRENT_MAP.Tiles[_rand.Next(1, GameStatus.CURRENT_MAP.XSize), _rand.Next(1, GameStatus.CURRENT_MAP.YSize)];
            Map map = GameStatus.CURRENT_MAP;
            SetInitialPlayerLocation(map);
            RefreshScreen();

        }

        private void SetInitialPlayerLocation(Map map)
        {
            Room startingRoom = map.Rooms.First();
            Player p = GameStatus.PLAYER;
            p.X = startingRoom.TopLeftX + startingRoom.XSize / 2;
            p.Y = startingRoom.TopLeftY - startingRoom.YSize / 2;
            map.Tiles[p.X, p.Y].Objects = new List<GameObject>() { p };
        }

        //create a map and fill it with empty image elements
        private void CreateDisplay()
        {
            Map map = new Map(45);
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

        //Move the map in accordance with player's movement.
        private void RefreshScreen()
        {
            int xpos = GameStatus.PLAYER.X - 5;
            int ypos = GameStatus.PLAYER.Y - 6;
            int z = 0;
            for (int y = Definitions.WINDOW_Y_SIZE - 1; y >= 0; y--)
            {
                for (int x = 0; x < Definitions.WINDOW_X_SIZE; x++)
                {
                    bool tileExists = false;
                    if ((xpos + x) >= 0 && (ypos + y) >= 0 && (xpos + x) < GameStatus.CURRENT_MAP.XSize && (ypos + y) < GameStatus.CURRENT_MAP.YSize) //Make sure indices are in range of array
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

        private Tile GetTileFromCoordinate(int x,int y)
        {
            return GameStatus.CURRENT_MAP.Tiles[x, y];
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
            Tile oldTile = map.Tiles[p.X, p.Y];
            string str = e.Key.ToString(); //get the string value of pressed key
            switch (str)
            {
                case "NumPad8":
                case "Up":
                    {
                        p.MoveTo(p.X, p.Y + 1, map);
                        break;
                    }
                case "NumPad2":
                case "Down":
                    {
                        p.MoveTo(p.X, p.Y - 1, map);
                        break;
                    }
                case "NumPad4":
                case "Left":
                    {
                        p.MoveTo(p.X - 1, p.Y, map);
                        break;
                    }
                case "NumPad6":
                case "Right":
                    {
                        p.MoveTo(p.X + 1, p.Y, map);
                        break;
                    }
                case "NumPad1":
                    {
                        p.MoveTo(p.X - 1, p.Y -1, map);
                        break;
                    }
                case "NumPad3":
                    {
                        p.MoveTo(p.X + 1, p.Y -1, map);
                        break;
                    }
                case "NumPad7":
                    {
                        p.MoveTo(p.X - 1, p.Y +1, map);
                        break;
                    }
                case "NumPad9":
                    {
                        p.MoveTo(p.X + 1, p.Y +1, map);
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
            MoveMonsters();
            RefreshScreen();
        }

        public void MoveMonsters()
        {
            foreach (Monster monster in Monsters)
            {
                ////Get the line representing the tiles between monster and player
                //List<Tile> line = BersenhamLine.Line(monster.X,monster.Y,GameStatus.PLAYER.X,GameStatus.PLAYER.Y, GameStatus.CURRENT_MAP, IsSeeThrough);
                //if (!monster.AwareOfPlayer)
                //{
                //    if (IsPlayerInSight(line))
                //    {
                //        //monster.AwareOfPlayer = true;
                //        foreach (var item in line)
                //        {
                //            GameStatus.CURRENT_MAP.Tiles[item.X, item.Y].Image = new DrawingImage() { };
                //        }
                //    }
                //    //else BrownianMotion(monster); //If monster is not aware of player, move randomly.
                //}
                //if (monster.AwareOfPlayer)
                //{
                //    if (!IsPlayerInSight(line)) monster.TurnsWithoutPlayerInSight++;
                //    if (monster.TurnsWithoutPlayerInSight == 5)
                //    {
                //        monster.AwareOfPlayer = false;
                //        monster.TurnsWithoutPlayerInSight = 0;
                //    }
                //    else
                //    {
                //        //ADD SOMETHING
                //    }
                //}
            }
        }

        //If the line is not null and player is also within sight range of the monster, monster is now aware of player.
        public bool IsPlayerInSight(List<Tile> line)
        {
            return line != null && Monster.SIGHTRANGE > Math.Abs(line.First().X - line.Last().X) && Monster.SIGHTRANGE > Math.Abs(line.First().Y - line.Last().Y);
        }

        public bool IsWalkable(Tile tile)
        {
            return tile.Walkable && (tile.Objects == null || tile.Objects != null && tile.Objects.Where(x => x is Creature).Count() == 0);
        }

        public bool IsSeeThrough(Tile tile)
        {
            return tile.Seethrough;
        }

        /// <summary>
        /// Moves a creature around by one tile randomly.
        /// </summary>
        /// <param name="creature">The creature to be moved.</param>
        public void BrownianMotion(Creature creature)
        {
            Tile currentTile = GetTileFromCoordinate(creature.X,creature.Y);
            List<Tile> tilesChecked = new List<Tile>();
            while (currentTile.Objects!=null && currentTile.Objects.Contains(creature))
            {
                if (tilesChecked.Count() == 8) return; //No walkable tile to move to
                int newX = _rand.Next(-1, 2) + creature.X;
                int newY = _rand.Next(-1, 2) + creature.Y;
                Tile newTile = GameStatus.CURRENT_MAP.Tiles[newX, newY];
                if (tilesChecked.Where(coord => coord.X == newX && coord.Y == newY).Count()>0) continue;
                if (IsWalkable(newTile))
                {
                    currentTile.Objects.Remove(creature);
                    currentTile.Objects = currentTile.Objects.Count == 0 ? null : currentTile.Objects;
                    newTile.Objects = newTile.Objects ?? new List<GameObject>();
                    newTile.Objects.Add(creature);
                    creature.X = newX; creature.Y = newY;
                }
                else tilesChecked.Add(GameStatus.CURRENT_MAP.Tiles[newX, newY]);
            }
        }

        private void MainGamePage_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
