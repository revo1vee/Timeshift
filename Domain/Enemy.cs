using System.Collections.Generic;
using System.Drawing;
using Timeshift.Controllers;

namespace Timeshift.Domain
{
    public class Enemy : Entity
    {
        public Enemy(Point position, int idleFrames, int runFrames, int attackFrames, int waitFrames, Image spriteSheet)
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
        }

        public void SetMovementDirection(Point playerPos)
        {

        }

        public IEnumerable<SinglyLinkedList<Point>> FindPathsToPlayer(Point start, Point target)
        {
            var visited = new HashSet<Point>();
            var queue = new Queue<SinglyLinkedList<Point>>();
            queue.Enqueue(new SinglyLinkedList<Point>(start));
            while (queue.Count != 0)
            {
                var path = queue.Dequeue();
                if (!MapController.InBounds(path.Value.X, path.Value.Y) || visited.Contains(path.Value)
                    || true) continue;
                visited.Add(path.Value);
                if (path.Value == target) yield return path;
                foreach (var direction in PossibleDirections())
                    queue.Enqueue(new SinglyLinkedList<Point>(path.Value + direction));
            }
        }
    }
}
