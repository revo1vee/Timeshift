using System.Drawing;

namespace Timeshift.Domain
{
    public class Enemy : Entity
    {
        public Enemy(int x, int y, int idle, int run, int attack, int wait, Image sprites)
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
        }
    }
}
