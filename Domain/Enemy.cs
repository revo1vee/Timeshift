using System.Collections.Generic;
using System.Drawing;
using Timeshift.Controllers;

namespace Timeshift.Domain
{
    public class Enemy : Entity
    {
        public bool Defeated;
        public double Damage;

        public Enemy(TilePoint position, int runFrames, Image spriteSheet)
        {
            Position = position;
            RunFrames = runFrames;
            SpriteSheet = spriteSheet;
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

        public void Attack(Player player)
        {
            if (MapController.State == GameState.Normal)
            {
                if (MapController.GetPointFromCoordinates(new TilePoint(Position.X, Position.Y))
                    .Equals(MapController.GetPointFromCoordinates(new TilePoint(player.Position.X, player.Position.Y))))
                    player.Health -= Damage;
                if (player.Health == 0)
                    MapController.State = GameState.Frozen;
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
