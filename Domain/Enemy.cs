using System;
using System.Diagnostics;
using System.Drawing;
using Timeshift.Controllers;

namespace Timeshift.Domain
{
    public class Enemy : Entity
    {
        public bool Defeated;
        public double Damage;
        public int SpriteID;
        public Action<Player> AttackPattern;
        public bool IsProtected;
        public Stopwatch RandomMovement = new Stopwatch();

        public void SetDirection(TilePoint playerPos)
        {
            var rnd = new Random();
            if (RandomMovement.IsRunning)
            {
                MoveDirection.X = rnd.Next(-1, 1);
                MoveDirection.Y = rnd.Next(-1, 1);
                return;
            }
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
            AttackPattern(player);
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
                new Size(Flip * SizeX * 2, SizeY * 2)), 16 * (23 + CurrentFrame), 16 * SpriteID, SizeX, SizeY, GraphicsUnit.Pixel);
        }
    }
}
