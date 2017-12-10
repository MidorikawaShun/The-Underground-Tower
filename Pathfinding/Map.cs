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
        private const int MAX_CORRIDOR_LENGTH = 6;
        private const int MIN_CORRIDOR_LENGTH = 4;
        private const double CHANCE_TO_CREATE_CORRIDOR = 0.7;
        private const double CHANCE_TO_CREATE_ROOM = 0.3;
        private const int MAX_CORRIDOR_TWISTS = 5;

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
            //for (int i = 0; i < _xSize; i++)
            //    for (int j = 0; j < _ySize; j++)
            //    {
            //        _tiles[i, j] = new Tile(GameData.POSSIBLE_TILES[(int)CreateTile.Tiles.OrdinaryFloor]);
            //    }
        }

        //public void GenerateRooms()
        //{
        //    Room currentRoom = new Room(this);
        //    _rooms.Add(currentRoom);
        //    Console.WriteLine(currentRoom);
        //    int corridorAttemptCounter = 1;
        //    int roomAttemptCounter = 1;
        //    bool isTopOrBottomWall;
        //    int incDec;
        //    Tile startPoint = currentRoom.Walls.Random(_rand);
        //    while (roomAttemptCounter <= ATTEMPTS_BEFORE_NO_MORE_ROOMS)
        //    {
        //        double corridorCounter = 0;
        //        while (corridorAttemptCounter <= ATTEMPTS_BEFORE_STOP_MAKING_CORRIDORS)
        //        {
        //            while (currentRoom.IsTileARoomCorner(startPoint)) startPoint = currentRoom.Walls.Random(_rand);
        //            Console.WriteLine(startPoint);
        //            isTopOrBottomWall = currentRoom.TopLeftY == startPoint.Y || currentRoom.BottomLeftY == startPoint.Y;
        //            if (isTopOrBottomWall) incDec = currentRoom.TopLeftY == startPoint.Y ? 1 : -1;
        //            else incDec = currentRoom.TopLeftX == startPoint.X ? -1 : 1;
        //            List<Tile> newTiles;
        //            if (corridorCounter / MAX_CORRIDOR_TWISTS < (_rand.NextDouble() + 0.01)) //at least one corridor
        //            {
        //                if (TryToCreateCorridor(startPoint, isTopOrBottomWall, incDec, out newTiles))
        //                {
        //                    corridorCounter++;
        //                    currentRoom.Walls.Remove(startPoint);
        //                    startPoint = newTiles.Random(_rand);
        //                }
        //                else corridorAttemptCounter++;
        //            }
        //            else
        //            {
        //                int attemptedRoomPlacements = 0;
        //                while (attemptedRoomPlacements++ < ATTEMPTS_BEFORE_NO_MORE_ROOMS)
        //                {
        //                    if ()
        //                }
        //                corridorAttemptCounter = ATTEMPTS_BEFORE_STOP_MAKING_CORRIDORS+1;
        //            }
        //        }
        //        //corridorAttemptCounter = 0;
        //        roomAttemptCounter++;
        //    }
        //}

        public void GenerateRooms()
        {
            Room currentRoom = new Room(this);
            _rooms.Add(currentRoom);
            Console.WriteLine(currentRoom);
            int corridorAttemptCounter = 0, corridorCounter = 0, roomAttemptCounter = 1, incDec = 0;
            Tile startPoint = currentRoom.Walls.Random(_rand);
            Tile previousPoint = startPoint;
            bool isStartPointARoom = true,nextBuildIsCorridor=true, isTopOrBottomWall=true;
            List<Tile> newTiles = null;
            while (roomAttemptCounter <= ATTEMPTS_BEFORE_NO_MORE_ROOMS)
            {
                PrepareVariablesForBuilding(currentRoom, ref startPoint, ref previousPoint, isStartPointARoom, ref isTopOrBottomWall, ref incDec);
                if (nextBuildIsCorridor && corridorAttemptCounter < ATTEMPTS_BEFORE_STOP_MAKING_CORRIDORS)
                {
                    if (TryToCreateCorridor(startPoint, isTopOrBottomWall, incDec, out newTiles))
                    {
                        DrawMapToConsole();
                        corridorCounter++;
                        nextBuildIsCorridor = (corridorCounter / MAX_CORRIDOR_TWISTS) < (_rand.NextDouble() - 0.01);
                        previousPoint = startPoint;
                        startPoint = newTiles.Random(_rand);
                    }
                    else corridorAttemptCounter++;
                }
                else
                {
                    //TODO: Add room building
                    roomAttemptCounter++;
                }
            } 
        }

        //This method shortens GenerateRooms to enhance readability
        public void PrepareVariablesForBuilding(Room currentRoom, ref Tile startPoint, ref Tile previousPoint, bool isStartPointARoom,ref bool isTopOrBottomWall,ref int incDec)
        {
            while (currentRoom.IsTileARoomCorner(startPoint)) startPoint = previousPoint = currentRoom.Walls.Random(_rand);
            if (isStartPointARoom)
            {
                isTopOrBottomWall = currentRoom.TopLeftY == startPoint.Y || currentRoom.BottomLeftY == startPoint.Y;
                if (isTopOrBottomWall) incDec = currentRoom.TopLeftY == startPoint.Y ? 1 : -1;
                else incDec = currentRoom.TopLeftX == startPoint.X ? -1 : 1;
            }
            else
            {
                isTopOrBottomWall = previousPoint.Y + 1 == startPoint.Y || previousPoint.Y - 1 == startPoint.Y;
                if (isTopOrBottomWall) incDec = startPoint.X > previousPoint.X ? 1 : -1;
                else incDec = startPoint.Y > previousPoint.Y ? 1 : -1;
            }
        }

        private bool TryToCreateRoom(Tile startPoint, bool isTopOrBottomWall, int incDec)
        {
            throw new NotImplementedException();
        }

        private bool TryToCreateCorridor(Tile startPoint, bool topOrBottom, int incDec, out List<Tile> newTiles)
        {
            int corridorLength = _rand.Next(MIN_CORRIDOR_LENGTH, MAX_CORRIDOR_LENGTH);
            newTiles = null;
            //check that we have enough room to place this corridor in
            for (int checkedLength = 1; checkedLength < corridorLength; checkedLength++)
            {
                if (topOrBottom)
                {
                    if (InBoundsOfMap(startPoint.X + 1, startPoint.Y + checkedLength * incDec) && InBoundsOfMap(startPoint.X - 1, startPoint.Y + checkedLength * incDec))
                        if (_tiles[startPoint.X + 1, startPoint.Y + checkedLength * incDec] == null && _tiles[startPoint.X - 1, startPoint.Y + checkedLength * incDec] == null)
                            continue;
                }
                else
                {
                    if (InBoundsOfMap(startPoint.X + checkedLength * incDec, startPoint.Y + 1) && InBoundsOfMap(startPoint.X + checkedLength * incDec, startPoint.Y - 1))
                        if (_tiles[startPoint.X + checkedLength * incDec, startPoint.Y + 1] == null && _tiles[startPoint.X + checkedLength * incDec, startPoint.Y - 1] == null)
                            continue;
                }
                return false;
            }
            newTiles = CreateCorridor(startPoint, topOrBottom, incDec, corridorLength);
            return true;
        }

        private List<Tile> CreateCorridor(Tile startPoint, bool topOrBottom, int incDec, int corridorLength)
        {
            List<Tile> newWalls = new List<Tile>();
            _tiles[startPoint.X, startPoint.Y] = new Tile(_floorTile);
            for (int i = 1; i < corridorLength; i++)
            {
                if (topOrBottom)
                {
                    _tiles[startPoint.X + 1, startPoint.Y + i * incDec] = new Tile(_wallTile) { X = startPoint.X + 1, Y = startPoint.Y + i * incDec };
                    _tiles[startPoint.X - 1, startPoint.Y + i * incDec] = new Tile(_wallTile) { X = startPoint.X - 1, Y = startPoint.Y + i * incDec };
                    if (i + 1 < corridorLength)
                    {
                        _tiles[startPoint.X, startPoint.Y + i * incDec] = new Tile(_floorTile) { X = startPoint.X, Y = startPoint.Y + i * incDec };
                        newWalls.Add(_tiles[startPoint.X + 1, startPoint.Y + i * incDec]);
                        newWalls.Add(_tiles[startPoint.X - 1, startPoint.Y + i * incDec]);
                    }
                    else _tiles[startPoint.X, startPoint.Y + i * incDec] = new Tile(_wallTile);
                }
                else
                {
                    _tiles[startPoint.X + i * incDec, startPoint.Y + 1] = new Tile(_wallTile) { X = startPoint.X + i * incDec, Y = startPoint.Y + 1 };
                    _tiles[startPoint.X + i * incDec, startPoint.Y - 1] = new Tile(_wallTile) { X = startPoint.X + i * incDec, Y = startPoint.Y - 1 };
                    if (i + 1 < corridorLength)
                    {
                        _tiles[startPoint.X + i * incDec, startPoint.Y] = new Tile(_floorTile) { X = startPoint.X + i * incDec, Y = startPoint.Y };
                        newWalls.Add(_tiles[startPoint.X + i * incDec, startPoint.Y + 1]);
                        newWalls.Add(_tiles[startPoint.X + i * incDec, startPoint.Y - 1]);
                    }
                    else _tiles[startPoint.X + i * incDec, startPoint.Y] = new Tile(_wallTile);
                }
            }
            return newWalls.Skip(2).ToList();
        }

        public bool InBoundsOfMap(int x, int y)
        {
            return !(x >= XSize || y >= YSize || x < 0 || y < 0);
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
    }
}
