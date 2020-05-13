using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

            Height = MapController.GetHeight();
            Width = MapController.GetWidth();

            PlayerSheet = new Bitmap(Path.Combine(new DirectoryInfo(
                Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(), "Sprites\\Player.png"));

            Player = new Player(176, 144, PlayerModel.IdleFrames, PlayerModel.RunFrames, PlayerModel.AttackFrames,
                PlayerModel.WaitingFrames, PlayerSheet);

            EnemySheet = new Bitmap(Path.Combine(new DirectoryInfo(
                Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(), "Sprites\\Player.png"));

            Enemy = new Enemy(336, 144, PlayerModel.IdleFrames, PlayerModel.RunFrames, PlayerModel.AttackFrames,
                PlayerModel.WaitingFrames, PlayerSheet);

            MapController.Enemies.Add(Enemy);
            MapController.State = GameState.Normal;

            Timer.Start();
            Stopwatch = Stopwatch.StartNew();
        }

        public void OnPress(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    MovePlayer(Direction.Up);
                    break;
                case Keys.A:
                    MovePlayer(Direction.Left);
                    break;
                case Keys.S:
                    MovePlayer(Direction.Down);
                    break;
                case Keys.D:
                    MovePlayer(Direction.Right);
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

        private void MovePlayer(Direction direction)
        {
            if (MapController.State == GameState.Frozen)
                Player.TimeMoves.Enqueue(direction);
            else
            {
                Player.DirY = direction == Direction.Up ? -Player.MoveRange
                    : (direction == Direction.Down ? Player.MoveRange : Player.DirY);
                Player.DirX = direction == Direction.Left ? -Player.MoveRange
                    : (direction == Direction.Right ? Player.MoveRange : Player.DirX);
                Player.Flip = direction == Direction.Right ? 1
                    : (direction == Direction.Left ? -1 : Player.Flip);
                Player.Direction = direction;
                Player.IsMoving = true;
                Player.SetAnimationConfiguration(AnimationType.Run);
            }
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    Player.DirY = 0;
                    break;
                case Keys.A:
                    Player.DirX = 0;
                    break;
                case Keys.S:
                    Player.DirY = 0;
                    break;
                case Keys.D:
                    Player.DirX = 0;
                    break;
                case Keys.ShiftKey:
                    MapController.State = GameState.Normal;
                    Player.TimeDash();
                    break;
            }
            if (Player.DirX == 0 && Player.DirY == 0)
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
                if (enemy.IsMoving) enemy.Move();
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
            g.DrawString(Player.PosX.ToString() + " X " + Player.PosY.ToString() + " Y", new Font("Arial", 16),
                new SolidBrush(Color.White), new PointF(Width - 160, 16));
            g.DrawString((Stopwatch.ElapsedMilliseconds / 1000).ToString() + " s", new Font("Arial", 16),
                new SolidBrush(Color.White), new PointF(Width - 160, 48));
            foreach (var enemy in MapController.Enemies)
                g.DrawString((enemy.Health).ToString(), new Font("Arial", 16),
                new SolidBrush(Color.Red), new PointF(enemy.PosX - 8, enemy.PosY - 8));
        }
    }
}