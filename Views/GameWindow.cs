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
        public Image TilesSheet;
        public Player Player;
        public Stopwatch WaitingAnimationTrigger;

        public Timeshift()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.FixedDialog;

            Timer.Interval = 75;
            Timer.Tick += new EventHandler(Update);

            KeyDown += new KeyEventHandler(OnPress);
            KeyUp += new KeyEventHandler(OnKeyUp);

            Initialize();
        }

        public void Initialize()
        {
            MapController.Initialize();
            MapController.State = GameState.Normal;
            InitializeEntities();
            Height = MapController.GetPixelHeight();
            Width = MapController.GetPixelWidth();
            Timer.Start();
            WaitingAnimationTrigger = Stopwatch.StartNew();
        }

        private void InitializeEntities()
        {
            PlayerSheet = new Bitmap(Path.Combine(new DirectoryInfo(
                            Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(), "Sprites\\Player.png"));

            TilesSheet = new Bitmap(Path.Combine(new DirectoryInfo(
                Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(), "Sprites\\Tiles.png"));

            Player = new Player(new TilePoint(176, 144), PlayerModel.IdleFrames, PlayerModel.RunFrames, PlayerModel.AttackFrames,
                PlayerModel.WaitingFrames, PlayerSheet);

            MapController.SpawnEnemies(TilesSheet);
        }

        public void OnPress(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    if (MapController.State == GameState.Frozen)
                        AddTimeMove(0,-Player.MovementRange * 4);
                    else ChangePlayerDirection(Direction.Up);
                    break;
                case Keys.A:
                    if (MapController.State == GameState.Frozen)
                        AddTimeMove(-Player.MovementRange * 4, 0);
                    else ChangePlayerDirection(Direction.Left);
                    break;
                case Keys.S:
                    if (MapController.State == GameState.Frozen)
                        AddTimeMove(0, Player.MovementRange * 4);
                    else ChangePlayerDirection(Direction.Down);
                    break;
                case Keys.D:
                    if (MapController.State == GameState.Frozen)
                        AddTimeMove(Player.MovementRange * 4, 0);
                    else ChangePlayerDirection(Direction.Right);
                    break;
                case Keys.Space:
                    Player.Attack();
                    Player.SetAnimationConfiguration(AnimationType.Attack);
                    break;
                case Keys.ShiftKey:
                    MapController.State = GameState.Frozen;
                    Player.FreezedTime.Start();
                    MapController.LastPlayerPosition = new TilePoint(Player.Position.X, Player.Position.Y);
                    break;
                case Keys.R:
                    Initialize();
                    break;
            }
            WaitingAnimationTrigger.Restart();
        }

        private void AddTimeMove(int dx, int dy)
        {
            Player.TimeMoves.Push(new TilePoint(Player.TimeMoves.Peek().X + dx, Player.TimeMoves.Peek().Y + dy));
        }

        private void ChangePlayerDirection(Direction direction)
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
                    Player.FreezedTime.Reset();
                    Player.TimeDash();
                    break;
            }
            if (Player.MoveDirection.Equals(new TilePoint()))
            {
                Player.IsMoving = false;
                if (Player.CurrentAnimation != AnimationType.Attack)
                Player.SetAnimationConfiguration(AnimationType.Idle);
            }
        }

        public void Update(object sender, EventArgs e)
        {
            if (Player.IsMoving) Player.Move();
            if (PhysicsController.IsSpikeAt(Player.Position)) Player.TakeDamage(0.5);
            if (Player.FreezedTime.ElapsedMilliseconds >= 5000)
            {
                Player.TimeDash();
                MapController.State = GameState.Normal;
                Player.FreezedTime.Reset();
            }
            if (Player.IFrames.ElapsedMilliseconds >= 1000) Player.IFrames.Reset();
            if (Player.TimeAfterDash.ElapsedMilliseconds >= 1000) Player.TimeAfterDash.Reset();
            foreach (var enemy in MapController.Enemies)
            {
                if (enemy.Defeated) continue;
                else
                {
                    if (Player.TimeAfterDash.IsRunning && Player.TimeAfterDash.ElapsedMilliseconds <= 1000)
                        enemy.SetDirection(MapController.LastPlayerPosition);
                    else enemy.SetDirection(Player.Position);
                }
                enemy.Move();
                enemy.Attack(Player);
            }
            Invalidate();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            MapController.DrawMap(g);
            Player.PlayAnimation(g);
            foreach (var enemy in MapController.Enemies)
            {
                if (enemy.Defeated) continue;
                enemy.PlayAnimation(g);
            }

            if (WaitingAnimationTrigger.ElapsedMilliseconds >= 10000)
                Player.SetAnimationConfiguration(AnimationType.Waiting);

            ShowPlayerHealth(g);
            //DrawDebugInfo(g);
        }

        private void ShowPlayerHealth(Graphics g)
        {
            for (int i = 1; i <= Player.InitialHealth; i++)
                DrawHeart(g, 16 * i + 16 * (i - 1), 16, i);
        }

        private void DrawHeart(Graphics g, int x, int y, int heartPos)
        {
            if (Player.Health >= heartPos) DrawFullHeart(g, new Point(x, y));
            else if (Player.Health == heartPos - 0.5) DrawDamagedHeart(g, new Point(x, y));
            else DrawEmptyHeart(g, new Point(x, y));
        }

        private void DrawFullHeart(Graphics g, Point point)
        {
            g.DrawImage(TilesSheet, new Rectangle(point, new Size(32, 32)), 18 * 16, 16 * 16, 16, 16, GraphicsUnit.Pixel);
        }

        private void DrawDamagedHeart(Graphics g, Point point)
        {
            g.DrawImage(TilesSheet, new Rectangle(point, new Size(32, 32)), 19 * 16, 16 * 16, 16, 16, GraphicsUnit.Pixel);
        }

        private void DrawEmptyHeart(Graphics g, Point point)
        {
            g.DrawImage(TilesSheet, new Rectangle(point, new Size(32, 32)), 20 * 16, 16 * 16, 16, 16, GraphicsUnit.Pixel);
        }

        private void DrawDebugInfo(Graphics g)
        {
            g.DrawString(Player.Position.ToString(), new Font("Arial", 16),
                new SolidBrush(Color.White), new PointF(Width - 176, 16));
            g.DrawString((WaitingAnimationTrigger.ElapsedMilliseconds / 1000).ToString() + " s", new Font("Arial", 16),
                new SolidBrush(Color.White), new PointF(Width - 176, 48));
            foreach (var enemy in MapController.Enemies)
                if (!enemy.Defeated)
                g.DrawString(enemy.Health.ToString(), new Font("Arial", 16),
                new SolidBrush(Color.Red), new PointF(enemy.Position.X - 8, enemy.Position.Y - 8));
        }
    }
}