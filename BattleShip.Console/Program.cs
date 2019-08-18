using System;
using System.Drawing;
using BattleShip.GameLogic;

namespace BattleShip
{
    internal static class Program
    {
        private const int CONSOLE_CELL_WIDTH = 5;

        private static void Main()
        {
            var grid = new Grid(10, 10);
            var randomGenerator = new RandomGenerator();
            var battleShipGame = new BattleShipGame(grid, randomGenerator);
            battleShipGame.NewGame();

            var status = string.Empty;
            
            while (!battleShipGame.Grid.AreAllShipsDead)
            {
                for (var i = -1; i < battleShipGame.Grid.Rows; i++)
                {
                    for (var j = -1; j < battleShipGame.Grid.Columns; j++)
                    {
                        if (i == -1 && j == -1)
                            Console.Write($"{string.Empty,CONSOLE_CELL_WIDTH}");
                        else if (i == -1)
                            Console.Write($"{j,CONSOLE_CELL_WIDTH}");
                        else if (j == -1)
                            Console.Write($"{(char) ('A' + i),CONSOLE_CELL_WIDTH}");
                        else
                        {
                            var point = new Point(i, j);
                            var isHit = battleShipGame.Grid.Hits.ContainsKey(point);
                            if (!isHit)
                                Console.Write($"{"?",CONSOLE_CELL_WIDTH}");
                            else
                                Console.Write(battleShipGame.Grid.Hits[point]
                                    ? $"{"X",CONSOLE_CELL_WIDTH}"
                                    : $"{"-",CONSOLE_CELL_WIDTH}");
                        }
                    }

                    Console.WriteLine();
                }

                if (!string.IsNullOrWhiteSpace(status))
                {
                    Console.WriteLine(status);
                    Console.WriteLine($"Score (number of hits): {battleShipGame.Grid.Hits.Count}");
                }
                
                Console.Write("Enter coordinates: ");
                var coordinates = Console.ReadLine();
                if (coordinates.Length == 2)
                    status = battleShipGame.Hit(coordinates)
                        ? "You hit a ship!"
                        : "You missed!";
                else
                    status = "Invalid Coordinates";
            }

            Console.WriteLine("You Won!!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}