using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1;

namespace TheUndergroundTower.Pathfinding
{
    /// <summary>
    /// A class that represents a level in the dungeon.
    /// </summary>
    public class Map
    {
        #region Members
        /// <summary>
        /// The dimensions of the map (width, height).
        /// </summary>
        private int _xSize, _ySize;

        private const int MAX_PLACEMENT_ATTEMPTS = 5;

        /// <summary>
        /// All the tiles that are contained in this map (Floors, walls).
        /// </summary>
        private Tile[,] _tiles;

        /// <summary>
        /// The rooms that are present on this map.
        /// </summary>
        private List<Room> _rooms;

        private static double MAP_SIZE_MULTIPLIER = 1;
        private const int MAP_DIMENSIONS = 40;
        #endregion
        #region Properties
        public int XSize { get => _xSize; set => _xSize = value; }
        public int YSize { get => _ySize; set => _ySize = value; }
        public Tile[,] Tiles { get => _tiles; set => _tiles = value; }
        public List<Room> Rooms { get => _rooms; set => _rooms = value; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates the map and the rooms inside of it.
        /// </summary>
        public Map()
        {
            _xSize = _ySize = MAP_DIMENSIONS * (int)MAP_SIZE_MULTIPLIER;
            _tiles = new Tile[_xSize, _ySize];
            Random rand = new Random(DateTime.Now.Millisecond);
            Rooms = new List<Room>();
            int attemptedRoomPlacements = 0;
            while (attemptedRoomPlacements++ < MAX_PLACEMENT_ATTEMPTS &&
                   Rooms.Count < 5)
            {
                Room room = new Room();
                room.TopLeft = new Tuple<int, int>(rand.Next(room.XSize,_xSize) - room.XSize, rand.Next(room.YSize,_ySize) - room.YSize);
                if (room.TopLeft!=null && room.TopLeft.Item1 >= 0 && room.TopLeft.Item2 >= 0) //If room is in bounds of the map
                {
                    CreateWalls(room);
                    CreateFloors(room);
                    attemptedRoomPlacements = 0;
                    Rooms.Add(room);
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates the walls of the room according to it's size, but only if they don't overwrite existing floors.
        /// </summary>
        /// <param name="room">The Room object we want to create tiles for.</param>
        private void CreateWalls(Room room)
        {
            int baseX = room.BottomLeft.Item1;
            int baseY = room.BottomLeft.Item2;
            for (int i = 0; i < room.XSize; i++) //Create horizontal walls
            {
                if (_tiles[baseX + i, baseY] == null)
                    _tiles[baseX + i, baseY] = new Tile(GameData.POSSIBLE_TILES[(int)CreateTile.Tiles.OrdinaryWall]);
                if (_tiles[baseX + i, baseY + room.YSize] == null)
                    _tiles[baseX + i, baseY + room.YSize] = new Tile(GameData.POSSIBLE_TILES[(int)CreateTile.Tiles.OrdinaryWall]);
            }
            for (int i = 1; i < room.YSize - 1; i++) //Create vertical walls
            {
                if (_tiles[baseX, baseY + i] == null)
                    _tiles[baseX, baseY + i] = new Tile(GameData.POSSIBLE_TILES[(int)CreateTile.Tiles.OrdinaryWall]);
                if (_tiles[baseX + room.XSize, baseY + i] == null)
                    _tiles[baseX + room.XSize, baseY + i] = new Tile(GameData.POSSIBLE_TILES[(int)CreateTile.Tiles.OrdinaryWall]);
            }
        }

        private void CreateFloors(Room room)
        {
            int baseX = room.BottomLeft.Item1;
            int baseY = room.BottomLeft.Item2;
            for (int x = 1; x < room.XSize - 1; x++)
                for (int y = 1; y < room.YSize - 1; y++)
                    if (_tiles[baseX + x, baseY + y] == null || _tiles[baseX + x, baseY + y].Walkable == false)
                        _tiles[baseX + x, baseY + y] = new Tile(GameData.POSSIBLE_TILES[(int)CreateTile.Tiles.OrdinaryFloor]);
        }
        #endregion
    }
}
