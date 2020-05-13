using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Timeshift.Controllers;
using Timeshift.Domain;
using Timeshift.Models;

namespace Timeshift.Views
{
    public partial class Timeshift : Form
    {
        public Image PlayerSheet;
        public Image EnemySheet;
        public Player Player;
        public Enemy Enemy;
        public Stopwatch Stopwatch;

        public Timeshift()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.FixedDialog;

            Timer.Interval = 100;
            Timer.Tick += new EventHandler(Update);

            KeyDown += new KeyEventHandler(OnPress);
            KeyUp += new KeyEventHandler(OnKeyUp);

            Initialize();
        }

        public void Initialize()
        {
            MapController.Initialize();
            InitializeEntities();
            Height = MapController.GetPixelHeight();
            Width = MapController.GetPixelWidth();
            Timer.Start();
            Stopwatch = Stopwatch.StartNew();
        }

        private void InitializeEntities()
        {
            PlayerSheet = new Bitmap(Path.Combine(new DirectoryInfo(
                            Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(), "Sprites\\Player.png"));

            Player = new Player(new Point(176, 144), PlayerModel.IdleFrames, PlayerModel.RunFrames, PlayerModel.AttackFrames,
                PlayerModel.WaitingFrames, PlayerSheet);

            EnemySheet = new Bitmap(Path.Combine(new DirectoryInfo(
                Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(), "Sprites\\Player.png"));

            Enemy = new Enemy(new Point(336, 144), PlayerModel.IdleFrames, PlayerModel.RunFrames, PlayerModel.AttackFrames,
                PlayerModel.WaitingFrames, PlayerSheet);

            MapController.Enemies.Add(Enemy);
        }

        public void OnPress(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    ChangePlayerDirection(Direction.Up);
                    break;
                case Keys.A:
                    ChangePlayerDirection(Direction.Left);
                    break;
                case Keys.S:
                    ChangePlayerDirection(Direction.Down);
                    break;
                case Keys.D:
                    ChangePlayerDirection(Direction.Right);
                    break;
                case Keys.Space:
                    Player.Attack();
                    Player.SetAnimationConfiguration(AnimationType.Attack);
                    break;
                case Keys.ShiftKey:
                    MapController.State = GameState.Frozen;
                    break;
                case Keys.R:
                    Initialize();
                    break;
            }
            Stopwatch.Restart();
        }

        private void ChangePlayerDirection(Direction direction)
        {
            if (MapController.State == GameState.Frozen)
                Player.TimeMoves.Enqueue(direction);
            else
            {
                if (direction == Direction.Left || direction == Direction.Right)
                {
                    Player.MoveDirection.X = (int)direction;
                    Player.Direction = direction;
                }
                else Player.MoveDirection.Y = (int)direction / 2; 
                Player.Flip = direction == Direction.Right ? 1 : (direction == Direction.Left ? -1 : Player.Flip);
                Player.IsMoving = true;
                Player.SetAnimationConfiguration(AnimationType.Run);
            }
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    Player.MoveDirection.Y = 0;
                    break;
                case Keys.A:
                    Player.MoveDirection.X = 0;
                    break;
                case Keys.S:
                    Player.MoveDirection.Y = 0;
                    break;
                case Keys.D:
                    Player.MoveDirection.X = 0;
                    break;
                case Keys.ShiftKey:
                    MapController.State = GameState.Normal;
                    Player.TimeDash();
                    break;
            }
            if (Player.MoveDirection == new Point(0, 0))
            {
                Player.IsMoving = false;
                if (Player.CurrentAnimation != AnimationType.Attack)
                Player.SetAnimationConfiguration(AnimationType.Idle);
            }
        }

        public void Update(object sender, EventArgs e)
        {
            if (Player.IsMoving) Player.Move();
            foreach (var enemy in MapController.Enemies)
            {
                
            }
            Invalidate();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            MapController.DrawMap(g);
            Player.PlayAnimation(g);
            foreach (var enemy in MapController.Enemies)
                enemy.PlayAnimation(g);

            if (Stopwatch.ElapsedMilliseconds >= 10000)
                Player.SetAnimationConfiguration(AnimationType.Waiting);

            DrawDebugInfo(g);
        }

        private void DrawDebugInfo(Graphics g)
        {
            g.DrawString(Player.Position.ToString(), new Font("Arial", 16),
                new SolidBrush(Color.White), new PointF(Width - 176, 16));
            g.DrawString((Stopwatch.ElapsedMilliseconds / 1000).ToString() + " s", new Font("Arial", 16),
                new SolidBrush(Color.White), new PointF(Width - 176, 48));
            foreach (var enemy in MapController.Enemies)
                g.DrawString(enemy.Health.ToString(), new Font("Arial", 16),
                new SolidBrush(Color.Red), new PointF(enemy.Position.X - 8, enemy.Position.Y - 8));
        }
    }
}