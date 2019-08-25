using System.Drawing;
using Xunit;

namespace BattleShip.GameLogic.Tests
{
    public class ShipTests
    {
        private const int SHIP_SIZE = 5;
        private const ShipType SHIP_TYPE = ShipType.BattleShip;
        private static readonly Point StartingPosition = new Point(0, 0);
        private static readonly Point EndingPosition = new Point(0, 1);

        [Fact]
        public void Can_Create()
        {
            new Ship(SHIP_SIZE, SHIP_TYPE);
        }

        [Fact]
        public void Can_Initialize_Type_And_Size()
        {
            var ship = new Ship(SHIP_SIZE, SHIP_TYPE);
            
            Assert.Equal(SHIP_SIZE, ship.Size);
            Assert.Equal(SHIP_TYPE, ship.ShipType);
        }

        [Fact]
        public void Can_Set_Starting_And_Ending_Position()
        {
            var ship = new Ship(SHIP_SIZE, SHIP_TYPE);

            ship.SetPositions(StartingPosition, EndingPosition);
            
            Assert.Equal(StartingPosition, ship.StartingPosition);
            Assert.Equal(EndingPosition, ship.EndingPosition);
        }
    }
}