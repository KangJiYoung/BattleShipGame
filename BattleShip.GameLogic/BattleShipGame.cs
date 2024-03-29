using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BattleShip.GameLogic
{
    public class BattleShipGame
    {
        private const int COORDINATES_LENGTH = 2;
        
        public Grid Grid { get; }
        public IRandomGenerator RandomGenerator { get; }
        public IList<IShip> Ships { get; }

        public BattleShipGame(Grid grid, IRandomGenerator randomGenerator)
        {
            Grid = grid;
            RandomGenerator = randomGenerator;

            Ships = new List<IShip>();
        }

        public void GenerateRandomShipPositions(IShip ship)
        {
            if (ship == null)
                throw new ArgumentNullException(nameof(ship));

            bool result;
            do
            {
                Point startingPosition;
                Point endingPosition;

                var isHorizontal = RandomGenerator.GetRandomBoolean();
                if (isHorizontal)
                    GenerateHorizontalPositions(ship, out startingPosition, out endingPosition);
                else
                    GenerateVerticalPositions(ship, out startingPosition, out endingPosition);

                ship.SetPositions(startingPosition, endingPosition);
                result = Grid.AddShip(ship);
            } while (!result);
        }

        private void GenerateHorizontalPositions(IShip ship, out Point startingPosition, out Point endingPosition)
        {
            startingPosition = new Point
            {
                X = RandomGenerator.GetRandomNumber(0, Grid.Rows),
                Y = RandomGenerator.GetRandomNumber(0, Grid.Columns - ship.Size)
            };
            endingPosition = new Point
            {
                X = startingPosition.X,
                Y = startingPosition.Y + ship.Size - 1
            };
        }

        private void GenerateVerticalPositions(IShip ship, out Point startingPosition, out Point endingPosition)
        {
            startingPosition = new Point
            {
                X = RandomGenerator.GetRandomNumber(0, Grid.Rows - ship.Size),
                Y = RandomGenerator.GetRandomNumber(0, Grid.Columns)
            };
            endingPosition = new Point
            {
                X = startingPosition.X + ship.Size - 1,
                Y = startingPosition.Y
            };
        }

        public void NewGame()
        {
            foreach (var ship in Ships)
                GenerateRandomShipPositions(ship);
        }

        public bool Hit(string coordinates)
        {
            if (string.IsNullOrWhiteSpace(coordinates))
                throw new ArgumentNullException(nameof(coordinates));

            if (coordinates.Length != COORDINATES_LENGTH)
                throw new ArgumentException("Coordinates must have a length of 2 characters", nameof(coordinates));

            var x = CoordinateParser.Parse(coordinates.First());
            var parseResult = int.TryParse(coordinates.Last().ToString(), out var y);

            return parseResult && Grid.Hit(x, y);
        }

        public void AddShip(IShip ship)
        {
            if (ship == null)
                throw new ArgumentNullException(nameof(ship));

            Ships.Add(ship);
        }
    }
}