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
        private const int MAP_DIMENSIONS = 100;
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
            int mapSize = _xSize * YSize; //The total number of possible tiles for this map.
            int sumOfTiles = 0;
            _rooms = new List<Room>();
            int attemptedRoomPlacements = 0;
            while (attemptedRoomPlacements++ < MAX_PLACEMENT_ATTEMPTS &&
                   sumOfTiles <= mapSize / 5) //don't fill the map with too many rooms
            {
                Room room = new Room();
                room.TopLeft = new MapCoord(_rand.Next(room.XSize, _xSize) - room.XSize, _rand.Next(room.YSize, _ySize) - room.YSize);
                if (room.TopLeft != null && IsRoomInBoundsOfMap(room))
                {
                    sumOfTiles += room.YSize * room.XSize;
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
            int baseX = room.BottomLeft.X;
            int baseY = room.BottomLeft.Y;
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
            int baseX = room.BottomLeft.X;
            int baseY = room.BottomLeft.Y;
            //Create floors in enclosing space
            for (int x = 1; x < room.XSize; x++)
                for (int y = 1; y < room.YSize; y++)
                    if (_tiles[baseX + x, baseY + y] == null || _tiles[baseX + x, baseY + y].Walkable == false)
                        _tiles[baseX + x, baseY + y] = new Tile(Floor);
        }

        ///// <summary>
        ///// Create corridors between all existing rooms on this map.
        ///// </summary>
        //public void CreateCorridors()
        //{
        //    List<Room> roomsWithNoExits = Rooms.ToList();
        //    ConnectedRooms = new List<Room>();
        //    //While there are rooms with no exits.
        //    while (roomsWithNoExits.Count() > 0)
        //    {
        //        Room firstRoom = roomsWithNoExits.FirstOrDefault(); //get a room with no exit
        //        Room secondRoom = null;
        //        if (ConnectedRooms.Count > 0) secondRoom = ConnectedRooms[_rand.Next(ConnectedRooms.Count)];
        //        else secondRoom = Rooms.Where(x => x != firstRoom).FirstOrDefault();
        //        if (secondRoom == null) return;
        //        MapCoord firstRoomCorridorOrigin = new MapCoord(_rand.Next(firstRoom.TopLeft.X+1, firstRoom.TopRight.X), _rand.Next(firstRoom.BottomLeft.Y+1, firstRoom.TopLeft.Y));
        //        MapCoord secondRoomCorridorOrigin = new MapCoord(_rand.Next(secondRoom.TopLeft.X+1, secondRoom.TopRight.X), _rand.Next(secondRoom.BottomLeft.Y+1, secondRoom.TopLeft.Y));
        //        CreateCorridor(firstRoomCorridorOrigin, secondRoomCorridorOrigin);
        //        firstRoom.NumOfExits++;
        //        secondRoom.NumOfExits++;
        //        ConnectedRooms.Add(firstRoom);
        //        if (!ConnectedRooms.Contains(secondRoom)) ConnectedRooms.Add(secondRoom);
        //        roomsWithNoExits.Remove(firstRoom);
        //        roomsWithNoExits.Remove(secondRoom);
        //    }
        //}

        /// <summary>
        /// Create corridors in a way that no room remains unconnected to a different room.
        /// </summary>
        public void CreateCorridors()
        {
            List<Room> RoomsWithNoExit = Rooms.ToList();
            while (RoomsWithNoExit.Count > 0)
            {
                Room chosenRoom = RoomsWithNoExit[_rand.Next(RoomsWithNoExit.Count)];
                chosenRoom.BasicConnectedRoom = FindNearestDisconnectedRoom(chosenRoom);
                if (chosenRoom.BasicConnectedRoom == null)
                {
                    List<Room> tempRooms = _rooms.Where(x => x!=chosenRoom && x.NumOfExits!=0).ToList();
                    chosenRoom.BasicConnectedRoom = tempRooms[_rand.Next(tempRooms.Count())];
                }
                else
                    Console.WriteLine($"Source = {chosenRoom.ToString()} ||| Target = {chosenRoom.BasicConnectedRoom.ToString()}");
                chosenRoom.BasicConnectedRoom.BasicConnectedRoom = chosenRoom;
                RoomsWithNoExit.Remove(chosenRoom);
                RoomsWithNoExit.Remove(chosenRoom.BasicConnectedRoom);
            }
            foreach (Room room in _rooms)
            {
                MapCoord firstRoomCorridorOrigin = new MapCoord(_rand.Next(room.XSize-1) + room.TopLeft.X+1, _rand.Next(room.YSize-1) + room.BottomLeft.Y+1);
                Room sRoom = room.BasicConnectedRoom;
                MapCoord secondRoomCorridorOrigin = new MapCoord(_rand.Next(sRoom.XSize-1) + sRoom.TopLeft.X+1, _rand.Next(sRoom.YSize-1) + sRoom.BottomLeft.Y+1);
                CreateCorridor(firstRoomCorridorOrigin,secondRoomCorridorOrigin);
                room.NumOfExits++;
                sRoom.NumOfExits++;
            }
        }

        /// <summary>
        /// Finds the nearest room to this one that has ConnectedRoom==null by checking around this one in evenly-spaced areas.
        /// </summary>
        /// <param name="room">The room to find the closest room to.</param>
        /// <returns>The closest room.</returns>
        public Room FindNearestDisconnectedRoom(Room room)
        {
            //Five is the minimum room size, so that is our multiplier.
            MapCoord startPoint = null;
            MapCoord iterator = null;
            Room resultRoom = null;
            int multiplier = 5;
            for (int i = 1; i <= MAP_DIMENSIONS/2 && i*multiplier<=MAP_DIMENSIONS; i++)
            {
                multiplier = i * 5;
                startPoint = new MapCoord(room.TopLeft.X - multiplier, room.TopLeft.Y + multiplier);
                resultRoom = FindRoomForThisPoint(startPoint);
                if (resultRoom != null && resultRoom.BasicConnectedRoom == null)
                    return resultRoom; 
                iterator = new MapCoord(room.TopLeft.X - multiplier + 5, room.TopLeft.Y + multiplier);
                int j = 0;
                while (iterator.X>0 && iterator.X<XSize && iterator.Y>0 && iterator.Y < YSize)
                {
                    resultRoom = FindRoomByCoordinate(iterator);
                    if (resultRoom != null && resultRoom.BasicConnectedRoom == null && resultRoom!=room && !DoRoomsContainEachOther(room.TopLeft,iterator))
                        return resultRoom;
                    if (iterator.X == startPoint.X)
                        if (iterator.Y == startPoint.Y) break;
                        else iterator.Y = iterator.Y + 5 > startPoint.Y ? startPoint.Y : iterator.Y + 5;
                    if (iterator.Y == room.BottomLeft.Y - multiplier)
                        iterator.X = iterator.X - 5 < startPoint.X ? startPoint.X : iterator.X - 5;
                    if (iterator.X == room.TopRight.X + multiplier) iterator.Y = iterator.Y - 5 < room.BottomRight.Y - multiplier ? room.BottomRight.Y - multiplier : iterator.Y - 5;
                    if (iterator.Y == startPoint.Y && iterator.X!=startPoint.X) iterator.X = iterator.X + 5 > room.TopRight.X + multiplier ? room.TopRight.X + multiplier : iterator.X + 5;
                    j++;
                }
            }
            List<Room> tempRooms = _rooms.Where(x => x != room).ToList();
            return tempRooms[_rand.Next(tempRooms.Count())];
        }

        /// <summary>
        /// Creates a corridor between two coordinates unless they are both located in the same room (room in a room).
        /// </summary>
        /// <param name="firstOrigin">The random spot in the first room you want to create a corridor to.</param>
        /// <param name="secondOrigin">The random spot in the second room you want to create a corridor to.</param>
        public void CreateCorridor(MapCoord firstOrigin, MapCoord secondOrigin)
        {
            //if (DoRoomsContainEachOther(firstOrigin, secondOrigin)) return;
            int rightOrLeft = firstOrigin.X > secondOrigin.X ? -1 : 1; //if first origin is to the right of second origin
            int upOrDown = firstOrigin.Y > secondOrigin.Y ? -1 : 1; //if first origin is above second origin
            for (int createdTiles = 0; firstOrigin.X + (createdTiles * rightOrLeft) != secondOrigin.X + rightOrLeft; createdTiles++)
            {
                _tiles[firstOrigin.X + (createdTiles * rightOrLeft), firstOrigin.Y] = new Tile(Floor);
                if (_tiles[firstOrigin.X + (createdTiles * rightOrLeft), firstOrigin.Y + 1] == null)
                    _tiles[firstOrigin.X + (createdTiles * rightOrLeft), firstOrigin.Y + 1] = new Tile(Wall);
                if (_tiles[firstOrigin.X + (createdTiles * rightOrLeft), firstOrigin.Y - 1] == null)
                    _tiles[firstOrigin.X + (createdTiles * rightOrLeft), firstOrigin.Y - 1] = new Tile(Wall);
            }
            if (_tiles[secondOrigin.X + rightOrLeft, firstOrigin.Y - upOrDown] == null)
                _tiles[secondOrigin.X + rightOrLeft, firstOrigin.Y - upOrDown] = new Tile(Wall);
            for (int createdTiles = 0; firstOrigin.Y + (createdTiles * upOrDown) != secondOrigin.Y + upOrDown; createdTiles++)
            {
                _tiles[secondOrigin.X, firstOrigin.Y + (createdTiles * upOrDown)] = new Tile(Floor);
                if (_tiles[secondOrigin.X + 1, firstOrigin.Y + (createdTiles * upOrDown)] == null)
                    _tiles[secondOrigin.X + 1, firstOrigin.Y + (createdTiles * upOrDown)] = new Tile(Wall);
                if (_tiles[secondOrigin.X - 1, firstOrigin.Y + (createdTiles * upOrDown)] == null)
                    _tiles[secondOrigin.X - 1, firstOrigin.Y + (createdTiles * upOrDown)] = new Tile(Wall);
            }
        }

        /// <summary>
        /// Check if two points on the maps are contained in rooms, and if those rooms contain each other.
        /// </summary>
        /// <param name="firstOrigin"></param>
        /// <param name="secondOrigin"></param>
        /// <returns></returns>
        public bool DoRoomsContainEachOther(MapCoord firstOrigin, MapCoord secondOrigin)
        {
            Room firstRoom = FindRoomByCoordinate(firstOrigin);
            Room secondRoom = FindRoomByCoordinate(secondOrigin);
            if (firstRoom.TopLeft.X <= secondRoom.TopLeft.X && firstRoom.TopRight.X >= secondRoom.TopRight.X)
                if (firstRoom.TopLeft.Y >= secondRoom.TopLeft.Y && firstRoom.BottomLeft.Y <= secondRoom.TopLeft.Y)
                    return true;
            if (secondRoom.TopLeft.X <= firstRoom.TopLeft.X && secondRoom.TopRight.X >= firstRoom.TopRight.X)
                if (secondRoom.TopLeft.Y >= firstRoom.TopLeft.Y && secondRoom.BottomLeft.Y <= firstRoom.TopLeft.Y)
                    return true;
            return false;
        }

        /// <summary>
        /// Find a room by a given coordinate. If there are multiple, return the smallest one.
        /// </summary>
        /// <param name="coord">The coordinate whose room you want to find.</param>
        /// <returns>A Room object where the coordinate is.</returns>
        public Room FindRoomByCoordinate(MapCoord coord)
        {
            IEnumerable<Room> matchingRooms = new List<Room>();
            matchingRooms = _rooms.Where(x => x.TopLeft.X <= coord.X);
            matchingRooms = matchingRooms.Where(x => x.TopRight.X >= coord.X);
            matchingRooms = matchingRooms.Where(x => x.BottomLeft.Y <= coord.Y);
            matchingRooms = matchingRooms.Where(x => x.TopLeft.Y >= coord.Y).ToList();
            //Now that we have a list of rooms this coord matches, find the smallest possible room.
            //Room size = X size * Y size, so look for the smallest one or return the first result if mutliple exist.
            return matchingRooms/*.Where(x => (x.YSize * x.XSize) == matchingRooms.Select(y => y.XSize * y.YSize).Min())*/.FirstOrDefault();
        }

        /// <summary>
        /// Overload for FindRoomByCoordinate<int,int>, ignores the 3rd coordinate.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public Room FindRoomByCoordinate(FullCoord coord)
        {
            return FindRoomByCoordinate(new MapCoord(coord.X, coord.Y));
        }

        public bool IsRoomInBoundsOfMap(Room room)
        {
            if (room.TopLeft.X < 1 || room.TopRight.X >= _xSize) return false;
            if (room.BottomLeft.Y < 1 || room.TopLeft.Y >= _ySize) return false;
            return true;
        }

        /// <summary>
        /// Checks if 'point' is in 'room', returns true or false
        /// </summary>
        /// <param name="point"></param>
        /// <param name="room"></param>
        /// <returns></returns>
        public bool IsPointInRoom(MapCoord point, Room room)
        {
            if (point.X >= room.TopLeft.X && point.X <= room.TopRight.X)
                if (point.Y >= room.BottomLeft.Y && point.Y <= room.TopLeft.Y)
                    return true;
            return false;
        }

        /// <summary>
        /// Check if a certain point is in any room that exists on this map.
        /// </summary>
        /// <param name="point">The point whose room we want to find.</param>
        /// <returns>The room, if one exists, or null, if it does not.</returns>
        public Room FindRoomForThisPoint(MapCoord point)
        {
            IEnumerable<Room> resultRooms = Rooms.Where(x => x.TopLeft.X <= point.X);
            resultRooms = resultRooms.Where(x => x.TopRight.X >= point.X);
            resultRooms = resultRooms.Where(x => x.TopLeft.Y >= point.Y);
            resultRooms = resultRooms.Where(x => x.BottomLeft.Y <= point.Y);
            return resultRooms.FirstOrDefault();
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
                        if (_tiles[x, y].Objects != null) s = "@";
                    }
                    Console.Write(s);
                }
                Console.WriteLine();
            }
        }
        #endregion
    }
}
