using System.Collections.Generic;
using System.Drawing;
using Timeshift.Controllers;

namespace Timeshift.Domain
{
    public class Enemy : Entity
    {
        public bool Defeated;

        public Enemy(TilePoint position, int idleFrames, int runFrames, int attackFrames, int waitFrames, Image spriteSheet)
        {
            Position = position;
            IdleFrames = 8;
            RunFrames = runFrames;
            AttackFrames = attackFrames;
            WaitingFrames = waitFrames;
            SpriteSheet = spriteSheet;
            MovementRange = 4;
            Direction = Direction.Right;
            Flip = 1;
            MoveDirection = new TilePoint();
            SizeX = 16;
            SizeY = 16;
            CurrentAnimation = AnimationType.Idle;
            CurrentFrame = 0;
            CurrentFrameLimit = IdleFrames;
            Health = 3;
        }

        public void SetDirection(TilePoint playerPos)
        {
            if (playerPos.X > Position.X)
            {
                Direction = Direction.Right;
                Flip = 1;
                MoveDirection.X = 1;
            }
            else if (playerPos.X < Position.X)
            {
                Direction = Direction.Left;
                Flip = -1;
                MoveDirection.X = -1;
            }
            else
            {
                MoveDirection.X = 0;
            }
            if (playerPos.Y > Position.Y)
            {
                Direction = Direction.Down;
                MoveDirection.Y = 1;
            }
            else if (playerPos.Y < Position.Y)
            {
                Direction = Direction.Up;
                MoveDirection.Y = -1;
            }
            else
            {
                MoveDirection.Y = 0;
            }
            IsMoving = MoveDirection.Equals(new TilePoint()) ? false : true;
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
                        enemy.Defeated = true;
                }
            }
        }

        public IEnumerable<SinglyLinkedList<TilePoint>> FindPathsToPlayer(TilePoint start, TilePoint target)
        {
            var visited = new HashSet<TilePoint>();
            var queue = new Queue<SinglyLinkedList<TilePoint>>();
            queue.Enqueue(new SinglyLinkedList<TilePoint>(start));
            while (queue.Count != 0)
            {
                var path = queue.Dequeue();
                if (!MapController.InBounds(path.Value.X, path.Value.Y) || visited.Contains(path.Value)
                    || true) continue;
                visited.Add(path.Value);
                if (path.Value == target) yield return path;
                foreach (var direction in PossibleDirections())
                    queue.Enqueue(new SinglyLinkedList<TilePoint>(path.Value + direction));
            }
        }

        public void PlayAnimation(Graphics g)
        {
            SetFrame();
            DrawEnemy(g);
        }

        private void SetFrame()
        {
            if (MapController.State == GameState.Normal)
                if (CurrentFrame < 7) CurrentFrame++;
                else CurrentFrame = 0;
        }

        private void DrawEnemy(Graphics g)
        {
            g.DrawImage(SpriteSheet, new Rectangle(new Point(Position.X - Flip * SizeX, Position.Y + 32), 
                new Size(Flip * SizeX * 2, SizeY * 2)), 16 * (23 + CurrentFrame), 16 * 2, SizeX, SizeY, GraphicsUnit.Pixel);
        }
    }
}
