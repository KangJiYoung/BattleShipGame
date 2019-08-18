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

        public int AddShip(Point startingPosition, Point endingPosition)
        {
            var isHorizontal = startingPosition.X == endingPosition.X;
            var isVertical = startingPosition.Y == endingPosition.Y;

            if (!isHorizontal && !isVertical)
                return -1;
                
            if (isHorizontal)
            {
                var startingY = Math.Min(startingPosition.Y, endingPosition.Y);
                var endingY = Math.Max(startingPosition.Y, endingPosition.Y);

                for (var j = startingY; j <= endingY; j++)
                    if (!IsValidIndex(startingPosition.X, j) || Matrix[startingPosition.X, j])
                        return -1;

                for (var j = startingY; j <= endingY; j++)
                    Matrix[startingPosition.X, j] = true;
            }
            else
            {
                var startingX = Math.Min(startingPosition.X, endingPosition.X);
                var endingX = Math.Max(startingPosition.X, endingPosition.X);
                
                for (var i = startingX; i <= endingX; i++)
                    if (!IsValidIndex(i, startingPosition.Y) || Matrix[i, startingPosition.Y])
                        return -1;
                
                for (var i = startingX; i <= endingX; i++)
                    Matrix[i, startingPosition.Y] = true;
            }

            AreAllShipsDead = false;

            return 0;
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