using System;
using System.Drawing;
using Timeshift.Controllers;

namespace Timeshift.Domain
{
    public class Enemy : Entity
    {
        public bool Defeated;
        public double Damage;
        public int SpriteID;
        public Action<Enemy, Player> AttackPattern;
        public bool IsProtected;

        public void SetDirection(TilePoint target)
        {
            if (target.X > Position.X)
            {
                Direction = Direction.Right;
                MoveDirection.X = 1;
            }
            else if (target.X < Position.X)
            {
                Direction = Direction.Left;
                MoveDirection.X = -1;
            }
            else MoveDirection.X = 0;

            if (target.Y > Position.Y) MoveDirection.Y = 1;
            else if (target.Y < Position.Y) MoveDirection.Y = -1;
            else MoveDirection.Y = 0;

            Flip = (int)Direction;
            IsMoving = MoveDirection.Equals(new TilePoint()) ? false : true;
        }

        public void Attack(Player player)
        {
            AttackPattern(this, player);
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