using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Timeshift.Controllers;

namespace Timeshift.Domain
{
    public class Player : Entity
    {
        public Stack<TilePoint> TimeMoves;
        public Stopwatch IFrames;
        public Stopwatch FreezedTime;
        public Stopwatch TimeAfterDash;
        public double InitialHealth;

        public Player(TilePoint position, int idleFrames, int runFrames, int attackFrames, int waitFrames, Image spriteSheet)
        {
            Position = position;
            IdleFrames = idleFrames;
            RunFrames = runFrames;
            AttackFrames = attackFrames;
            WaitingFrames = waitFrames;
            SpriteSheet = spriteSheet;
            MovementRange = 8;
            Direction = Direction.Right;
            MoveDirection = new TilePoint();
            SizeX = 50;
            SizeY = 37;
            CurrentAnimation = AnimationType.Idle;
            CurrentFrame = 0;
            CurrentFrameLimit = IdleFrames;
            Flip = 1;
            Health = 5;
            InitialHealth = Health;
            TimeMoves = new Stack<TilePoint>();
            TimeMoves.Push(Position);
            IFrames = new Stopwatch();
            TimeAfterDash = new Stopwatch();
            FreezedTime = new Stopwatch();
        }

        public void Attack()
        {
            if (MapController.State == GameState.Normal)
            {
                foreach (var enemy in MapController.Enemies)
                {
                    if (enemy.IsProtected && (int)enemy.Direction == -(int)Direction) return;
                    if (IsAttackInRange(enemy))
                        enemy.Health--;
                    if (enemy.Health == 0)
                        enemy.Defeated = true;
                }
            }
        }

        public void TakeDamage(double damage)
        {
            if (IFrames.IsRunning) return;
            Health -= damage;
            if (Health <= 0) MapController.State = GameState.Frozen;
            else IFrames.Start();
        }

        private bool IsAttackInRange(Enemy enemy)
        {
            var attackRange = Direction == Direction.Right ? MapController.TileSize
                : (Direction == Direction.Left ? -MapController.TileSize : 0);
            var attackPoint = MapController.GetPointFromCoordinates(new TilePoint(Position.X + attackRange, Position.Y));
            var enemyPos = MapController.GetPointFromCoordinates(new TilePoint(enemy.Position.X, enemy.Position.Y));
            return Math.Abs(attackPoint.X - enemyPos.X) < 2 && attackPoint.Y == enemyPos.Y;
        }

        public void TimeDash()
        {
            while (TimeMoves.Count > 1)
            {
                var dashPoint = TimeMoves.Pop();
                if (!PhysicsController.IsCollide(dashPoint))
                {
                    Position = dashPoint;
                    break;
                }
            }
            TimeMoves = new Stack<TilePoint>();
            TimeMoves.Push(Position);
            TimeAfterDash.Start();
        }

        public void PlayAnimation(Graphics g)
        {
            CheckAttackAnimationFrame();
            SetFrame();
            if (IFrames.ElapsedMilliseconds % 100 > 10 && IFrames.ElapsedMilliseconds % 100 < 90) return;
            DrawPlayer(g);
        }

        private void CheckAttackAnimationFrame()
        {
            if (CurrentAnimation == AnimationType.Attack && CurrentFrame == CurrentFrameLimit - 1)
                if (IsMoving) SetAnimationConfiguration(AnimationType.Run);
                else SetAnimationConfiguration(AnimationType.Idle);
        }

        private void SetFrame()
        {
            if (MapController.State == GameState.Normal)
                if (CurrentFrame < CurrentFrameLimit - 1) CurrentFrame++;
                else if (CurrentAnimation == AnimationType.Run) CurrentFrame = 1;
                else CurrentFrame = 0;
        }

        private void DrawPlayer(Graphics g)
        {
            if (CurrentAnimation == AnimationType.Waiting)
                g.DrawImage(SpriteSheet, new Rectangle(new Point(Position.X - Flip * SizeX, Position.Y), new Size(Flip * SizeX * 2, SizeY * 2)),
                50 * (CurrentFrame + 4), 37 * ((int)CurrentAnimation - 2), SizeX, SizeY, GraphicsUnit.Pixel);
            else
                g.DrawImage(SpriteSheet, new Rectangle(new Point(Position.X - Flip * SizeX, Position.Y), new Size(Flip * SizeX * 2, SizeY * 2)),
                    50 * CurrentFrame, 37 * (int)CurrentAnimation, SizeX, SizeY, GraphicsUnit.Pixel);
        }

        public void SetAnimationConfiguration(AnimationType animation)
        {
            if (CurrentAnimation != animation && MapController.State == GameState.Normal)
            {
                CurrentAnimation = animation;
                CurrentFrame = 0;
                switch (animation)
                {
                    case AnimationType.Idle:
                        CurrentFrameLimit = IdleFrames;
                        break;
                    case AnimationType.Run:
                        CurrentFrameLimit = RunFrames;
                        break;
                    case AnimationType.Attack:
                        CurrentFrameLimit = AttackFrames;
                        break;
                    case AnimationType.Waiting:
                        CurrentFrameLimit = WaitingFrames;
                        break;
                }
            }
        }
    }
}
