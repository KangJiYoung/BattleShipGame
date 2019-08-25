using System;
using BattleShip.GameLogic;

namespace BattleShip
{
    internal static class Program
    {
        private const int GRID_ROWS = 10;
        private const int GRID_COLUMNS = 10;
        private const int BATTLESHIP_SIZE = 5;
        private const int DESTROYER_SIZE = 4;

        private static void Main()
        {
            try
            {
                var grid = new Grid(GRID_ROWS, GRID_COLUMNS);
                var randomGenerator = new RandomGenerator();
                var battleShipGame = new BattleShipGame(grid, randomGenerator);
                battleShipGame.AddShip(new Ship(BATTLESHIP_SIZE, ShipType.BattleShip));
                battleShipGame.AddShip(new Ship(DESTROYER_SIZE, ShipType.Destroyer));
                battleShipGame.AddShip(new Ship(DESTROYER_SIZE, ShipType.Destroyer));
                
                var battleShipUI = new BattleShipUI(battleShipGame);
                battleShipUI.Run();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}