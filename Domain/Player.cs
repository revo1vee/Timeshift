using System.Collections.Generic;
using System.Drawing;
using Timeshift.Controllers;

namespace Timeshift.Domain
{
    public class Player : Entity
    {
        public Queue<Direction> TimeMoves;

        public Player(Point position, int idleFrames, int runFrames, int attackFrames, int waitFrames, Image spriteSheet)
        {
            Position = position;
            IdleFrames = idleFrames;
            RunFrames = runFrames;
            AttackFrames = attackFrames;
            WaitingFrames = waitFrames;
            SpriteSheet = spriteSheet;
            MovementRange = 32;
            Direction = Direction.Right;
            SizeX = 50;
            SizeY = 37;
            CurrentAnimation = AnimationType.Idle;
            CurrentFrame = 0;
            CurrentFrameLimit = IdleFrames;
            Flip = 1;
            Health = 3;
            TimeMoves = new Queue<Direction>();
        }

        public void Attack()
        {
            if (MapController.State == GameState.Normal)
            {
                var attackDirection = Direction == Direction.Right ? MovementRange : (Direction == Direction.Left ? -MovementRange : 0);
                foreach (var enemy in MapController.Enemies)
                {
                    if (enemy.Position.X == Position.X + attackDirection && enemy.Position.Y == Position.Y)
                        enemy.Health--;
                    if (enemy.Health == 0)
                        enemy.SetAnimationConfiguration(AnimationType.Waiting);
                }
            }
        }

        public void TimeDash()
        {
            while (TimeMoves.Count > 0)
            {
                switch (TimeMoves.Dequeue())
                {
                    case Direction.Up:
                        MoveDirection.Y = -1;
                        break;
                    case Direction.Left:
                        MoveDirection.X = -1;
                        break;
                    case Direction.Down:
                        MoveDirection.Y = 1;
                        break;
                    case Direction.Right:
                        MoveDirection.X = 1;
                        break;
                }
                Move();
                MoveDirection = Point.Empty;
            }
        }
    }
}
