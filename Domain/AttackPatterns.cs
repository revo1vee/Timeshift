using System;
using Timeshift.Controllers;

namespace Timeshift.Domain
{
    public static class AttackPatterns
    {
        public static readonly Action<Enemy, Player> OnTouch = (enemy, player) =>
        {
            if (MapController.State == GameState.Normal)
            {
                if (MapController.GetPointFromCoordinates(new TilePoint(enemy.Position.X, enemy.Position.Y))
                    .Equals(MapController.GetPointFromCoordinates(new TilePoint(player.Position.X, player.Position.Y))))
                    player.TakeDamage(enemy.Damage);
            }
        };
    }
}
