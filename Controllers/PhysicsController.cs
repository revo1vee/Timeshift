using Timeshift.Domain;

namespace Timeshift.Controllers
{
    public static class PhysicsController
    {
        public static bool IsCollide(Entity entity, int dirX, int dirY)
        {
            if (dirX != 0) dirX /= 32;
            if (dirY != 0) dirY /= 32;
            dirX = entity.PosX / MapController.TileSize + dirX;
            dirY = (entity.PosY + 48) / MapController.TileSize + dirY;

            return !MapController.InBounds(dirX, dirY) || MapController.Map[dirY, dirX] != 1;
        }
    }
}