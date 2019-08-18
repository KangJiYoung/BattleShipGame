using System.Drawing;

namespace BattleShip.GameLogic
{
    public class BattleShipGame
    {
        public Grid Grid { get; }
        public IRandomGenerator RandomGenerator { get; }

        public BattleShipGame(Grid grid, IRandomGenerator randomGenerator)
        {
            Grid = grid;
            RandomGenerator = randomGenerator;
        }

        public void GenerateRandomShip(int shipSize)
        {
            int result;
            do
            {
                Point startingPosition;
                Point endingPosition;
            
                var isHorizontal = RandomGenerator.GetRandomBoolean();
                if (isHorizontal)
                {
                    startingPosition = new Point
                    {
                        X = RandomGenerator.GetRandomNumber(0, Grid.Rows),
                        Y = RandomGenerator.GetRandomNumber(0, Grid.Columns - shipSize)
                    };
                    endingPosition = new Point
                    {
                        X = startingPosition.X,
                        Y = startingPosition.Y + shipSize - 1
                    };
                }
                else
                {
                    startingPosition = new Point
                    {
                        X = RandomGenerator.GetRandomNumber(0, Grid.Rows - shipSize),
                        Y = RandomGenerator.GetRandomNumber(0, Grid.Columns)
                    };
                    endingPosition = new Point
                    {
                        X = startingPosition.X + shipSize - 1,
                        Y = startingPosition.Y
                    };
                }
                
                result = Grid.AddShip(startingPosition, endingPosition);
            } while (result == -1);
        }

        public void NewGame()
        {
            foreach (var shipSize in new[] {5, 4, 4})
                GenerateRandomShip(shipSize);
        }

        public bool Hit(string coordinates)
        {
            var x = CoordinateParser.Parse(coordinates[0]);
            var parseResult = int.TryParse(coordinates[1].ToString(), out var y);

            return parseResult && Grid.Hit(x, y);
        }
    }
}