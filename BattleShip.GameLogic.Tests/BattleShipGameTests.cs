using System.Drawing;
using Moq;
using Xunit;

namespace BattleShip.GameLogic.Tests
{
    public class BattleShipGameTests
    {
        private const int ROWS = 10;
        private const int COLUMNS = 9;
        private const int BATTLESHIP_SIZE = 5;
        private const int DESTROYER_SIZE = 4;
        
        [Fact]
        public void Can_Create()
        {
            new BattleShipGame(GetGrid(), new RandomGenerator());
        }

        [Fact]
        public void Can_Initialize_With_Grid()
        {
            var battleShipGame = new BattleShipGame(GetGrid(), new RandomGenerator());
            
            Assert.NotNull(battleShipGame.Grid);
            Assert.NotNull(battleShipGame.RandomGenerator);
        }

        [Fact]
        public void Can_Generate_Random_Horizontal_Ship()
        {
            var randomGeneratorMock = new Mock<IRandomGenerator>();
            randomGeneratorMock
                .Setup(it => it.GetRandomBoolean())
                .Returns(true);
            // Setup random row
            randomGeneratorMock
                .Setup(it => it.GetRandomNumber(0, ROWS))
                .Returns(1);
            // Setup column with ROWS - ship size in order to exclude invalid ships
            randomGeneratorMock
                .Setup(it => it.GetRandomNumber(0, COLUMNS - BATTLESHIP_SIZE))
                .Returns(1);
            
            var battleShipGame = new BattleShipGame(GetGrid(), randomGeneratorMock.Object);
            battleShipGame.GenerateRandomShip(BATTLESHIP_SIZE);
            
            Assert.True(battleShipGame.Grid.Matrix[1, 1]);
            Assert.True(battleShipGame.Grid.Matrix[1, 2]);
            Assert.True(battleShipGame.Grid.Matrix[1, 3]);
            Assert.True(battleShipGame.Grid.Matrix[1, 4]);
            Assert.True(battleShipGame.Grid.Matrix[1, 5]);
        }
        
        [Fact]
        public void Can_Generate_Random_Vertical_Ship()
        {
            var randomGeneratorMock = new Mock<IRandomGenerator>();
            randomGeneratorMock
                .Setup(it => it.GetRandomBoolean())
                .Returns(false);
            randomGeneratorMock
                .Setup(it => it.GetRandomNumber(0, ROWS - BATTLESHIP_SIZE))
                .Returns(1);
            randomGeneratorMock
                .Setup(it => it.GetRandomNumber(0, COLUMNS))
                .Returns(1);
            
            var battleShipGame = new BattleShipGame(GetGrid(), randomGeneratorMock.Object);
            battleShipGame.GenerateRandomShip(BATTLESHIP_SIZE);
            
            Assert.True(battleShipGame.Grid.Matrix[1, 1]);
            Assert.True(battleShipGame.Grid.Matrix[2, 1]);
            Assert.True(battleShipGame.Grid.Matrix[3, 1]);
            Assert.True(battleShipGame.Grid.Matrix[4, 1]);
            Assert.True(battleShipGame.Grid.Matrix[5, 1]);
        }

        [Fact]
        public void Can_Retry_Generating_Ship_If_Ship_Was_On_Top_Of_Another_Ship()
        {
            var randomGeneratorMock = new Mock<IRandomGenerator>();
            randomGeneratorMock
                .SetupSequence(it => it.GetRandomBoolean())
                .Returns(false).Returns(false);
            randomGeneratorMock
                .SetupSequence(it => it.GetRandomNumber(0, ROWS - BATTLESHIP_SIZE))
                .Returns(1).Returns(2); // 2nd time we return a 2 since first row contains a ship
            randomGeneratorMock
                .SetupSequence(it => it.GetRandomNumber(0, COLUMNS))
                .Returns(1).Returns(1);

            var grid = GetGrid();
            grid.AddShip(new Point(1, 1), new Point(1, 5));
            var battleShipGame = new BattleShipGame(grid, randomGeneratorMock.Object);
            battleShipGame.GenerateRandomShip(5);

            Assert.True(battleShipGame.Grid.Matrix[2, 1]);
            Assert.True(battleShipGame.Grid.Matrix[3, 1]);
            Assert.True(battleShipGame.Grid.Matrix[4, 1]);
            Assert.True(battleShipGame.Grid.Matrix[5, 1]);
            Assert.True(battleShipGame.Grid.Matrix[6, 1]);
        }

        [Fact]
        public void On_New_Game_Initialize_Three_Ships()
        {
            var randomGeneratorMock = new Mock<IRandomGenerator>();
            randomGeneratorMock
                .SetupSequence(it => it.GetRandomBoolean())
                .Returns(true).Returns(true).Returns(false);
            randomGeneratorMock
                .SetupSequence(it => it.GetRandomNumber(0, ROWS))
                .Returns(1).Returns(3);
            randomGeneratorMock
                .SetupSequence(it => it.GetRandomNumber(0, COLUMNS - BATTLESHIP_SIZE))
                .Returns(1);
            randomGeneratorMock
                .Setup(it => it.GetRandomNumber(0, COLUMNS - DESTROYER_SIZE))
                .Returns(3);
            randomGeneratorMock
                .SetupSequence(it => it.GetRandomNumber(0, ROWS - DESTROYER_SIZE))
                .Returns(5);
            randomGeneratorMock
                .Setup(it => it.GetRandomNumber(0, COLUMNS))
                .Returns(5);
            
            var battleShipGame = new BattleShipGame(GetGrid(), randomGeneratorMock.Object);
            battleShipGame.NewGame();
            
            // Battle Ship
            Assert.True(battleShipGame.Grid.Matrix[1, 1]);
            Assert.True(battleShipGame.Grid.Matrix[1, 2]);
            Assert.True(battleShipGame.Grid.Matrix[1, 3]);
            Assert.True(battleShipGame.Grid.Matrix[1, 4]);
            Assert.True(battleShipGame.Grid.Matrix[1, 5]);
            
            // First Destroyer
            Assert.True(battleShipGame.Grid.Matrix[3, 3]);
            Assert.True(battleShipGame.Grid.Matrix[3, 4]);
            Assert.True(battleShipGame.Grid.Matrix[3, 5]);
            Assert.True(battleShipGame.Grid.Matrix[3, 6]);
            
            // Second Destroyer
            Assert.True(battleShipGame.Grid.Matrix[5, 5]);
            Assert.True(battleShipGame.Grid.Matrix[6, 5]);
            Assert.True(battleShipGame.Grid.Matrix[7, 5]);
            Assert.True(battleShipGame.Grid.Matrix[8, 5]);
        }

        [Theory]
        [InlineData("B1", true)]
        [InlineData("B5", true)]
        [InlineData("A1", false)]
        [InlineData("Z8", false)]
        [InlineData("2A", false)]
        [InlineData("22", false)]
        public void Can_Hit_Ship(string coordinate, bool expectedResult)
        {
            var grid = GetGrid();
            grid.AddShip(new Point(1, 1), new Point(1, 5));
            var battleShipGame = new BattleShipGame(grid, new Mock<IRandomGenerator>().Object);

            var result = battleShipGame.Hit(coordinate);
            
            Assert.Equal(expectedResult, result);
        }

        private static Grid GetGrid() => new Grid(ROWS, COLUMNS);
    }
}