using System.Collections.Generic;
using System.Drawing;
using Timeshift.Controllers;

namespace Timeshift.Domain
{
    public class Entity
    {
        public int PosX;
        public int PosY;

        public int DirX;
        public int DirY;

        public int DashX;
        public int DashY;

        public bool IsMoving;
        public int MoveRange;
        public Direction Direction;

        public int Flip;

        public int PrevAnimation;
        public int CurrentAnimation;
        public int CurrentFrame;
        public int CurrentLimit;
        public int IdleFrames;
        public int RunFrames;
        public int AttackFrames;
        public int WaitingFrames;

        public int SizeX;
        public int SizeY;

        public Image SpriteSheet;

        public Queue<Direction> TimeMoves;

        public Entity(int x, int y, int idle, int run, int attack, int wait, Image sprites)
        {
            PosX = x;
            PosY = y;
            MoveRange = 32;
            IdleFrames = idle;
            RunFrames = run;
            AttackFrames = attack;
            WaitingFrames = wait;
            SpriteSheet = sprites;
            SizeX = 50;
            SizeY = 37;
            CurrentAnimation = 0;
            CurrentFrame = 0;
            CurrentLimit = IdleFrames;
            Flip = 1;
            TimeMoves = new Queue<Direction>();
        }

        public void Move()
        {
            if (MapController.State == GameState.Normal)
            {
                if (!PhysicsController.IsCollide(this, DirX, 0)) PosX += DirX;
                if (!PhysicsController.IsCollide(this, 0, DirY)) PosY += DirY;
            }
        }

        public void TimeDash()
        {
            while (TimeMoves.Count > 0)
            {
                switch (TimeMoves.Dequeue())
                {
                    case Direction.Up:
                        DirY = -MoveRange;
                        break;
                    case Direction.Left:
                        DirX = -MoveRange;
                        break;
                    case Direction.Down:
                        DirY = MoveRange;
                        break;
                    case Direction.Right:
                        DirX = MoveRange;
                        break;
                }
                Move();
                DirX = 0;
                DirY = 0;
            }
        }

        public void PlayAnimation(Graphics g)
        {
            if (CurrentAnimation == 6 && CurrentFrame == CurrentLimit - 1)
                if (IsMoving) SetAnimationConfiguration(1);
                else SetAnimationConfiguration(0);

            if (MapController.State == GameState.Normal)
            if (CurrentFrame < CurrentLimit - 1) CurrentFrame++;
            else if (CurrentAnimation == 1) CurrentFrame = 1;
            else CurrentFrame = 0;

            if (CurrentAnimation == 2)
                g.DrawImage(SpriteSheet, new Rectangle(new Point(PosX - Flip * SizeX, PosY), new Size(Flip * SizeX * 2, SizeY * 2)),
                50 * (CurrentFrame + 4), 37 * (CurrentAnimation - 2), SizeX, SizeY, GraphicsUnit.Pixel);
            else
                g.DrawImage(SpriteSheet, new Rectangle(new Point(PosX - Flip * SizeX, PosY), new Size(Flip * SizeX * 2, SizeY * 2)),
                    50 * CurrentFrame, 37 * CurrentAnimation, SizeX, SizeY, GraphicsUnit.Pixel);
        }

        public void SetAnimationConfiguration(int newAnimation)
        {
            if (CurrentAnimation != newAnimation && MapController.State == GameState.Normal)
            {
                CurrentFrame = 0;
                PrevAnimation = CurrentAnimation;
                CurrentAnimation = newAnimation;

                switch (newAnimation)
                {
                    case 0:
                        CurrentLimit = IdleFrames;
                        break;
                    case 1:
                        CurrentLimit = RunFrames;
                        break;
                    case 6:
                        CurrentLimit = AttackFrames;
                        break;
                    case 2:
                        CurrentLimit = WaitingFrames;
                        break;
                }
            }
        }
    }
}