using System;

namespace BattleShip.GameLogic
{
    public class RandomGenerator : IRandomGenerator
    {
        public Random Random { get; }
        
        public RandomGenerator()
        {
            Random = new Random();
        }

        public bool GetRandomBoolean() => Random.NextDouble() >= 0.5;

        public int GetRandomNumber(int min, int max) => Random.Next(min, max);
    }
}