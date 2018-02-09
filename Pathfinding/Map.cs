using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheUndergroundTower.Creatures;
using TheUndergroundTower.OtherClasses;
using WpfApp1;

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
        private const double CHANCE_TO_CREATE_CORRIDOR = 0.7;
        private const double CHANCE_TO_CREATE_ROOM = 0.3;
        private const double MAX_CORRIDOR_TWISTS = 5;

        private static int _mapCounter = 0;
        private static Random _rand;

        private int _xSize, _ySize;
        private Tile[,] _tiles;
        private Tile _wallTile, _floorTile;
        private List<Room> _rooms;

        public int XSize { get => _xSize; set => _xSize = value; }
        public int YSize { get => _ySize; set => _ySize = value; }
        public Tile[,] Tiles { get => _tiles; set => _tiles = value; }
        public Tile WallTile { get => _wallTile; set => _wallTile = value; }
        public Tile FloorTile { get => _floorTile; set => _floorTile = value; }
        public List<Room> Rooms { get => _rooms; set => _rooms = value; }

        //Constructor
        public Map(int size)
        {
            _rand = new Random(DateTime.Now.Millisecond);
            _wallTile = GameData.POSSIBLE_TILES[(int)CreateTile.Tiles.OrdinaryWall];
            _floorTile = GameData.POSSIBLE_TILES[(int)CreateTile.Tiles.OrdinaryFloor];
            _rooms = new List<Room>();
            _mapCounter++;
            _tiles = new Tile[size, size];
            _xSize = _ySize = size;
            GenerateRooms();
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

    }
}
