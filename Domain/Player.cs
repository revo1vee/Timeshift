using System.Collections.Generic;
using System.Drawing;
using Timeshift.Controllers;

namespace Timeshift.Domain
{
    public class Player : Entity
    {
        public Queue<Direction> TimeMoves;

        public Player(int x, int y, int idle, int run, int attack, int wait, Image sprites)
        {
            PosX = x;
            PosY = y;
            MoveRange = 32;
            Direction = Direction.Right;
            IdleFrames = idle;
            RunFrames = run;
            AttackFrames = attack;
            WaitingFrames = wait;
            SpriteSheet = sprites;
            SizeX = 50;
            SizeY = 37;
            CurrentAnimation = AnimationType.Idle;
            CurrentFrame = 0;
            CurrentLimit = IdleFrames;
            Flip = 1;
            Health = 3;
            TimeMoves = new Queue<Direction>();
        }

        public void Attack()
        {
            if (MapController.State == GameState.Normal)
            {
                var attackX = Direction == Direction.Right ? MoveRange : (Direction == Direction.Left ? -MoveRange : 0);
                foreach (var enemy in MapController.Enemies)
                {
                    if (enemy.PosX == attackX + PosX && enemy.PosY == PosY)
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
                        DirY = -MoveRange;
                        break;
                    case Direction.Left:
                        DirX = -MoveRange;
                        break;
                    case Direction.Down:
                        DirY = MoveRange;
                        break;
                    case Direction.Right:
                        DirX = MoveRange;
                        break;
                }
                Move();
                DirX = 0;
                DirY = 0;
            }
        }
    }
}
