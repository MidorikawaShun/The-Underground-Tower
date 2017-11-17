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

        /// <summary>
        /// The coordinate of this room's top-left tile on the map it belongs to.
        /// </summary>
        private Tuple<int, int> _topLeft;

        /// <summary>
        /// Object for random number generation.
        /// </summary>
        private static Random _rand;

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

        public Tuple<int, int> TopLeft
        {
            get => _topLeft;
            set
            {
                int numOfAttempts = 0;
                while ((value.Item1-XSize<0 || value.Item2-YSize<0) && numOfAttempts<10)
                {
                    DetermineRoomDimensions();
                    numOfAttempts++;
                }
                if (numOfAttempts == 10)
                    return;
                _topLeft = value;
            }
        }
        public Tuple<int, int> TopRight
        {
            get
            {
                if (_topLeft == null)
                    return null;
                return new Tuple<int, int>(_topLeft.Item1 + _xSize, _topLeft.Item2);
            }
        }
        public Tuple<int, int> BottomLeft
        {
            get
            {
                if (_topLeft == null)
                    return null;
                return new Tuple<int, int>(_topLeft.Item1, _topLeft.Item2 - _ySize);
            }
        }
        public Tuple<int, int> BottomRight
        {
            get
            {
                if (_topLeft == null)
                    return null;
                return new Tuple<int, int>(_topLeft.Item1 + _xSize, _topLeft.Item2 - _ySize);
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

        public void DetermineRoomDimensions()
        {
            _rand = _rand ?? new Random(DateTime.Now.Millisecond);
            _xSize = _rand.Next(MIN_ROOM_SIZE, MAX_ROOM_X_SIZE);
            _ySize = _rand.Next(MIN_ROOM_SIZE, MAX_ROOM_Y_SIZE);
        }
    }
}
