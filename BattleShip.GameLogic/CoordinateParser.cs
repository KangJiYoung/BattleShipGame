namespace BattleShip.GameLogic
{
    public static class CoordinateParser
    {
        public static int Parse(char coordinate)
        {
            return char.ToLower(coordinate) - 'a';
        }
    }
}