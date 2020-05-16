using System.Diagnostics;
using System.Drawing;
using Timeshift.Controllers;

namespace Timeshift.Domain
{
    public class SmallOrc : Enemy
    {
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
            RandomMovementDuration = new Stopwatch();
            RandomMovementTrigger = Stopwatch.StartNew();
            Health = 3;
            Damage = 0.5;
            AttackPattern = (player) =>
            {
                if (MapController.State == GameState.Normal)
                {
                    if (MapController.GetPointFromCoordinates(new TilePoint(Position.X, Position.Y))
                        .Equals(MapController.GetPointFromCoordinates(new TilePoint(player.Position.X, player.Position.Y))))
                        player.TakeDamage(Damage);
                }
            };
        }
    }
}
