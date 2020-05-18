using System.Drawing;

namespace Timeshift.Domain
{
    public class SmallOrc : Enemy
    {
        public SmallOrc(TilePoint position)
        {
            Position = position;
            MoveDirection = new TilePoint();
            AttackPattern = AttackPatterns.OnTouch;
        }

        public SmallOrc(TilePoint position, int runFrames, Image spriteSheet)
        {
            Position = position;
            RunFrames = runFrames;
            SpriteSheet = spriteSheet;
            SpriteID = 2;
            MovementRange = 4;
            Direction = Direction.Right;
            Flip = 1;
            MoveDirection = new TilePoint();
            SizeX = 16;
            SizeY = 16;
            CurrentAnimation = AnimationType.Run;
            CurrentFrame = 0;
            CurrentFrameLimit = IdleFrames;
            Health = 3;
            Damage = 0.5;
            AttackPattern = AttackPatterns.OnTouch;
        }
    }
}