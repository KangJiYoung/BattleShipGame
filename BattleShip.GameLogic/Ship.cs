using System;
using System.ComponentModel;
using System.Drawing;

namespace BattleShip.GameLogic
{
    public class Ship : IShip
    {
        public int Size { get; }
        public ShipType ShipType { get; }
        public Point StartingPosition { get; private set; }
        public Point EndingPosition { get; private set; }

        public Ship(int size, ShipType shipType)
        {
            if (size <= 0) 
                throw new ArgumentOutOfRangeException(nameof(size));
            if (!Enum.IsDefined(typeof(ShipType), shipType))
                throw new InvalidEnumArgumentException(nameof(shipType), (int) shipType, typeof(ShipType));

            Size = size;
            ShipType = shipType;
        }

        public void SetPositions(Point startingPosition, Point endingPosition)
        {
            StartingPosition = startingPosition;
            EndingPosition = endingPosition;
        }
    }
}