using Timeshift.Domain;

namespace Timeshift.Controllers
{
    public static class PhysicsController
    {
        public static bool IsCollide(Entity entity, int dirX, int dirY)
        {
            if (dirX != 0) dirX /= entity.MovementRange;
            if (dirY != 0) dirY /= entity.MovementRange;
            dirX = entity.Position.X / MapController.TileSize + dirX;
            dirY = (entity.Position.Y + (int)(entity.MovementRange * 1.5)) / MapController.TileSize + dirY;

            return !MapController.InBounds(dirX, dirY) || MapController.Map[dirY, dirX] != 1;
        }
    }
}