using System;
using System.Collections.Generic;

using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
using TheUndergroundTower.Windows.MetaMenus;
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
        private Random _rand;

        public pageMainGame()
        {
            InitializeComponent();
            GameData.InitializeTiles();
            Console.WriteLine("Tiles initialized");
            GameData.InitializeMonsters();
            Console.WriteLine("Monsters initialized");
            GameData.InitializeItems();
            Console.WriteLine("Items initialized");
            _rand = new Random(DateTime.Now.Millisecond);
            GameStatus.MAPS = new List<Map>();
            GameStatus.STAIRS_DOWN_LOCATIONS = new Dictionary<Map, Tile>();
            GameStatus.STAIRS_UP_LOCATIONS = new Dictionary<Map, Tile>();
            GameStatus.CURRENT_MAP = new Map(60);
            Console.WriteLine("Map generated");
            GameStatus.MAPS.Add(GameStatus.CURRENT_MAP);
            Console.WriteLine("Monsters generated");
            Console.WriteLine("Items generated");
            CreateDisplay();
            Console.WriteLine("Display generated");
            GameLogic.GameLog = GameLog; //initialize gamelog
            GameStatus.PLAYER = GameStatus.PLAYER ?? new Player();
            Map map = GameStatus.CURRENT_MAP;
            SetInitialPlayerLocation(map);
            RefreshScreen();
        }

        private void SetInitialPlayerLocation(Map map)
        {
            Room startingRoom = null;
            int selectedX = 0, selectedY = 0;
            bool firstLoop = true;
            while (firstLoop || (map.TileExists(selectedX, selectedY) && (map.Tiles[selectedX, selectedY].Objects != null && map.Tiles[selectedX, selectedY].Objects.OfType<Monster>().Count() > 0)))
            {
                startingRoom = map.Rooms.Random(_rand);
                selectedX = startingRoom.TopLeftX + startingRoom.XSize / 2;
                selectedY = startingRoom.TopLeftY - startingRoom.YSize / 2;
                firstLoop = false;
            }
            GameStatus.PLAYER.X = selectedX;
            GameStatus.PLAYER.Y = selectedY;
            map.Tiles[GameStatus.PLAYER.X, GameStatus.PLAYER.Y].Objects = new List<GameObject>() { GameStatus.PLAYER };
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
            CreateInventoryGrid();
        }

        private void CreateInventoryGrid()
        {
            int NumOfInventorySlots = InventoryGrid.Rows * InventoryGrid.Columns;
            for (int i = 0; i < NumOfInventorySlots; i++)
            {
                Border border = new Border();
                border.BorderBrush = new System.Windows.Media.SolidColorBrush() { Color = Colors.Black };
                border.BorderThickness = new Thickness(1);
                Canvas canvas = new Canvas()
                {
                    Width = 36,
                    Height = 36,
                    Background = new RadialGradientBrush(Colors.DarkGray, Colors.Gray)
                };
                Button button = new Button()
                {
                    Opacity = 0.5,
                    Width = 36,
                    Height = 36,
                    Content = new Image() { Height = 36, Width = 36 }
                    //Width = InventoryGrid.Width / InventoryGrid.Columns,
                    //Height = InventoryGrid.Height / InventoryGrid.Rows,
                    //Background = new RadialGradientBrush(Colors.DarkGray, Colors.Gray)
                };
                button.PreviewMouseLeftButtonUp += InteractWithInventory;
                button.PreviewMouseRightButtonUp += DropItem;
                border.Child = canvas;
                canvas.Children.Add(button);
                InventoryGrid.Children.Add(border);
            }
        }

        private void DropItem(object sender, RoutedEventArgs e)
        {
            Player player = GameStatus.PLAYER;
            string slotString = ((System.Windows.Controls.Button)sender).Name;
            if (slotString.Equals("EmptySlot") || !slotString.Contains("_")) return;
            slotString = "_" + slotString.Split('_')[1];
            Item droppedItem = player.Inventory.Where(x => x.ID.Equals(slotString)).FirstOrDefault();
            player.Inventory.Remove(droppedItem);
            Tile playerTile = GameStatus.CURRENT_MAP.Tiles[player.X, player.Y];
            playerTile.Objects = playerTile.Objects ?? new List<GameObject>();
            playerTile.Objects.Add(droppedItem);
            GameLogic.PrintToGameLog("You have dropped " + droppedItem.Name);
            RefreshScreen();
        }

        private void InteractWithInventory(object sender, RoutedEventArgs e)
        {
            string slot = ((Button)sender).Name;
            if (string.IsNullOrEmpty(slot) || slot.Equals("EmptySlot")) return;
            Player player = GameStatus.PLAYER;
            string itemIDString = slot.Split(new string[] { "InventoryItem" }, StringSplitOptions.None)[1];
            if (string.IsNullOrEmpty(itemIDString)) return;
            Item itemInSlot = player.Inventory.Where(x => x.ID == itemIDString).SingleOrDefault();
            Item leftHandItem = player.Equipment[(int)Definitions.EnumBodyParts.LeftHand];
            Item rightHandItem = player.Equipment[(int)Definitions.EnumBodyParts.RightHand];
            if (itemInSlot != null)
            {
                switch (itemInSlot.GetType().Name.ToString())
                {
                    case "Weapon":
                        {
                            Weapon weapon = itemInSlot as Weapon;
                            player.Equipment[(int)Definitions.EnumBodyParts.RightHand] = weapon;
                            if (weapon.TwoHanded)
                                player.Equipment[(int)Definitions.EnumBodyParts.LeftHand] = null;
                            GameLogic.PrintToGameLog($"You have equipped {weapon.Name}!");
                            break;
                        }
                    case "Armor":
                        {
                            Armor armor = itemInSlot as Armor;
                            if (armor.HeldInHand) //if shield
                            {
                                player.Equipment[(int)Definitions.EnumBodyParts.LeftHand] = armor;
                                if (rightHandItem != null && (rightHandItem as Weapon).TwoHanded)
                                    player.Equipment[(int)Definitions.EnumBodyParts.RightHand] = null;
                                GameLogic.PrintToGameLog($"You have equipped {armor.Name}!");
                            }
                            else
                                player.EquipArmor(armor);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

        //Move the map in accordance with Player's movement.
        private void RefreshScreen()
        {
            DrawGameScreen();
            DrawInventoryScreen();
            DrawPlayerInfoScreen();
        }

        private void DrawPlayerInfoScreen()
        {
            Player player = GameStatus.PLAYER;
            PlayerName.Content = "Name: " + player.Name;
            PlayerHP.Content = "HP: " + player.HP + "\\" + player.MaxHP;
            PlayerLevel.Content = "Level: " + player.Level;
            PlayerEXP.Content = "EXP: " + player.Experience + "\\" + player.NeededExperience;
            PlayerScore.Content = "Score: " + "NONE!";
            CurrentFloor.Content = "Floor: " + GameStatus.MAPS.IndexOf(GameStatus.CURRENT_MAP) + 1;
        }

        private void DrawGameScreen()
        {
            int xpos = GameStatus.PLAYER.X - 5;
            int ypos = GameStatus.PLAYER.Y - 6;
            int z = 0;
            for (int y = Definitions.WINDOW_Y_SIZE - 1; y >= 0; y--)
            {
                for (int x = 0; x < Definitions.WINDOW_X_SIZE; x++)
                {
                    (XAMLMap.Children[z] as Image).ToolTip = new TextBlock() { Text = string.Empty };
                    bool tileExists = false;
                    if ((xpos + x) >= 0 && (ypos + y) >= 0 && (xpos + x) < GameStatus.CURRENT_MAP.XSize && (ypos + y) < GameStatus.CURRENT_MAP.YSize) //Make sure indices are in range of array
                    {
                        Tile tile = GameStatus.CURRENT_MAP.Tiles[xpos + x, ypos + y];
                        if (tile != null)
                        {
                            ImageSource overlayedImage = tile.Image;
                            if (tile.Objects != null && tile.Objects.Count > 0)
                                for (int i = 0; i < tile.Objects.Count; i++)
                                {
                                    overlayedImage = CreateTile.Overlay(overlayedImage, tile.Objects[i].GetImage());
                                    ((XAMLMap.Children[z] as Image).ToolTip as TextBlock).Text += GetObjectTooltip(tile.Objects[i], i > 0);
                                }
                            else (XAMLMap.Children[z] as Image).ToolTip = null;
                            (XAMLMap.Children[z++] as Image).Source = overlayedImage;
                            tileExists = true;
                        }
                    }
                    if (!tileExists)
                    {
                        (XAMLMap.Children[z] as Image).ToolTip = null;
                        (XAMLMap.Children[z++] as Image).Source = CreateBlackImage();
                    }
                }
            }
        }

        public string GetObjectTooltip(GameObject obj, bool isFirstTooltip)
        {
            string result = obj.Name + Environment.NewLine + obj.Description;
            if (isFirstTooltip) result = Environment.NewLine + Environment.NewLine + result;
            switch (obj.GetType().Name)
            {
                case "Monster":
                    {
                        return result + Environment.NewLine + "HP left: " + (obj as Monster).HP;
                    }
                case "Armor":
                    {
                        return result + Environment.NewLine + "Armor bonus: " + (obj as Armor).ArmorBonus;
                    }
                case "Weapon":
                    {
                        return result + Environment.NewLine + "Damage: " + (obj as Weapon).DamageRange;
                    }
            }
            return result;
        }

        private void DrawInventoryScreen()
        {
            List<Item> inventory = GameStatus.PLAYER.Inventory;
            for (int i = 0; i <= inventory.Count; i++)
            {
                Button btn = (((InventoryGrid.Children[i] as Border).Child as Canvas).Children[0] as Button);
                Image buttonImage = btn.Content as Image;
                if (i < inventory.Count)
                {
                    TextBlock tooltipInfo = new TextBlock();
                    buttonImage.Source = inventory[i].GetImage();
                    tooltipInfo.Text = $"{inventory[i].Name}\n{inventory[i].Description}";
                    btn.ToolTip = tooltipInfo;
                    btn.Name = $"InventoryItem{inventory[i].ID}";
                }
                else
                {
                    buttonImage.Source = null;
                    btn.ToolTip = null;
                    btn.Name = "EmptySlot";
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
        /// This is our main game loop. It handles input and makes the game proceed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            bool performMonsterLogic = true;
            if (GameStatus.GamePaused) return;
            Map map = GameStatus.CURRENT_MAP;
            string str = e.Key.ToString(); //get the string value of pressed key
            switch (str)
            {
                case "NumPad8":
                case "Up":
                    {
                        GameStatus.PLAYER.MoveTo(GameStatus.PLAYER.X, GameStatus.PLAYER.Y + 1, map);
                        break;
                    }
                case "NumPad2":
                case "Down":
                    {
                        GameStatus.PLAYER.MoveTo(GameStatus.PLAYER.X, GameStatus.PLAYER.Y - 1, map);
                        break;
                    }
                case "NumPad4":
                case "Left":
                    {
                        GameStatus.PLAYER.MoveTo(GameStatus.PLAYER.X - 1, GameStatus.PLAYER.Y, map);
                        break;
                    }
                case "NumPad6":
                case "Right":
                    {
                        GameStatus.PLAYER.MoveTo(GameStatus.PLAYER.X + 1, GameStatus.PLAYER.Y, map);
                        break;
                    }
                case "NumPad1":
                    {
                        GameStatus.PLAYER.MoveTo(GameStatus.PLAYER.X - 1, GameStatus.PLAYER.Y - 1, map);
                        break;
                    }
                case "NumPad3":
                    {
                        GameStatus.PLAYER.MoveTo(GameStatus.PLAYER.X + 1, GameStatus.PLAYER.Y - 1, map);
                        break;
                    }
                case "NumPad7":
                    {
                        GameStatus.PLAYER.MoveTo(GameStatus.PLAYER.X - 1, GameStatus.PLAYER.Y + 1, map);
                        break;
                    }
                case "NumPad9":
                    {
                        GameStatus.PLAYER.MoveTo(GameStatus.PLAYER.X + 1, GameStatus.PLAYER.Y + 1, map);
                        break;
                    }
                case "NumPad5":
                    {
                        break;
                    }
                case "G":
                    {
                        GameStatus.PLAYER.PickUp(map.Tiles[GameStatus.PLAYER.X, GameStatus.PLAYER.Y]);
                        break;
                    }
                case "OemComma": //','
                    {
                        performMonsterLogic = false;
                        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) //handle '<'
                            GoUpStairs();
                        break;
                    }
                case "OemPeriod": //'.'
                    {
                        performMonsterLogic = false;
                        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) //handle '>'
                            GoDownStairs();
                        break;
                    }
                case "OemQuestion": //'/'
                    {
                        performMonsterLogic = false;
                        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) //handle '?'
                            new Instructions();
                        break;
                    }
                default:
                    {
                        return;
                    }
            }
            if (performMonsterLogic)
            {
                LevelUpPlayer();
                MonsterLogic();
                RefreshScreen();
                if (GameStatus.PLAYER.HP <= 0) //if player has died
                {
                    Console.WriteLine("DEAD");
                }
            }
        }

        private void ResetMonsterLogic()
        {
            foreach (Monster monster in GameStatus.CURRENT_MAP.Monsters)
            {
                monster.AwareOfPlayer = false;
                monster.FollowingPlayer = false;
                monster.TurnsWithoutPlayerInSight = 0;
            }
        }

        private void GoDownStairs()
        {
            bool isNewMap = false;
            Map currentMap = GameStatus.CURRENT_MAP;
            if (GameStatus.CURRENT_MAP.Tiles[GameStatus.PLAYER.X, GameStatus.PLAYER.Y].LeadsDown == false)
            {
                GameLogic.PrintToGameLog("There are no stairs down here!");
                return;
            }
            if (GameStatus.MAPS.Count == GameStatus.MAPS.IndexOf(currentMap) + 1)
            {
                GameStatus.MAPS.Add(new Map(100));
                isNewMap = true;

            }
            ResetMonsterLogic();
            GameStatus.PLAYER.Z++;
            int currentMapIndex = GameStatus.MAPS.IndexOf(currentMap);
            currentMap = GameStatus.CURRENT_MAP = GameStatus.MAPS[++currentMapIndex];

            //check if we've already been to the floor we're going down to
            if (isNewMap)
            {
                Console.WriteLine("Making new map!");
                SetInitialPlayerLocation(currentMap);
                currentMap.GenerateStairsUp();
                Tile currentTile = currentMap.Tiles[GameStatus.PLAYER.X, GameStatus.PLAYER.Y];
                Console.WriteLine($"New tile location: {currentTile.X},{currentTile.Y},{currentTile.Z}");
                currentTile.Objects = new List<GameObject>() { GameStatus.PLAYER };
            }
            else //if an already existing floor
            {
                Console.WriteLine("Moving to existing floor!");
                GameStatus.PLAYER.X = GameStatus.STAIRS_UP_LOCATIONS[currentMap].X;
                GameStatus.PLAYER.Y = GameStatus.STAIRS_UP_LOCATIONS[currentMap].Y;
                Console.WriteLine($"New tile location: {GameStatus.STAIRS_DOWN_LOCATIONS[currentMap].X},{GameStatus.STAIRS_DOWN_LOCATIONS[currentMap].Y},{GameStatus.STAIRS_DOWN_LOCATIONS[currentMap].Z}");
            }
        }

        private void GoUpStairs()
        {
            Map currentMap = GameStatus.CURRENT_MAP;
            if (GameStatus.CURRENT_MAP.Tiles[GameStatus.PLAYER.X, GameStatus.PLAYER.Y].LeadsUp == false)
            {
                GameLogic.PrintToGameLog("There are no stairs up here!");
                return;
            }
            ResetMonsterLogic();
            Map mapMovedTo = GameStatus.MAPS[GameStatus.MAPS.IndexOf(currentMap) - 1];
            Player player = GameStatus.PLAYER;
            Tile stairsLocation = GameStatus.STAIRS_DOWN_LOCATIONS[mapMovedTo];
            player.X = stairsLocation.X;
            player.Y = stairsLocation.Y;
            player.Z--;
            GameStatus.CURRENT_MAP = mapMovedTo;
        }

        private void LevelUpPlayer()
        {
            if (GameStatus.PLAYER.Experience >= GameStatus.PLAYER.NeededExperience)
                GameStatus.PLAYER.LevelUp();
        }

        public void MonsterLogic()
        {
            Map map = GameStatus.CURRENT_MAP;
            List<Monster> deadMonsters = new List<Monster>();
            foreach (Monster monster in map.Monsters)
            {
                if (monster.HP <= 0) //if this monster died during the player's turn
                {
                    map.Tiles[monster.X, monster.Y].Objects.Remove(monster);
                    deadMonsters.Add(monster);
                    continue;
                }
                List<Tile> path = null;
                //Get the line representing the tiles between monster and Player
                List<Tile> line = Algorithms.Line(monster.X, GameStatus.PLAYER.X, monster.Y, GameStatus.PLAYER.Y, map, IsSeeThrough);
                monster.AwareOfPlayer = line == null ? false : true;
                if (monster.AwareOfPlayer)
                {
                    monster.FollowingPlayer = true;
                    monster.LastKnownPlayerLocationX = GameStatus.PLAYER.X;
                    monster.LastKnownPlayerLocationY = GameStatus.PLAYER.Y;
                    monster.TurnsWithoutPlayerInSight = 0;
                }
                else
                    monster.TurnsWithoutPlayerInSight++;
                if (monster.FollowingPlayer) //move monsters
                {
                    if (monster.AwareOfPlayer && monster.FollowingPlayer) path = Algorithms.FindPath(map, map.Tiles[monster.X, monster.Y], map.Tiles[GameStatus.PLAYER.X, GameStatus.PLAYER.Y], CostToEnterTile);
                    if (!monster.AwareOfPlayer && monster.FollowingPlayer) path = Algorithms.FindPath(map, map.Tiles[monster.X, monster.Y], map.Tiles[monster.LastKnownPlayerLocationX, monster.LastKnownPlayerLocationY], CostToEnterTile);
                    if (path != null)
                    {
                        if (path.Count > 1 || (path.Count == 1 && path.Last() == map.Tiles[monster.LastKnownPlayerLocationX, monster.LastKnownPlayerLocationY])) //not adjacent to player
                        {
                            int pathX = path.First().X, pathY = path.First().Y;
                            monster.MoveTo(pathX, pathY, map);
                        }
                        if (path.Count == 1 && path.Last() == map.Tiles[GameStatus.PLAYER.X, GameStatus.PLAYER.Y]) //adjacent to player
                        {
                            monster.Attack(GameStatus.PLAYER);
                        }
                    }
                    else BrownianMotion(monster);
                }
                else BrownianMotion(monster);
            }
            map.Monsters.RemoveAll(x => deadMonsters.Contains(x));
        }

        private double CostToEnterTile(Tile tile)
        {
            return (tile.Objects != null && tile.Objects.OfType<Monster>().Any()) ? 5 : 0;
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
