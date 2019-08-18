using Xunit;

namespace BattleShip.GameLogic.Tests
{
    public class CoordinateParserTests
    {
        [Theory]
        [InlineData('a', 0)]
        [InlineData('A', 0)]
        [InlineData('b', 1)]
        [InlineData('B', 1)]
        [InlineData('j', 9)]
        [InlineData('J', 9)]
        public void Can_Parse(char coordinate, int expectedResult)
        {
            var result = CoordinateParser.Parse(coordinate);
            
            Assert.Equal(expectedResult, result);
        }
    }
}