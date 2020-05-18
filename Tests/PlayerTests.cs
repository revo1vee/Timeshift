using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using Timeshift.Controllers;
using Timeshift.Domain;

namespace Timeshift.Tests
{
    [TestFixture]
    class PlayerTests
    {
        [SetUp]
        public void SetUp()
        {
            MapController.Map = new int[,]
            {
                { 2, 1, 1 },
                { 1, 1, 1 },
                { 1, 1, 13 }
            };
            MapController.MapHeight = 3;
            MapController.MapWidth = 3;
            MapController.Enemies = new HashSet<Enemy>();
        }

        [Test]
        public void PlayerMovementDependsOnDirection()
        {
            var player = new Player(new TilePoint(32, -32)) { MovementRange = 32 };
            player.MoveDirection = new TilePoint(1, 0);
            player.Move();
            player.Position.Should().Be(new TilePoint(32 + player.MovementRange, -32));
            player.MoveDirection = new TilePoint(-1, 0);
            player.Move();
            player.Position.Should().Be(new TilePoint(32, -32));
            player.MoveDirection = new TilePoint(0, 1);
            player.Move();
            player.Position.Should().Be(new TilePoint(32, -32 + player.MovementRange));
            player.MoveDirection = new TilePoint(0, -1);
            player.Move();
            player.Position.Should().Be(new TilePoint(32, -32));
        }

        [Test]
        public void PlayerCantGetOutsideOfRoom()
        {
            var player = new Player(new TilePoint(32, -32)) { MovementRange = 32 };
            player.MoveDirection = new TilePoint(1, 0);
            player.Move();
            player.Move();
            player.Position.Should().Be(new TilePoint(32 + player.MovementRange, -32));
        }

        [Test]
        public void PlayerCantMoveOnObjects()
        {
            var player = new Player(new TilePoint(32, -32));
            player.MoveDirection = new TilePoint(0, -1);
            player.Move();
            player.MoveDirection = new TilePoint(-1, 0);
            player.Move();
            player.Position.Should().Be(new TilePoint(32, -32 - player.MovementRange));
        }

        [Test]
        public void PlayerAttacksEnemies()
        {
            var player = new Player(new TilePoint(32, -32));
            var enemy = new SmallOrc(new TilePoint(64, -32)) { Health = 3 };
            MapController.Enemies.Add(enemy);
            player.Attack();
            enemy.Health.Should().Be(2);
        }

        [Test]
        public void PlayerCantAttackThroughWalls()
        {
            MapController.Map = new int[,]
            {
                { 2, 2, 2 },
                { 1, 2, 1 },
                { 2, 2, 2 }
            };
            var player = new Player(new TilePoint(0, -32));
            var enemy = new SmallOrc(new TilePoint(64, -32)) { Health = 3 };
            MapController.Enemies.Add(enemy);
            player.Attack();
            enemy.Health.Should().Be(3);
        }

        [Test]
        public void PlayerDashesCorrectly()
        {
            var player = new Player(new TilePoint(32, -32));
            player.TimeMoves.Push(new TilePoint(64, -32));
            player.TimeMoves.Push(new TilePoint(96, -32));
            player.TimeDash();
            player.Position.Should().Be(new TilePoint(64, -32));
        }
    }
}
