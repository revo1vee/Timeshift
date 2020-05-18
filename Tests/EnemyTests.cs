using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using Timeshift.Controllers;
using Timeshift.Domain;

namespace Timeshift.Tests
{
    [TestFixture]
    class EnemyTests
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
        public void EnemyDirectionDependsOnPlayerPosition()
        {
            var player = new Player(new TilePoint(0, -32)) { MovementRange = 32 };
            var enemy = new SmallOrc(new TilePoint(32, -32));
            enemy.SetDirection(player.Position);
            enemy.Direction.Should().Be(Direction.Left);
            player.MoveDirection = new TilePoint(1, 0);
            player.Move();
            player.Move();
            enemy.SetDirection(player.Position);
            enemy.Direction.Should().Be(Direction.Right);
        }

        [Test]
        public void EnemiesCantStandOnTheSamePoint()
        {
            var orc = new SmallOrc(new TilePoint(0, -32)) { MovementRange = 32 };
            var maskedOrc = new MaskedOrc(new TilePoint(32, -32));
            MapController.Enemies.Add(orc);
            MapController.Enemies.Add(maskedOrc);
            orc.SetDirection(maskedOrc.Position);
            orc.Move();
            orc.Position.Should().Be(new TilePoint(0, -32));
        }

        [Test]
        public void EnemyAttacksPlayerCorrectly()
        {
            var player = new Player(new TilePoint(0, -32)) { Health = 3 };
            var enemy = new SmallOrc(new TilePoint(32, -32)) { Damage = 1, MovementRange = 32 };
            enemy.Attack(player);
            player.Health.Should().Be(3);
            enemy.SetDirection(player.Position);
            enemy.Move();
            enemy.Attack(player);
            player.Health.Should().Be(3 - enemy.Damage);
        }
    }
}
