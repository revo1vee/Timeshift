using System.Collections.Generic;
using System.Drawing;
using Timeshift.Controllers;

namespace Timeshift.Domain
{
    public class Player : Entity
    {
        public Stack<TilePoint> TimeMoves;

        public Player(TilePoint position, int idleFrames, int runFrames, int attackFrames, int waitFrames, Image spriteSheet)
        {
            Position = position;
            IdleFrames = idleFrames;
            RunFrames = runFrames;
            AttackFrames = attackFrames;
            WaitingFrames = waitFrames;
            SpriteSheet = spriteSheet;
            MovementRange = 8;
            Direction = Direction.Right;
            MoveDirection = new TilePoint();
            SizeX = 50;
            SizeY = 37;
            CurrentAnimation = AnimationType.Idle;
            CurrentFrame = 0;
            CurrentFrameLimit = IdleFrames;
            Flip = 1;
            Health = 3;
            TimeMoves = new Stack<TilePoint>();
            TimeMoves.Push(Position);
        }

        public void Attack()
        {
            if (MapController.State == GameState.Normal)
            {
                var attackRange = Direction == Direction.Right ? MapController.TileSize
                    : (Direction == Direction.Left ? -MapController.TileSize : 0);
                foreach (var enemy in MapController.Enemies)
                {
                    if (MapController.GetPointFromCoordinates(new TilePoint(Position.X + attackRange, Position.Y))
                        .Equals(MapController.GetPointFromCoordinates(new TilePoint(enemy.Position.X, enemy.Position.Y))))
                        enemy.Health--;
                    if (enemy.Health == 0)
                        enemy.SetAnimationConfiguration(AnimationType.Waiting);
                }
            }
        }

        public void TimeDash()
        {
            while (TimeMoves.Count > 1)
            {
                var dashPoint = TimeMoves.Pop();
                if (!PhysicsController.IsCollide(dashPoint))
                {
                    Position = dashPoint;
                    break;
                }
            }
            TimeMoves = new Stack<TilePoint>();
            TimeMoves.Push(Position);
        }
    }
}
