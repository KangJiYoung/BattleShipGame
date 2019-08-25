using System.Drawing;
using System.Linq;
using Xunit;

namespace BattleShip.GameLogic.Tests
{
    public class GridTests
    {
        private const int ROWS = 10;
        private const int COLUMNS = 10;
        private const int SHIP_SIZE = -1;
        private const ShipType SHIP_TYPE = ShipType.BattleShip;

        [Fact]
        public void Can_Create()
        {
            new Grid(ROWS, COLUMNS);
        }

        [Fact]
        public void Can_Initialize_Rows_And_Height()
        {
            var grid = new Grid(ROWS, COLUMNS);
            
            Assert.Equal(ROWS, grid.Rows);
            Assert.Equal(COLUMNS, grid.Columns);
        }

        [Fact]
        public void Can_Initialize_A_Matrix()
        {
            var grid = new Grid(ROWS, COLUMNS);

            Assert.Equal(ROWS, grid.Matrix.GetLength(0));
            Assert.Equal(COLUMNS, grid.Matrix.GetLength(1));
            Assert.True(grid.Matrix.Cast<bool>().All(it => !it));
        }

        [Fact]
        public void Can_Initialize_Hashset()
        {
            var grid = new Grid(ROWS, COLUMNS);

            Assert.NotNull(grid.Hits);
        }

        [Fact]
        public void Can_Add_Horizontal_Ship()
        {
            var grid = new Grid(ROWS, COLUMNS);
            var ship = new Ship(SHIP_SIZE, SHIP_TYPE);
            ship.SetPositions(new Point(0, 1), new Point(0, 3));

            grid.AddShip(ship);

            Assert.True(grid.Matrix[0, 1]);
            Assert.True(grid.Matrix[0, 2]);
            Assert.True(grid.Matrix[0, 3]);
        }

        [Fact]
        public void Can_Add_Vertical_Ship()
        {
            var grid = new Grid(ROWS, COLUMNS);
            var ship = new Ship(SHIP_SIZE, SHIP_TYPE);
            ship.SetPositions(new Point(1, 3), new Point(3, 3));
            
            grid.AddShip(ship);
            
            Assert.True(grid.Matrix[1, 3]);
            Assert.True(grid.Matrix[2, 3]);
            Assert.True(grid.Matrix[3, 3]);
        }

        [Fact]
        public void Can_Add_Multiple_Ships()
        {
            var grid = new Grid(ROWS, COLUMNS);
            var horizontalShip = new Ship(SHIP_SIZE, SHIP_TYPE);
            horizontalShip.SetPositions(new Point(0, 1), new Point(0, 3));
            var verticalShip = new Ship(SHIP_SIZE, SHIP_TYPE);
            verticalShip.SetPositions(new Point(1, 3), new Point(3, 3));

            grid.AddShip(horizontalShip);
            grid.AddShip(verticalShip);

            Assert.True(grid.Matrix[0, 1]);
            Assert.True(grid.Matrix[0, 2]);
            Assert.True(grid.Matrix[0, 3]);
            
            Assert.True(grid.Matrix[1, 3]);
            Assert.True(grid.Matrix[2, 3]);
            Assert.True(grid.Matrix[3, 3]);
        }

        [Theory]
        [InlineData(0, 0, 0, -1)]
        [InlineData(0, 0, 0, 11)]
        [InlineData(0, 0, -1, 0)]
        [InlineData(0, 0, 11, 0)]
        [InlineData(0, -1, 0, 0)]
        [InlineData(0, 11, 0, 0)]
        [InlineData(-1, 0, 0, 0)]
        [InlineData(11, 0, 0, 0)]
        public void Can_Return_Minus_One_On_Invalid_Coordinates(int x1, int y1, int x2, int y2)
        {
            var grid = new Grid(ROWS, COLUMNS);
            var ship = new Ship(SHIP_SIZE, SHIP_TYPE);
            ship.SetPositions(new Point(x1, y1), new Point(x2, y2));

            var result = grid.AddShip(ship);
            
            Assert.Equal(-1, result);
        }

        [Theory]
        [InlineData(0, 0, 1, 1)]
        [InlineData(3, 3, 6, 8)]
        public void Can_Return_Minus_One_If_Ship_Is_Not_Horizontal_Or_Vertical(int x1, int y1, int x2, int y2)
        {
            var grid = new Grid(ROWS, COLUMNS);
            var ship = new Ship(SHIP_SIZE, SHIP_TYPE);
            ship.SetPositions(new Point(x1, y1), new Point(x2, y2));

            var result = grid.AddShip(ship);
            
            Assert.Equal(-1, result);
        }

        [Fact]
        public void Can_Return_Minus_One_If_Adding_Ship_On_Top_Of_Another_Ship()
        {
            var grid = new Grid(ROWS, COLUMNS);
            var ship = new Ship(SHIP_SIZE, SHIP_TYPE);
            ship.SetPositions(new Point(0, 1), new Point(0, 3));
            grid.AddShip(ship);

            var invalidShip = new Ship(SHIP_SIZE, SHIP_TYPE);
            invalidShip.SetPositions(new Point(0, 1), new Point(0, 3));
            var result = grid.AddShip(invalidShip);
            
            Assert.Equal(-1, result);
        }

        [Fact]
        public void Can_Hit_A_Ship()
        {
            var grid = new Grid(ROWS, COLUMNS);
            var ship = new Ship(SHIP_SIZE, SHIP_TYPE);
            ship.SetPositions(new Point(0, 1), new Point(0, 3));
            grid.AddShip(ship);

            var result = grid.Hit(0, 2);
            
            Assert.True(result);
            Assert.True(grid.Matrix[0, 1]);
            Assert.False(grid.Matrix[0, 2]);
            Assert.True(grid.Matrix[0, 3]);
        }

        [Fact]
        public void Can_Miss_A_Ship()
        {
            var grid = new Grid(ROWS, COLUMNS);
            var ship = new Ship(SHIP_SIZE, SHIP_TYPE);
            ship.SetPositions(new Point(0, 1), new Point(0, 3));
            grid.AddShip(ship);
            
            var result = grid.Hit(0, 4);

            Assert.False(result);
            Assert.True(grid.Matrix[0, 1]);
            Assert.True(grid.Matrix[0, 2]);
            Assert.True(grid.Matrix[0, 3]);
        }

        [Fact]
        public void Can_Return_False_On_Invalid_Indexes_When_Hitting()
        {
            var grid = new Grid(ROWS, COLUMNS);
            
            var result = grid.Hit(-1, -1);

            Assert.False(result);
        }

        [Fact]
        public void On_Initialize_All_Ships_Are_Dead()
        {
            var grid = new Grid(ROWS, COLUMNS);

            Assert.True(grid.AreAllShipsDead);
        }

        [Fact]
        public void On_Add_Ship_All_Ships_Are_Not_Dead()
        {
            var grid = new Grid(ROWS, COLUMNS);
            var ship = new Ship(SHIP_SIZE, SHIP_TYPE);
            ship.SetPositions(new Point(0, 1), new Point(0, 3));
            grid.AddShip(ship);

            Assert.False(grid.AreAllShipsDead);
        }

        [Fact]
        public void On_Destroy_Ship_All_Ships_Are_Dead()
        {
            var grid = new Grid(ROWS, COLUMNS);
            var ship = new Ship(SHIP_SIZE, SHIP_TYPE);
            ship.SetPositions(new Point(0, 1), new Point(0, 3));
            grid.AddShip(ship);

            grid.Hit(0, 1);
            grid.Hit(0, 2);
            grid.Hit(0, 3);
            
            Assert.True(grid.AreAllShipsDead);
        }

        [Fact]
        public void On_Miss_All_Ships_Are_Not_Dead()
        {
            var grid = new Grid(ROWS, COLUMNS);
            var ship = new Ship(SHIP_SIZE, SHIP_TYPE);
            ship.SetPositions(new Point(0, 1), new Point(0, 3));
            grid.AddShip(ship);
            
            grid.Hit(0, 1);
            grid.Hit(0, 2);
            grid.Hit(0, 4);
            
            Assert.False(grid.AreAllShipsDead);
        }

        [Theory]
        [InlineData(0, 1, true)]
        [InlineData(0, 4, false)]
        public void Can_Persist_A_List_Of_Hit_Indexes(int x, int y, bool hit)
        {
            var grid = new Grid(ROWS, COLUMNS);
            var ship = new Ship(SHIP_SIZE, SHIP_TYPE);
            ship.SetPositions(new Point(0, 1), new Point(0, 3));
            grid.AddShip(ship);
            
            grid.Hit(x, y);
            
            Assert.Equal(hit, grid.Hits[new Point(x, y)]);
        }

        [Fact]
        public void On_Hitting_The_Same_Index_The_Hit_Value_Does_Not_Change()
        {
            var grid = new Grid(ROWS, COLUMNS);
            var ship = new Ship(SHIP_SIZE, SHIP_TYPE);
            ship.SetPositions(new Point(0, 1), new Point(0, 3));
            grid.AddShip(ship);
            
            grid.Hit(0, 1);
            grid.Hit(0, 1);

            Assert.True(grid.Hits[new Point(0, 1)]);
        }
    }
}