using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1;

namespace TheUndergroundTower.Pathfinding
{
    public class Room
    {
        public const int minRoomSize = 4;
        public const int maxRoomSize = 9;

        private int _xSize, _ySize;
        private int _topLeftX, _topLeftY;
        private List<Tile> _walls;

        private static Random _rand;

        public int XSize { get => _xSize; set { if (value >= 0) { _xSize = value; } } }
        public int YSize { get => _ySize; set { if (value >= 0) { _ySize = value; } } }

        public int TopLeftX { get => _topLeftX; set { if (value >= 0) { _topLeftX = value; } } }
        public int TopLeftY { get => _topLeftY; set { if (value >= 0) { _topLeftY = value; } } }

        public int TopRightX { get => _topLeftX + _xSize; }
        public int TopRightY { get => _topLeftY; }
        public int BottomLeftX { get => _topLeftX; }
        public int BottomLeftY { get => _topLeftY - _ySize; }
        public int BottomRightX { get => _topLeftX + _xSize; }
        public int BottomRightY { get => _topLeftY - _ySize; }

        public List<Tile> Walls { get => _walls; set => _walls = value; }

        public Room(Map map, int? topLeftX = null, int? topLeftY = null, int? xSize = null, int? ySize = null)
        {
            _walls = new List<Tile>();

            _rand = _rand ?? new Random(DateTime.Now.Millisecond);
            _xSize = xSize != null ? (int)xSize : _rand.Next(minRoomSize, maxRoomSize);
            _ySize = ySize != null ? (int)ySize : _rand.Next(minRoomSize, maxRoomSize);
            TopLeftX = topLeftX == null ? map.XSize / 2 : (int)topLeftX;
            TopLeftY = topLeftY == null ? map.YSize / 2 : (int)topLeftY;
            GenerateWalls(map);
            GenerateFloors(map);
            map.Rooms.Add(this);
        }

        private void GenerateWalls(Map map)
        {
            int x = 0, y = 0;

            try
            {
                for (x = 0; x < _xSize; x++)
                {
                    if (map.Tiles[TopLeftX + x, TopLeftY] == null) _walls.Add(new Tile(map.WallTile) { X = TopLeftX + x, Y = TopLeftY });
                    if (map.Tiles[TopLeftX + x, BottomLeftY] == null) _walls.Add(new Tile(map.WallTile) { X = TopLeftX + x, Y = BottomLeftY });
                }
                for (y = 0; y < _ySize; y++)
                {
                    if (map.Tiles[TopLeftX, BottomLeftY + y] == null) _walls.Add(new Tile(map.WallTile) { X = TopLeftX, Y = BottomLeftY + y });
                    if (map.Tiles[TopRightX, BottomLeftY + y] == null) _walls.Add(new Tile(map.WallTile) { X = TopRightX, Y = BottomLeftY + y });
                }
                _walls.Add(new Tile(map.WallTile) { X = TopRightX, Y = TopRightY });
                foreach (Tile wall in _walls)
                    map.Tiles[wall.X, wall.Y] = wall;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void GenerateFloors(Map map)
        {
            try
            {
                for (int x = 1; x < _xSize; x++)
                    for (int y = 1; y < _ySize; y++)
                        map.Tiles[BottomLeftX + x, BottomLeftY + y] = new Tile(map.FloorTile) { X = BottomLeftX + x, Y = BottomLeftY + y };
            }
            catch (Exception ex)
            {
                Console.WriteLine("OOPS");
            }
        }

        public bool IsTileARoomCorner(Tile tile)
        {
            if (tile.X == TopLeftX && tile.Y == TopLeftY) return true;
            if (tile.X == TopRightX && tile.Y == TopRightY) return true;
            if (tile.X == BottomLeftX && tile.Y == BottomLeftY) return true;
            if (tile.X == BottomRightX && tile.Y == BottomRightY) return true;
            return false;
        }

        public override string ToString()
        {
            string str = $"TopLeftX: {_topLeftX} , TopLeftY: {_topLeftY}  ||  TopRightX: {TopRightX} , TopRightY: {TopRightY}\n";
            str += $"BottomLeftX: {BottomLeftX} , BottomLeftY: {BottomLeftY}  ||  BottomRightX: {BottomRightX} , BottomRightY: {BottomRightY}";
            return str;
        }

    }
}
