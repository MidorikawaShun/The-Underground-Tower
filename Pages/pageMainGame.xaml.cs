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
        Player Player;
        private List<Monster> Monsters;
        private Random _rand;
        private const double MAX_MONSTERS_MULTIPLIER = 2;
        private const double MIN_MONSTERS_MULTIPLIER = 0.5;
        private const int MAX_MONSTER_PLACEMENT_ATTEMPTS = 10;

        public pageMainGame()
        {
            InitializeComponent();
            GameData.InitializeTiles();
            GameData.InitializeMonsters();
            _rand = new Random(DateTime.Now.Millisecond);
            GameStatus.MAPS = new List<Map>();
            GameStatus.CURRENT_MAP = new Map(60);
            GameStatus.MAPS.Add(GameStatus.CURRENT_MAP);
            GenerateMonsters();
            CreateDisplay();
            Player = GameStatus.PLAYER = new Player();
            //Tile Startpoint = GameStatus.CURRENT_MAP.Tiles[_rand.Next(1, GameStatus.CURRENT_MAP.XSize), _rand.Next(1, GameStatus.CURRENT_MAP.YSize)];
            Map map = GameStatus.CURRENT_MAP;
            SetInitialPlayerLocation(map);
            RefreshScreen();
            map.DrawMapToConsole();
        }

        private void GenerateMonsters()
        {
            List<Monster> monsters = GameData.POSSIBLE_MONSTERS;
            Map currentMap = GameStatus.CURRENT_MAP;
            int monstersInThisStage = _rand.Next((int)(currentMap.Rooms.Count * MIN_MONSTERS_MULTIPLIER), (int)(currentMap.Rooms.Count * MAX_MONSTERS_MULTIPLIER));
            Monsters = new List<Monster>();

            for (int i = 0; i < monstersInThisStage; i++) //Place 'monstersInThisStage' monsters on this map
            {
                //Make sure that the monster fits
                for (int attemptedMonsterPlacements = 0; attemptedMonsterPlacements < MAX_MONSTER_PLACEMENT_ATTEMPTS; attemptedMonsterPlacements++)
                {
                    Room room = GameStatus.CURRENT_MAP.Rooms.Random(_rand);
                    int x = _rand.Next(room.TopLeftX + 1, room.TopRightX);
                    int y = _rand.Next(room.BottomLeftY + 1, room.TopLeftY);
                    Monster newMonster = new Monster(monsters.Random(_rand)) { X = x, Y = y, Z = GameStatus.MAPS.Count() - 1 };
                    Tile tile = GameStatus.CURRENT_MAP.Tiles[x, y];
                    tile.Objects = tile.Objects ?? new List<GameObject>() { newMonster };
                    if (tile.Objects.Count == 0)
                        tile.Objects.Add(newMonster);
                    if (tile.Objects.Contains(newMonster)) Monsters.Add(newMonster);
                }

            }
        }

        private void SetInitialPlayerLocation(Map map)
        {
            Room startingRoom = map.Rooms.First();
            Player.X = startingRoom.TopLeftX + startingRoom.XSize / 2;
            Player.Y = startingRoom.TopLeftY - startingRoom.YSize / 2;
            map.Tiles[Player.X, Player.Y].Objects = new List<GameObject>() { Player };
        }

        //create a map and fill it with empty image elements
        private void CreateDisplay()
        {
            for (int x = 0; x < Definitions.WINDOW_X_SIZE; x++)
                for (int y = 0; y < Definitions.WINDOW_Y_SIZE; y++)
                {
                    Image img = new Image();
                    img.Height = img.Width = 50;
                    XAMLMap.Children.Add(img);
                }
        }

        //Move the map in accordance with Player's movement.
        private void RefreshScreen()
        {
            int xpos = Player.X - 5;
            int ypos = Player.Y - 6;
            int z = 0;
            for (int y = Definitions.WINDOW_Y_SIZE - 1; y >= 0; y--)
            {
                for (int x = 0; x < Definitions.WINDOW_X_SIZE; x++)
                {
                    bool tileExists = false;
                    if ((xpos + x) >= 0 && (ypos + y) >= 0 && (xpos + x) < GameStatus.CURRENT_MAP.XSize && (ypos + y) < GameStatus.CURRENT_MAP.YSize) //Make sure indices are in range of array
                    {
                        Tile tile = GameStatus.CURRENT_MAP.Tiles[xpos + x, ypos + y];
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

        private Tile GetTileFromCoordinate(int x, int y)
        {
            return GameStatus.CURRENT_MAP.Tiles[x, y];
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += HandleKeyPress;
        }

        /// <summary>
        /// This is our main game loop.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            Map map = GameStatus.CURRENT_MAP;
            Tile oldTile = map.Tiles[Player.X, Player.Y];
            string str = e.Key.ToString(); //get the string value of pressed key
            switch (str)
            {
                case "NumPad8":
                case "Up":
                    {
                        Player.MoveTo(Player.X, Player.Y + 1, map);
                        break;
                    }
                case "NumPad2":
                case "Down":
                    {
                        Player.MoveTo(Player.X, Player.Y - 1, map);
                        break;
                    }
                case "NumPad4":
                case "Left":
                    {
                        Player.MoveTo(Player.X - 1, Player.Y, map);
                        break;
                    }
                case "NumPad6":
                case "Right":
                    {
                        Player.MoveTo(Player.X + 1, Player.Y, map);
                        break;
                    }
                case "NumPad1":
                    {
                        Player.MoveTo(Player.X - 1, Player.Y - 1, map);
                        break;
                    }
                case "NumPad3":
                    {
                        Player.MoveTo(Player.X + 1, Player.Y - 1, map);
                        break;
                    }
                case "NumPad7":
                    {
                        Player.MoveTo(Player.X - 1, Player.Y + 1, map);
                        break;
                    }
                case "NumPad9":
                    {
                        Player.MoveTo(Player.X + 1, Player.Y + 1, map);
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
            Map map = GameStatus.CURRENT_MAP;
            foreach (Monster monster in Monsters)
            {
                List<Tile> path = null;
                //Get the line representing the tiles between monster and Player
                List<Tile> line = Algorithms.Line(monster.X, Player.X, monster.Y, Player.Y, map, IsSeeThrough);
                monster.AwareOfPlayer = line == null ? false : true;
                if (monster.AwareOfPlayer)
                {
                    monster.FollowingPlayer = true;
                    monster.LastKnownPlayerLocationX = Player.X;
                    monster.LastKnownPlayerLocationY = Player.Y;
                    monster.TurnsWithoutPlayerInSight = 0;
                }
                else
                    monster.TurnsWithoutPlayerInSight++;
                if (monster.FollowingPlayer) //move monsters
                {
                    if (monster.AwareOfPlayer && monster.FollowingPlayer) path = Algorithms.FindPath(map, map.Tiles[monster.X, monster.Y], map.Tiles[Player.X, Player.Y]);
                    if (!monster.AwareOfPlayer && monster.FollowingPlayer) path = Algorithms.FindPath(map, map.Tiles[monster.X, monster.Y], map.Tiles[monster.LastKnownPlayerLocationX, monster.LastKnownPlayerLocationY]);
                    if (path != null)
                    {
                        int pathX = path.First().X, pathY = path.First().Y;
                        monster.MoveTo(pathX, pathY, map);
                    }
                }
                else BrownianMotion(monster);
            }
        }

        //If the line is not null and Player is also within sight range of the monster, monster is now aware of Player.
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
            if (tile == null) return false;
            else return tile.Seethrough;
        }

        /// <summary>
        /// Moves a creature around by one tile randomly.
        /// </summary>
        /// <param name="creature">The creature to be moved.</param>
        public void BrownianMotion(Creature creature)
        {
            Tile currentTile = GetTileFromCoordinate(creature.X, creature.Y);
            List<Tile> tilesChecked = new List<Tile>();
            while (currentTile.Objects != null && currentTile.Objects.Contains(creature))
            {
                if (tilesChecked.Count() == 8) return; //No walkable tile to move to
                int newX = _rand.Next(-1, 2) + creature.X;
                int newY = _rand.Next(-1, 2) + creature.Y;
                Tile newTile = GameStatus.CURRENT_MAP.Tiles[newX, newY];
                if (tilesChecked.Where(coord => coord.X == newX && coord.Y == newY).Count() > 0) continue;
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
