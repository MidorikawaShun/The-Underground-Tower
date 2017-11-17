﻿using System;
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

        /// <summary>
        /// Used to generate random numbers.
        /// </summary>
        private static Random _rand;

        private const int MAX_PLACEMENT_ATTEMPTS = 5;

        /// <summary>
        /// All the tiles that are contained in this map (Floors, walls).
        /// </summary>
        private Tile[,] _tiles;

        /// <summary>
        /// The generic tiles used for this map.
        /// </summary>
        private Tile Floor, Wall, Door;

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
            Floor = GameData.POSSIBLE_TILES[(int)CreateTile.Tiles.OrdinaryFloor];
            Wall = GameData.POSSIBLE_TILES[(int)CreateTile.Tiles.OrdinaryWall];
            _xSize = _ySize = MAP_DIMENSIONS * (int)MAP_SIZE_MULTIPLIER;
            _tiles = new Tile[_xSize, _ySize];
            _rand = _rand ?? new Random(DateTime.Now.Millisecond);
            Rooms = new List<Room>();
            int attemptedRoomPlacements = 0;
            while (attemptedRoomPlacements++ < MAX_PLACEMENT_ATTEMPTS &&
                   Rooms.Count < 5)
            {
                Room room = new Room();
                room.TopLeft = new Tuple<int, int>(_rand.Next(room.XSize, _xSize) - room.XSize, _rand.Next(room.YSize, _ySize) - room.YSize);
                if (room.TopLeft != null && room.TopLeft.Item1 >= 0 && room.TopLeft.Item2 >= 0) //If room is in bounds of the map
                {
                    attemptedRoomPlacements = 0;
                    CreateRoom(room);
                }
            }
            CreateCorridors();
            DrawMapToConsole();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Creates the room tiles and adds it to the list of rooms on this map.
        /// </summary>
        /// <param name="room">The room to generate.</param>
        private void CreateRoom(Room room)
        {
            CreateWalls(room);
            CreateFloors(room);
            Rooms.Add(room);
        }

        /// <summary>
        /// Creates the walls of the room according to its size, but only if they don't overwrite existing floors.
        /// </summary>
        /// <param name="room">The Room object we want to create walls for.</param>
        private void CreateWalls(Room room)
        {
            int baseX = room.BottomLeft.Item1;
            int baseY = room.BottomLeft.Item2;
            for (int i = 0; i < room.XSize; i++) //Create horizontal walls
            {
                if (_tiles[baseX + i, baseY] == null)
                    _tiles[baseX + i, baseY] = new Tile(Wall);
                if (_tiles[baseX + i, baseY + room.YSize] == null)
                    _tiles[baseX + i, baseY + room.YSize] = new Tile(Wall);
            }
            for (int i = 0; i < room.YSize; i++) //Create vertical walls
            {
                if (_tiles[baseX, baseY + i] == null)
                    _tiles[baseX, baseY + i] = new Tile(Wall);
                if (_tiles[baseX + room.XSize, baseY + i] == null)
                    _tiles[baseX + room.XSize, baseY + i] = new Tile(Wall);
            }
            _tiles[baseX + room.XSize, baseY + room.YSize] = new Tile(Wall);
        }

        /// <summary>
        /// Creates the floors for a certain room according to its size.
        /// </summary>
        /// <param name="room">The Room object we want to create floors for.</param>
        private void CreateFloors(Room room)
        {
            int baseX = room.BottomLeft.Item1;
            int baseY = room.BottomLeft.Item2;
            //Create floors in enclosing space
            for (int x = 1; x < room.XSize; x++)
                for (int y = 1; y < room.YSize; y++)
                    if (_tiles[baseX + x, baseY + y] == null || _tiles[baseX + x, baseY + y].Walkable == false)
                        _tiles[baseX + x, baseY + y] = new Tile(Floor);
        }

        /// <summary>
        /// Create corridors between all existing rooms on this map.
        /// </summary>
        public void CreateCorridors()
        {
            List<Room> roomsWithNoExits = Rooms;
            //While there are rooms with no exits.
            while (roomsWithNoExits.Count() > 0)
            {
                Room firstRoom = roomsWithNoExits.FirstOrDefault(); //get a room with no exit
                Room secondRoom = Rooms.Where(x => x != firstRoom).FirstOrDefault(); //get a room that is not firstRoom
                if (secondRoom == null) return;
                Tuple<int, int> firstRoomCorridorOrigin = new Tuple<int, int>(_rand.Next(firstRoom.TopLeft.Item1, firstRoom.TopRight.Item1), _rand.Next(firstRoom.BottomLeft.Item2, firstRoom.TopLeft.Item2));
                Tuple<int, int> secondRoomCorridorOrigin = new Tuple<int, int>(_rand.Next(secondRoom.TopLeft.Item1, secondRoom.TopRight.Item1), _rand.Next(secondRoom.BottomLeft.Item2, secondRoom.TopLeft.Item2));
                CreateCorridor(firstRoomCorridorOrigin, secondRoomCorridorOrigin);
                roomsWithNoExits.Remove(firstRoom);
            }
        }

        /// <summary>
        /// Creates a corridor between two coordinates unless they are both located in the same room (room in a room).
        /// </summary>
        /// <param name="firstOrigin">The random spot in the first room you want to create a corridor to.</param>
        /// <param name="secondOrigin">The random spot in the second room you want to create a corridor to.</param>
        public void CreateCorridor(Tuple<int, int> firstOrigin, Tuple<int, int> secondOrigin)
        {
            //Check if rooms are contained within each other.
            if (CheckIfRoomsAreContained(firstOrigin, secondOrigin))
            {

            }
        }

        public bool CheckIfRoomsAreContained(Tuple<int, int> firstOrigin, Tuple<int, int> secondOrigin)
        {
            Room firstRoom = FindRoomByCoordinate(firstOrigin);
            Room secondRoom = FindRoomByCoordinate(secondOrigin);
            if (firstRoom.TopLeft.Item1 <= secondRoom.TopLeft.Item1 && secondRoom.TopRight.Item1 <= firstRoom.TopRight.Item1)
                if (firstRoom.BottomLeft.Item2 <= secondRoom.BottomLeft.Item2 && secondRoom.TopLeft.Item2 <= firstRoom.TopLeft.Item2)
                    return false;
            if (secondRoom.TopLeft.Item1 <= firstRoom.TopLeft.Item1 && firstRoom.TopRight.Item1 <= secondRoom.TopRight.Item1)
                if (secondRoom.BottomLeft.Item2 <= firstRoom.BottomLeft.Item2 && firstRoom.TopLeft.Item2 <= secondRoom.TopLeft.Item2)
                    return false;
            return true;
        }

        /// <summary>
        /// Find a room by a given coordinate. If there are multiple, return the smallest one.
        /// </summary>
        /// <param name="coord">The coordinate whose room you want to find.</param>
        /// <returns>A Room object where the coordinate is.</returns>
        public Room FindRoomByCoordinate(Tuple<int, int> coord)
        {
            List<Room> matchingRooms = new List<Room>();
            foreach (Room checkedRoom in Rooms)
                //check if targets X-coordinate is inside the room.
                if (coord.Item1 >= checkedRoom.TopLeft.Item1 && coord.Item1 <= checkedRoom.TopRight.Item1)
                    //check if targets Y-coordinate is inside the room.
                    if (coord.Item2 >= checkedRoom.BottomLeft.Item2 && coord.Item2 <= checkedRoom.TopLeft.Item2)
                        matchingRooms.Add(checkedRoom);
            //Now that we have a list of rooms this coord matches, find the smallest possible room.
            //Room size = X size * Y size, so look for the smallest one or return the first result if mutliple exist.
            return matchingRooms.Where(x => (x.YSize * x.XSize) == matchingRooms.Select(y => y.XSize * y.YSize).Min()).FirstOrDefault();
        }

        /// <summary>
        /// Overload for FindRoomByCoordinate<int,int>, ignores the 3rd coordinate.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public Room FindRoomByCoordinate(Tuple<int, int, int> coord)
        {
            return FindRoomByCoordinate(new Tuple<int, int>(coord.Item1, coord.Item2));
        }

        public void DrawMapToConsole()
        {
            for (int y = YSize - 1; y >= 0; y--)
            {
                for (int x = 0; x < XSize; x++)
                {
                    string s = " ";
                    if (_tiles[x, y] != null)
                    {
                        if (_tiles[x, y].Walkable == false) s = "X";
                        if (_tiles[x, y].Walkable == true) s = "O";
                        if (_tiles[x, y].Objects != null) s = "P";
                    }
                    Console.Write(s);
                }
                Console.WriteLine();
            }
        }
        #endregion
    }
}
