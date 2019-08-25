using System;

namespace BattleShip.GameLogic
{
    public static class CoordinateParser
    {
        public static int Parse(char coordinate)
        {
            if (coordinate <= 0) 
                throw new ArgumentOutOfRangeException(nameof(coordinate));
            
            return char.ToLower(coordinate) - 'a';
        }
    }
}