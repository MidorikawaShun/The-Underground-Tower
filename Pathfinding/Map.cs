using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheUndergroundTower.Creatures;
using TheUndergroundTower.OtherClasses;
using WpfApp1;
using WpfApp1.Creatures;

namespace TheUndergroundTower.Pathfinding
{
    /// <summary>
    /// A class that represents a level in the dungeon.
    /// </summary>
    public class Map
    {

        private const int ATTEMPTS_BEFORE_NO_MORE_ROOMS = 10;
        private const int ATTEMPTS_BEFORE_STOP_MAKING_CORRIDORS = 5;
        private const int MAX_CORRIDOR_LENGTH = 10;
        private const int MIN_CORRIDOR_LENGTH = 7;
        private const int MAX_MONSTERS_PER_ROOM = 2;
        private const int MIN_MONSTERS_PER_ROOM = 0;
        private const int MAX_MONSTER_PLACEMENT_ATTEMPTS = 10;
        private const int MIN_ITEMS_IN_ROOM = 1;
        private const int MAX_ITEMS_IN_ROOM = 5;
        private const double CHANCE_TO_CREATE_CORRIDOR = 0.7;
        private const double CHANCE_TO_CREATE_ROOM = 0.3;
        private const double MAX_CORRIDOR_TWISTS = 5;

        private static Random _rand;

        private int _xSize, _ySize;
        public int mapNum;
        private Tile[,] _tiles;
        private Tile _wallTile, _floorTile, _stairsUpTile, _stairsDownTile;
        private List<Room> _rooms;
        private List<Monster> _monsters;
        private List<Item> _items;

        public int XSize { get => _xSize; set => _xSize = value; }
        public int YSize { get => _ySize; set => _ySize = value; }
        public Tile[,] Tiles { get => _tiles; set => _tiles = value; }
        public Tile WallTile { get => _wallTile; set => _wallTile = value; }
        public Tile FloorTile { get => _floorTile; set => _floorTile = value; }
        public Tile StairsUpTile { get => _stairsUpTile; set => _stairsUpTile = value; }
        public Tile StairsDownTile { get => _stairsDownTile; set => _stairsDownTile = value; }
        public List<Room> Rooms { get => _rooms; set => _rooms = value; }
        public List<Monster> Monsters { get => _monsters; set => _monsters = value; }
        public List<Item> Items { get => _items; set => _items = value; }

        //Constructor
        public Map(int size)
        {
            //dont create the map 
            if (GameStatus.ChosenDepth.MaximumFloors <= mapNum) return;
            size = (int)(GameStatus.ChosenDepth.FloorSizeMultiplier * size);
            mapNum = GameStatus.Maps.Count() + 1;
            _rand = new Random(DateTime.Now.Millisecond);
            _wallTile = GameData.POSSIBLE_TILES[(int)CreateTile.Tiles.OrdinaryWall];
            _floorTile = GameData.POSSIBLE_TILES[(int)CreateTile.Tiles.OrdinaryFloor];
            _stairsDownTile = GameData.POSSIBLE_TILES[(int)CreateTile.Tiles.OrdinaryStairsDown];
            _stairsUpTile = GameData.POSSIBLE_TILES[(int)CreateTile.Tiles.OrdinaryStairsUp];
            _rooms = new List<Room>();
            _tiles = new Tile[size, size];
            _xSize = _ySize = size;
            GenerateRooms();
            GenerateStairsDown();
            Monsters = new List<Monster>();
            Items = new List<Item>();
            GenerateMonsters();
            GenerateItems();
            CreateAmulet(); //creates end-game condition if chosen depth allows
        }

        private void CreateAmulet()
        {
            //don't do this if not on the last floor
            if (GameStatus.ChosenDepth.MaximumFloors != mapNum) return;
            Room amuletRoom = Rooms.Random(_rand);
            Item amulet = new Item(GameData.POSSIBLE_ITEMS.Where(x=>x.IsGameEnderItem).FirstOrDefault());
            int xCenter = (int)((amuletRoom.BottomLeftX + amuletRoom.BottomRightX) / 2);
            int yCenter = (int)((amuletRoom.BottomLeftY + amuletRoom.TopRightY) / 2);
            if (_tiles[xCenter, yCenter].Objects != null)
                _tiles[xCenter, yCenter].Objects.Add(amulet);
            else
                _tiles[xCenter, yCenter].Objects = new List<GameObject>() { amulet };
        }

        //create stairs to next floor
        private void GenerateStairsDown()
        {
            try
            {
                //dont make another floor if reached floor limit
                if (GameStatus.ChosenDepth.MaximumFloors <= mapNum) return;
                Room roomToPutStairsIn = _rooms.Random(_rand);
                int stairsX = _rand.Next(roomToPutStairsIn.TopLeftX + 1, roomToPutStairsIn.XSize + roomToPutStairsIn.TopLeftX);
                int stairsY = _rand.Next(roomToPutStairsIn.BottomLeftY + 1, roomToPutStairsIn.YSize + roomToPutStairsIn.BottomLeftY);
                Tile stairsDownTile = new Tile(_stairsDownTile) { LeadsDown = true, X = stairsX, Y = stairsY };
                stairsDownTile.Image = CreateTile.Overlay(_tiles[stairsX, stairsY].Image, stairsDownTile.Image);
                _tiles[stairsX, stairsY] = stairsDownTile;
                GameStatus.StairsDownLocations.Add(this, stairsDownTile); //will be used to navigate up floors
            }
            catch (Exception ex)
            {
                Console.WriteLine("OOPS");
            }
        }

        public void GenerateStairsUp()
        {
            Tile stairsUpTile = new Tile(_stairsUpTile) { LeadsUp = true, X = GameStatus.Player.X, Y = GameStatus.Player.Y };
            stairsUpTile.Image = CreateTile.Overlay(_tiles[GameStatus.Player.X, GameStatus.Player.Y].Image, stairsUpTile.Image);
            _tiles[GameStatus.Player.X, GameStatus.Player.Y] = stairsUpTile;
            GameStatus.StairsUpLocations.Add(this, stairsUpTile);
        }

        private void GenerateRooms()
        {
            try
            {
                Room currentRoom = new Room(this);
                Tile startPoint = currentRoom.Walls.Random(_rand), prevPoint = startPoint;
                List<Tile> currentWallTiles = null;
                int roomAttempts = 0, incDec = 0, corridorTwists = 0, corridorAttempts = 0;
                bool isStartPointARoom = true, isTopOrBottomWall = true;
                while (roomAttempts < ATTEMPTS_BEFORE_NO_MORE_ROOMS)
                {
                    //InitializeVariables(ref incDec, ref isTopOrBottom, prevPoint, isStartPointARoom, currentRoom, startPoint);
                    PrepareVariablesForBuilding(currentRoom, ref startPoint, ref prevPoint, isStartPointARoom, ref isTopOrBottomWall, ref incDec);
                    //if (corridorAttempts == 5) roomAttempts++;
                    corridorAttempts = 0;
                    if (isStartPointARoom || ((corridorTwists / MAX_CORRIDOR_TWISTS) + (_rand.NextDouble() / 2 + 0.00001) < CHANCE_TO_CREATE_CORRIDOR))
                    {
                        while (corridorAttempts < ATTEMPTS_BEFORE_STOP_MAKING_CORRIDORS)
                        {
                            PrepareVariablesForBuilding(currentRoom, ref startPoint, ref prevPoint, isStartPointARoom, ref isTopOrBottomWall, ref incDec);
                            if (TryToCreateCorridor(startPoint, incDec, isTopOrBottomWall, out currentWallTiles))
                            {
                                corridorAttempts = ATTEMPTS_BEFORE_STOP_MAKING_CORRIDORS;
                                corridorTwists++;
                                prevPoint = startPoint;
                                startPoint = currentWallTiles.Random(_rand);
                                isStartPointARoom = false;
                            }
                            else
                            {
                                if (currentWallTiles != null) startPoint = currentWallTiles.Random(_rand);
                                else
                                {
                                    currentRoom = _rooms.Random(_rand);
                                    startPoint = currentRoom.Walls.Random(_rand);
                                }
                                corridorAttempts++;
                            }
                        }
                    }
                    else
                    {
                        if (TryToCreateRoom(startPoint, incDec, isTopOrBottomWall, ref currentRoom))
                        {
                            roomAttempts = 0;
                            corridorTwists = 0;
                            prevPoint = startPoint;
                            startPoint = _rooms.Last().Walls.Random(_rand);
                            isStartPointARoom = true;
                        }
                        else
                        {
                            roomAttempts++;
                            if (currentWallTiles != null) startPoint = currentWallTiles.Random(_rand);
                            else
                            {
                                currentRoom = _rooms.Random(_rand);
                                startPoint = currentRoom.Walls.Random(_rand);
                            }
                        }
                    }
                }
                DrawMapToConsole();
            }
            catch (Exception ex)
            {
                Console.WriteLine("HELLO");
            }
        }

        private bool TryToCreateRoom(Tile startPoint, int incDec, bool isTopOrBottomWall, ref Room currentRoom)
        {
            try
            {
                int roomXSize = _rand.Next(Room.minRoomSize, Room.maxRoomSize), roomYSize = _rand.Next(Room.minRoomSize, Room.maxRoomSize);
                int topLeftX = startPoint.X, topLeftY = startPoint.Y;
                if (isTopOrBottomWall)
                {
                    if (incDec == 1) topLeftY += roomYSize;
                    //else
                    //roomYSize--;
                    topLeftX--;
                }
                else
                {
                    if (incDec == 1) { topLeftX++; roomXSize--; }
                    else topLeftX -= roomXSize;
                    topLeftY++;
                }
                if (topLeftY - roomYSize < 0) roomYSize = topLeftY;
                for (int y = topLeftY; y > topLeftY - roomYSize; y--)
                    for (int x = 0; x <= roomXSize; x++)
                        if (!ViableTile(x + topLeftX, y)) return false;
                currentRoom = new Room(this, topLeftX, topLeftY, roomXSize, roomYSize);
                _tiles[startPoint.X, startPoint.Y] = new Tile(_floorTile) { X = startPoint.X, Y = startPoint.Y };
                if (isTopOrBottomWall) _tiles[startPoint.X, startPoint.Y + incDec] = new Tile(_floorTile) { X = startPoint.X, Y = startPoint.Y + incDec };
                else _tiles[startPoint.X + incDec, startPoint.Y] = new Tile(_floorTile) { X = startPoint.X + incDec, Y = startPoint.Y };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when attempting to create a room!");
            }
            return true;
        }

        private void PrepareVariablesForBuilding(Room currentRoom, ref Tile startPoint, ref Tile previousPoint, bool isStartPointARoom, ref bool isTopOrBottomWall, ref int incDec)
        {
            bool loop = true;
            while (loop)
            {
                while (currentRoom.IsTileARoomCorner(startPoint)) startPoint = previousPoint = currentRoom.Walls.Random(_rand);
                if (isStartPointARoom)
                {
                    isTopOrBottomWall = startPoint.Y == currentRoom.TopLeftY || startPoint.Y == currentRoom.BottomLeftY;
                    incDec = (startPoint.Y == currentRoom.TopLeftY || startPoint.X == currentRoom.TopRightX) ? 1 : -1;
                }
                else
                {
                    //isTopOrBottomWall = startPoint.X + 1 < previousPoint.X || startPoint.X - 1 > previousPoint.X;
                    if (startPoint.X != previousPoint.X && startPoint.Y != previousPoint.Y)
                        isTopOrBottomWall = isTopOrBottomWall = startPoint.X + 1 < previousPoint.X || startPoint.X - 1 > previousPoint.X;
                    else isTopOrBottomWall = startPoint.X == previousPoint.X;
                    incDec = (isTopOrBottomWall && previousPoint.Y < startPoint.Y) || (!isTopOrBottomWall && previousPoint.X < startPoint.X) ? 1 : -1;
                }
                if (startPoint.X != 0 && startPoint.Y != 0) loop = false;
                else startPoint = currentRoom.Walls.Random(_rand);
            }
        }

        //private void InitializeVariables(ref int incDec, ref bool isTopOrBottom, Tile startPoint, bool isStartPointARoom, Room currentRoom = null, Tile newStartPoint = null)
        //{
        //    if (isStartPointARoom)
        //    {
        //        isTopOrBottom = startPoint.Y == currentRoom.TopLeftY || startPoint.Y == currentRoom.BottomLeftY;
        //        incDec = (startPoint.Y == currentRoom.TopLeftY || startPoint.X == currentRoom.TopRightX) ? 1 : -1;
        //    }
        //    else
        //    {
        //        isTopOrBottom = startPoint.Y + 1 < newStartPoint.Y || startPoint.Y - 1 > newStartPoint.Y;
        //        incDec = (isTopOrBottom && startPoint.Y < newStartPoint.Y) || (!isTopOrBottom && startPoint.X < newStartPoint.X) ? 1 : -1;
        //    }
        //}

        private bool TryToCreateCorridor(Tile startPoint, int incDec, bool isTopOrBottom, out List<Tile> currentWallTiles)
        {
            currentWallTiles = null;
            int startX = startPoint.X, startY = startPoint.Y;
            if (isTopOrBottom) startY += incDec;
            else startX += incDec;
            int corridorLength = _rand.Next(MIN_CORRIDOR_LENGTH, MAX_CORRIDOR_LENGTH);
            for (int i = 0; i < corridorLength; i++)
                if (isTopOrBottom)
                {
                    int y = startY + i * incDec;
                    if (!ViableTile(startX - 1, y) || !ViableTile(startX, y) || !ViableTile(startX + 1, y)) return false;
                }
                else
                {
                    int x = startX + i * incDec;
                    if (!ViableTile(x, startY - 1) || !ViableTile(x, startY) || !ViableTile(x, startY + 1)) return false;
                }
            currentWallTiles = CreateCorridor(startPoint, incDec, isTopOrBottom, corridorLength);
            return true;
        }

        private List<Tile> CreateCorridor(Tile startPoint, int incDec, bool isTopOrBottom, int corridorLength)
        {
            List<Tile> newWalls = new List<Tile>();
            Tile firstWall = null, centerTile = null, secondWall = null;
            int startX = startPoint.X, startY = startPoint.Y;
            if (isTopOrBottom) startY += incDec;
            else startX += incDec;
            _tiles[startPoint.X, startPoint.Y] = new Tile(_floorTile) { X = startPoint.X, Y = startPoint.Y };

            for (int i = 0; i < corridorLength; i++)
            {
                if (isTopOrBottom)
                {
                    int y = startY + i * incDec;
                    _tiles[startX - 1, y] = firstWall = new Tile(_wallTile) { X = startX - 1, Y = y };
                    _tiles[startX, y] = centerTile = new Tile(i == corridorLength - 1 ? _wallTile : _floorTile) { X = startX, Y = y };
                    _tiles[startX + 1, y] = secondWall = new Tile(_wallTile) { X = startX + 1, Y = y };
                }
                else
                {
                    int x = startX + i * incDec;
                    //if (!ViableTile(x, startY - 1) || !ViableTile(x, startY) || !ViableTile(x, startY + 1)) return false;
                    _tiles[x, startY - 1] = firstWall = new Tile(_wallTile) { X = x, Y = startY - 1 };
                    _tiles[x, startY] = centerTile = new Tile(i == corridorLength - 1 ? _wallTile : _floorTile) { X = x, Y = startY };
                    _tiles[x, startY + 1] = secondWall = new Tile(_wallTile) { X = x, Y = startY + 1 };
                }
                if (i == corridorLength - 2) newWalls.AddRange(new List<Tile>() { firstWall, secondWall });
                if (i == corridorLength - 1) newWalls.Add(centerTile);
            }
            return newWalls;
        }

        public bool InBoundsOfMap(int x, int y)
        {
            return !(x >= XSize || y >= YSize || x < 0 || y < 0);
        }

        public bool ViableTile(int x, int y)
        {
            return InBoundsOfMap(x, y) && _tiles[x, y] == null;
        }

        public bool TileExists(int x, int y)
        {
            return InBoundsOfMap(x, y) && _tiles[x, y] != null;
        }

        public void DrawMapToConsole()
        {
            for (int y = YSize - 1; y >= 0; y--)
            {
                for (int x = 0; x < XSize; x++)
                {
                    string s = " ";
                    if (_tiles[x, y] != null)
                        if (_tiles[x, y].Walkable == false) s = "X";
                        else s = "O";
                    Console.Write(s);
                }
                Console.WriteLine();
            }
        }

        public List<Tile> AcquireNeighbours(Tile source)
        {
            List<Tile> neighbours = new List<Tile>();
            int x = source.X, y = source.Y;
            if (TileExists(x + 1, y + 1)) neighbours.Add(_tiles[x + 1, y + 1]);
            if (TileExists(x + 1, y)) neighbours.Add(_tiles[x + 1, y]);
            if (TileExists(x + 1, y - 1)) neighbours.Add(_tiles[x + 1, y - 1]);
            if (TileExists(x, y + 1)) neighbours.Add(_tiles[x, y + 1]);
            if (TileExists(x, y - 1)) neighbours.Add(_tiles[x, y - 1]);
            if (TileExists(x - 1, y + 1)) neighbours.Add(_tiles[x - 1, y + 1]);
            if (TileExists(x - 1, y)) neighbours.Add(_tiles[x - 1, y]);
            if (TileExists(x - 1, y - 1)) neighbours.Add(_tiles[x - 1, y - 1]);
            if (neighbours.Contains(null))
            {
                Console.WriteLine("OOPS");
            }
            return neighbours;
        }

        private void GenerateMonsters()
        {
            List<Monster> monsters = GameData.POSSIBLE_MONSTERS;
            Map currentMap = this;

            foreach (Room room in currentMap.Rooms)
            {
                int monstersInThisRoom = _rand.Next(MIN_MONSTERS_PER_ROOM, MAX_MONSTERS_PER_ROOM + 1);
                for (int attempts = 0, monstersPlaced = 0; monstersPlaced < monstersInThisRoom && attempts < MAX_MONSTER_PLACEMENT_ATTEMPTS; attempts++)
                {
                    int x = _rand.Next(room.TopLeftX + 1, room.TopRightX), y = _rand.Next(room.BottomLeftY + 1, room.TopLeftY);
                    Tile targetTile = currentMap.Tiles[x, y];
                    if (targetTile.Objects == null) targetTile.Objects = new List<GameObject>();
                    if (targetTile.Objects.OfType<Creature>().Count() > 0) continue;
                    Monster monster = (new Monster(monsters.Random(_rand)) { X = targetTile.X, Y = targetTile.Y, Z = GameStatus.Maps.IndexOf(currentMap) });
                    targetTile.Objects.Add(monster);
                    Monsters.Add(monster);
                    monstersPlaced++;
                }
            }

        }

        private void GenerateItems()
        {
            List<Item> items = GameData.POSSIBLE_ITEMS.Where(x=>x.IsGameEnderItem==false).ToList();
            Map currentMap = this;
            foreach (Room room in currentMap.Rooms)
            {
                int numOfItemsInThisRoom = _rand.Next(MIN_ITEMS_IN_ROOM, MAX_ITEMS_IN_ROOM + 1);
                for (; numOfItemsInThisRoom > 0; numOfItemsInThisRoom--)
                {
                    int x = _rand.Next(room.TopLeftX + 1, room.TopRightX), y = _rand.Next(room.BottomLeftY + 1, room.TopLeftY);
                    Tile targetTile = currentMap.Tiles[x, y];
                    if (targetTile.Objects == null) targetTile.Objects = new List<GameObject>();
                    var item = Item.Create(items.Random(_rand));
                    item.X = targetTile.X; item.Y = targetTile.Y; item.Z = GameStatus.Maps.IndexOf(currentMap);
                    targetTile.Objects.Add(item);
                    Items.Add(item);
                }
            }
        }

    }
}
