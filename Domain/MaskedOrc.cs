using System.Drawing;
using Timeshift.Controllers;

namespace Timeshift.Domain
{
    public class MaskedOrc : Enemy
    {
        public MaskedOrc(TilePoint position)
        {
            Position = position;
        }

        public MaskedOrc(TilePoint position, int runFrames, Image spriteSheet)
        {
            Position = position;
            RunFrames = runFrames;
            SpriteSheet = spriteSheet;
            SpriteID = 11;
            MovementRange = 2;
            Direction = Direction.Right;
            Flip = 1;
            MoveDirection = new TilePoint();
            SizeX = 16;
            SizeY = 16;
            CurrentAnimation = AnimationType.Run;
            CurrentFrame = 0;
            CurrentFrameLimit = IdleFrames;
            Health = 3;
            Damage = 1;
            IsProtected = true;
            AttackPattern = AttackPatterns.OnTouch;
        }
    }
}