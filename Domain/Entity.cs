using System.Collections.Generic;
using System.Drawing;
using Timeshift.Controllers;

namespace Timeshift.Domain
{
    public class Entity
    {
        public TilePoint Position;
        public int Health;

        public bool IsMoving;
        public int MovementRange;
        public TilePoint MoveDirection;
        public Direction Direction;

        public Image SpriteSheet;
        public AnimationType PrevAnimation;
        public AnimationType CurrentAnimation;
        public int CurrentFrame;
        public int CurrentFrameLimit;
        public int IdleFrames;
        public int RunFrames;
        public int AttackFrames;
        public int WaitingFrames;
        public int Flip;
        public int SizeX;
        public int SizeY;

        public void Move()
        {
            if (MapController.State == GameState.Normal)
            {
                if (!PhysicsController.IsCollide(new TilePoint(Position.X + MoveDirection.X * MovementRange + 15 * MoveDirection.X, Position.Y))) 
                    Position.X += MoveDirection.X * MovementRange;
                if (!PhysicsController.IsCollide(new TilePoint(Position.X, Position.Y + MoveDirection.Y * MovementRange)))
                    Position.Y += MoveDirection.Y * MovementRange;
            }
        }

        public static IEnumerable<Size> PossibleDirections()
        {
            for (var dy = -1; dy <= 1; dy++)
                for (var dx = -1; dx <= 1; dx++)
                    if (dx == 0 || dy == 0) yield return new Size(dx, dy);
        }

        public void PlayAnimation(Graphics g)
        {
            CheckAttackAnimationFrame();
            SetFrame();
            DrawEntity(g);
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

        private void DrawEntity(Graphics g)
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