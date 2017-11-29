using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheUndergroundTower.Pathfinding
{
    /// <summary>
    /// Represents a room that is contained inside of a map.
    /// </summary>
    public class Room
    {
        #region Members
        /// <summary>
        /// The dimensions of the room (width, height).
        /// </summary>
        private int _xSize, _ySize;

        public int RoomNumber =0;

        private static int RoomNumberer = 0;

        /// <summary>
        /// The coordinate of this room's top-left tile on the map it belongs to.
        /// </summary>
        private MapCoord _topLeft;

        /// <summary>
        /// Object for random number generation.
        /// </summary>
        private static Random _rand;

        /// <summary>
        /// The room that the corridor-generating algorithm pairs this room with.
        /// </summary>
        private Room _basicConnectedRoom;

        private int _numOfExits;

        /// <summary>
        /// The max width of any given room in tiles.
        /// </summary>
        private const int MAX_ROOM_X_SIZE = 9;
        /// <summary>
        /// The max height of any given room in tiles.
        /// </summary>
        private const int MAX_ROOM_Y_SIZE = 9;
        /// <summary>
        /// The minimum size any room dimension can be.
        /// </summary>
        private const int MIN_ROOM_SIZE = 5;
        #endregion

        #region Properties
        public int XSize { get => _xSize; private set => _xSize = value; }
        public int YSize { get => _ySize; private set => _ySize = value; }
        public int NumOfExits { get => _numOfExits; set => _numOfExits = value; }
        public Room BasicConnectedRoom { get => _basicConnectedRoom; set => _basicConnectedRoom = value; }

        public MapCoord TopLeft
        {
            get => _topLeft;
            set
            {
                int numOfAttempts = 0;
                while ((value.X-XSize<0 || value.Y-YSize<0) && numOfAttempts<10)
                {
                    DetermineRoomDimensions();
                    numOfAttempts++;
                }
                if (numOfAttempts == 10)
                    return;
                _topLeft = value;
            }
        }
        public MapCoord TopRight
        {
            get
            {
                if (_topLeft == null)
                    return null;
                return new MapCoord(_topLeft.X + _xSize, _topLeft.Y);
            }
        }
        public MapCoord BottomLeft
        {
            get
            {
                if (_topLeft == null)
                    return null;
                return new MapCoord(_topLeft.X, _topLeft.Y - _ySize);
            }
        }
        public MapCoord BottomRight
        {
            get
            {
                if (_topLeft == null)
                    return null;
                return new MapCoord(_topLeft.X + _xSize, _topLeft.Y - _ySize);
            }
        }
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public Room()
        {
            DetermineRoomDimensions();
        }

        private void DetermineRoomDimensions()
        {
            _rand = _rand ?? new Random(DateTime.Now.Millisecond);
            _xSize = _rand.Next(MIN_ROOM_SIZE, MAX_ROOM_X_SIZE);
            _ySize = _rand.Next(MIN_ROOM_SIZE, MAX_ROOM_Y_SIZE);
            RoomNumber = RoomNumberer++;
        }

        public override string ToString()
        {
            return $"TopLeft.X:{TopLeft.X} , TopLeft.Y:{TopLeft.Y}";
        }

        public MapCoord GetPointInRoom()
        {
            int xPos = _rand.Next(TopLeft.X + 1, TopRight.X - 1); //don't include walls
            int yPos = _rand.Next(BottomLeft.Y + 1, TopLeft.Y - 1);
            return new MapCoord(xPos, yPos);
        }
    }
}
