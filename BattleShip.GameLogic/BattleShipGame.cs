using System.Collections.Generic;
using System.Drawing;

namespace BattleShip.GameLogic
{
    public class BattleShipGame
    {
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
            int result;
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
            } while (result == -1);
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

        public void NewGame()
        {
            foreach (var ship in Ships)
                GenerateRandomShipPositions(ship);
        }

        public bool Hit(string coordinates)
        {
            var x = CoordinateParser.Parse(coordinates[0]);
            var parseResult = int.TryParse(coordinates[1].ToString(), out var y);

            return parseResult && Grid.Hit(x, y);
        }

        public void AddShip(IShip ship)
        {
            Ships.Add(ship);
        }
    }
}