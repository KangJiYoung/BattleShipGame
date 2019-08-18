namespace BattleShip.GameLogic
{
    public interface IRandomGenerator
    {
        bool GetRandomBoolean();
        int GetRandomNumber(int min, int max);
    }
}