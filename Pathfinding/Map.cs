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
        private const int MAX_CORRIDOR_LENGTH = 6;
        private const int MIN_CORRIDOR_LENGTH = 3;

        private static int _mapCounter=0;
        private static Random _rand;

        private int _xSize, _ySize;
        private Tile[,] _tiles;
        private Tile _wallTile,_floorTile;
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

        public void GenerateRooms()
        {
            Room currentRoom = new Room(this);
            int roomCounter = 1;
            bool isTopOrBottomWall;
            int incDec;
            while (roomCounter <= ATTEMPTS_BEFORE_NO_MORE_ROOMS)
            {
                Tile startPoint = currentRoom.Walls.Random(_rand);
                isTopOrBottomWall = currentRoom.TopLeftY == startPoint.Y || currentRoom.BottomLeftY == startPoint.Y;
                if (isTopOrBottomWall) incDec = currentRoom.TopLeftY == startPoint.Y ? 1 : -1;
                else incDec = currentRoom.TopLeftX == startPoint.X ? -1 : 1;
                roomCounter++;
            }
        }

        public bool InBoundsOfMap(int x, int y)
        {
            return !(x >= XSize || y >= YSize || x < 0 || y < 0);
        }
    }
}
