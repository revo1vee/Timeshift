using System.Drawing;

namespace MyGame.Domain
{
    public class Enemy
    {
        public Point Position { get; set; }
        public Room CurrentRoom { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public Direction Direction { get; set; }

        public Enemy(int x, int y, int hp, int dmg)
        {
            Direction = Direction.Down;
            Position = new Point(x, y);
            Health = hp;
            Damage = dmg;
        }

        public void Attack()
        {
            var attackPoint = new Point(Position.X, Position.Y);
            switch (Direction)
            {
                case Direction.Right:
                    attackPoint.X += 1;
                    break;
                case Direction.Left:
                    attackPoint.X -= 1;
                    break;
                case Direction.Up:
                    attackPoint.Y -= 1;
                    break;
                case Direction.Down:
                    attackPoint.Y += 1;
                    break;
            }
            if (CurrentRoom.Player.Position == attackPoint)
                CurrentRoom.Player.TakeDamage(Damage);
        }

        public void Move(int dx, int dy)
        {
            var pointToMove = new Point(Position.X + dx, Position.Y + dy);
            ChangeDirection(dx, dy);
            if (CurrentRoom.InRoom(pointToMove) && !CurrentRoom.IsHoleAt(pointToMove) && !CurrentRoom.IsEnemyAt(pointToMove))
            {
                if (CurrentRoom.Player.Position == pointToMove)
                    Attack();
                else
                {
                    Position = new Point(Position.X + dx, Position.Y + dy);
                    if (CurrentRoom.IsSpikeAt(pointToMove))
                        TakeDamage(1);
                }
            }
        }

        public void ChangeDirection(int dx, int dy)
        {
            if (dx == 1 && dy == 0) Direction = Direction.Right;
            else if (dx == -1 && dy == 0) Direction = Direction.Left;
            else if (dx == 0 && dy == 1) Direction = Direction.Down;
            else Direction = Direction.Up;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
        }
    }
}
