using System.Drawing;

namespace BattleShip.GameLogic
{
    public interface IShip
    {
        int Size { get; }
        Point StartingPosition { get; }
        Point EndingPosition { get; }
        
        void SetPositions(Point startingPosition, Point endingPosition);
    }
}