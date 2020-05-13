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

        public int Health;

        public bool IsMoving;
        public int MoveRange;
        public Direction Direction;

        public int Flip;

        public AnimationType PrevAnimation;
        public AnimationType CurrentAnimation;
        public int CurrentFrame;
        public int CurrentLimit;
        public int IdleFrames;
        public int RunFrames;
        public int AttackFrames;
        public int WaitingFrames;

        public int SizeX;
        public int SizeY;

        public Image SpriteSheet;

        public void Move()
        {
            if (MapController.State == GameState.Normal)
            {
                if (!PhysicsController.IsCollide(this, DirX, 0)) PosX += DirX;
                if (!PhysicsController.IsCollide(this, 0, DirY)) PosY += DirY;
            }
        }

        public void PlayAnimation(Graphics g)
        {
            if (CurrentAnimation == AnimationType.Attack && CurrentFrame == CurrentLimit - 1)
                if (IsMoving) SetAnimationConfiguration(AnimationType.Run);
                else SetAnimationConfiguration(AnimationType.Idle);

            if (MapController.State == GameState.Normal)
            if (CurrentFrame < CurrentLimit - 1) CurrentFrame++;
            else if (CurrentAnimation == AnimationType.Run) CurrentFrame = 1;
            else CurrentFrame = 0;

            if (CurrentAnimation == AnimationType.Waiting)
                g.DrawImage(SpriteSheet, new Rectangle(new Point(PosX - Flip * SizeX, PosY), new Size(Flip * SizeX * 2, SizeY * 2)),
                50 * (CurrentFrame + 4), 37 * ((int)CurrentAnimation - 2), SizeX, SizeY, GraphicsUnit.Pixel);
            else
                g.DrawImage(SpriteSheet, new Rectangle(new Point(PosX - Flip * SizeX, PosY), new Size(Flip * SizeX * 2, SizeY * 2)),
                    50 * CurrentFrame, 37 * (int)CurrentAnimation, SizeX, SizeY, GraphicsUnit.Pixel);
        }

        public void SetAnimationConfiguration(AnimationType newAnimation)
        {
            if (CurrentAnimation != newAnimation && MapController.State == GameState.Normal)
            {
                CurrentFrame = 0;
                PrevAnimation = CurrentAnimation;
                CurrentAnimation = newAnimation;

                switch (newAnimation)
                {
                    case AnimationType.Idle:
                        CurrentLimit = IdleFrames;
                        break;
                    case AnimationType.Run:
                        CurrentLimit = RunFrames;
                        break;
                    case AnimationType.Attack:
                        CurrentLimit = AttackFrames;
                        break;
                    case AnimationType.Waiting:
                        CurrentLimit = WaitingFrames;
                        break;
                }
            }
        }
    }
}