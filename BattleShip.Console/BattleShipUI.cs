using System;
using System.Drawing;
using System.Linq;
using BattleShip.GameLogic;

namespace BattleShip
{
    public class BattleShipUI
    {
        private const int CONSOLE_CELL_WIDTH = 5;
        private const int INDEX_OUTSIDE_OF_GRID = -1;
        private const string TILE_UNKNOWN = "?";
        private const string TILE_HIT = "X";
        private const string TILE_MISS = "-";

        public BattleShipGame BattleShipGame { get; }

        public string Status { get; private set; }

        public BattleShipUI(BattleShipGame battleShipGame)
        {
            BattleShipGame = battleShipGame;

            Status = string.Empty;
        }

        public void Run()
        {
            BattleShipGame.NewGame();

            while (!BattleShipGame.Grid.AreAllShipsDead)
            {
                DrawGrid();
                DisplayStatus();
                HandleCoordinates();

                Console.Clear();
            }

            DisplayEndingMessage();
        }

        private void DrawGrid()
        {
            for (var i = INDEX_OUTSIDE_OF_GRID; i < BattleShipGame.Grid.Rows; i++)
            {
                for (var j = INDEX_OUTSIDE_OF_GRID; j < BattleShipGame.Grid.Columns; j++)
                {
                    if (IsEmptyCell(i, j))
                        DrawEmptyCell();
                    else if (IsFirstColumn(i))
                        DrawColumnNumbers(j);
                    else if (IsFirstRow(j))
                        DrawRowCharacters(i);
                    else
                        DrawGrid(i, j);
                }

                Console.WriteLine();
            }
        }

        private void DisplayEndingMessage()
        {
            Console.WriteLine("You Won!!");
            DisplayScore();
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        private void HandleCoordinates()
        {
            Console.Write("Enter coordinates: ");

            var coordinates = Console.ReadLine();
            if (IsCoordinatesValid(coordinates))
                Status = BattleShipGame.Hit(coordinates)
                    ? "You hit a ship!"
                    : "You missed!";
            else
                Status = "Invalid Coordinates";
        }

        private static bool IsCoordinatesValid(string coordinates) 
            => coordinates.Length == 2 && char.IsLetter(coordinates.First()) && char.IsDigit(coordinates.Last());

        private void DisplayStatus()
        {
            if (!string.IsNullOrWhiteSpace(Status))
            {
                Console.WriteLine(Status);
                DisplayScore();
            }
        }

        private void DisplayScore()
        {
            Console.WriteLine($"Score (number of hits): {BattleShipGame.Grid.Hits.Count}");
        }

        private void DrawGrid(int x, int y)
        {
            var point = new Point(x, y);
            var isHit = BattleShipGame.Grid.Hits.ContainsKey(point);
            if (isHit)
                Console.Write(BattleShipGame.Grid.Hits[point]
                    ? $"{TILE_HIT,CONSOLE_CELL_WIDTH}"
                    : $"{TILE_MISS,CONSOLE_CELL_WIDTH}");
            else
                Console.Write($"{TILE_UNKNOWN,CONSOLE_CELL_WIDTH}");
        }

        private static void DrawRowCharacters(int row)
        {
            Console.Write($"{(char) ('A' + row),CONSOLE_CELL_WIDTH}");
        }

        private static bool IsFirstRow(int column) => column == INDEX_OUTSIDE_OF_GRID;

        private static void DrawColumnNumbers(int number)
        {
            Console.Write($"{number,CONSOLE_CELL_WIDTH}");
        }

        private static bool IsFirstColumn(int columnIndex) => columnIndex == INDEX_OUTSIDE_OF_GRID;

        private static void DrawEmptyCell()
        {
            Console.Write($"{string.Empty,CONSOLE_CELL_WIDTH}");
        }

        private static bool IsEmptyCell(int row, int column) =>
            row == INDEX_OUTSIDE_OF_GRID && column == INDEX_OUTSIDE_OF_GRID;
    }
}