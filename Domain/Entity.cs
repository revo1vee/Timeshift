using System.Collections.Generic;
using System.Drawing;
using Timeshift.Controllers;

namespace Timeshift.Domain
{
    public class Entity
    {
        public TilePoint Position;
        public double Health;

        public bool IsMoving;
        public int MovementRange;
        public TilePoint MoveDirection;
        public Direction Direction;

        public Image SpriteSheet;
        public AnimationType PrevAnimation;
        public AnimationType CurrentAnimation;
        public int CurrentFrame;
        public int CurrentFrameLimit;
        public int IdleFrames;
        public int RunFrames;
        public int AttackFrames;
        public int WaitingFrames;
        public int Flip;
        public int SizeX;
        public int SizeY;

        public void Move()
        {
            if (MapController.State == GameState.Normal)
            {
                if (this is Enemy)
                {
                    foreach (var enemy in MapController.Enemies)
                    {
                        if (enemy.Defeated || enemy.Position.Equals(Position)) continue;
                        if (MapController.GetPointFromCoordinates(enemy.Position)
                            .Equals(MapController.GetPointFromCoordinates(new TilePoint(Position.X + MoveDirection.X * MovementRange + 15 * MoveDirection.X,
                            Position.Y + MoveDirection.Y * MovementRange)))) return;
                    }
                }
                if (!PhysicsController.IsCollide(new TilePoint(Position.X + MoveDirection.X * MovementRange + 15 * MoveDirection.X, Position.Y))) 
                    Position.X += MoveDirection.X * MovementRange;
                if (!PhysicsController.IsCollide(new TilePoint(Position.X, Position.Y + MoveDirection.Y * MovementRange)))
                    Position.Y += MoveDirection.Y * MovementRange;
            }
        }

        public static IEnumerable<Size> PossibleDirections()
        {
            for (var dy = -1; dy <= 1; dy++)
                for (var dx = -1; dx <= 1; dx++)
                    if (dx == 0 || dy == 0) yield return new Size(dx, dy);
        }
    }
}