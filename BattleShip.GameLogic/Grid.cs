using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BattleShip.GameLogic
{
    public class Grid
    {
        public int Rows { get; }
        public int Columns { get; }
        public bool[,] Matrix { get; }
        public bool AreAllShipsDead { get; private set; }
        public Dictionary<Point, bool> Hits { get; private set; }

        public Grid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;

            AreAllShipsDead = true;
            Matrix = new bool[rows, columns];
            Hits = new Dictionary<Point, bool>();
        }

        private bool IsValidIndex(int x, int y)
            => x >= 0 && x < Rows && 
               y >= 0 && y < Columns;

        public bool AddShip(IShip ship)
        {
            var tiles = GetShipTiles(ship);
            if (!tiles.Any())
                return false;

            if (tiles.Any(tile => !IsValidIndex(tile.X, tile.Y) || Matrix[tile.X, tile.Y]))
                return false;

            foreach (var tile in tiles)
                Matrix[tile.X, tile.Y] = true;
            
            AreAllShipsDead = false;

            return true;
        }

        private static IList<Point> GetShipTiles(IShip ship)
        {
            var tiles = new List<Point>();
            
            var isHorizontal = ship.StartingPosition.X == ship.EndingPosition.X;
            var isVertical = ship.StartingPosition.Y == ship.EndingPosition.Y;

            if (isHorizontal)
                GetHorizontalTiles(ship, tiles);
            else if (isVertical)
                GetVerticalTiles(ship, tiles);

            return tiles;
        }

        private static void GetHorizontalTiles(IShip ship, ICollection<Point> tiles)
        {
            var startingY = Math.Min(ship.StartingPosition.Y, ship.EndingPosition.Y);
            var endingY = Math.Max(ship.StartingPosition.Y, ship.EndingPosition.Y);

            for (var j = startingY; j <= endingY; j++)
                tiles.Add(new Point(ship.StartingPosition.X, j));
        }

        private static void GetVerticalTiles(IShip ship, ICollection<Point> tiles)
        {
            var startingX = Math.Min(ship.StartingPosition.X, ship.EndingPosition.X);
            var endingX = Math.Max(ship.StartingPosition.X, ship.EndingPosition.X);

            for (var i = startingX; i <= endingX; i++)
                tiles.Add(new Point(i, ship.StartingPosition.Y));
        }

        public bool Hit(int x, int y)
        {
            var point = new Point(x, y);
            if (!IsValidIndex(x, y) || Hits.ContainsKey(point))
                return false;

            Hits.Add(point, Matrix[x, y]);
            
            if (Matrix[x, y])
            {
                Matrix[x, y] = false;

                AreAllShipsDead = Matrix.Cast<bool>().All(it => !it);

                return true;
            }

            return false;
        }
    }
}