using Timeshift.Domain;

namespace Timeshift.Controllers
{
    public static class PhysicsController
    {
        public static bool IsCollide(TilePoint point)
        {
            var pointToCheck = MapController.GetPointFromCoordinates(point);

            return !MapController.InBounds(pointToCheck.X, pointToCheck.Y)
            || MapController.Map[pointToCheck.Y, pointToCheck.X] != 1
            && MapController.Map[pointToCheck.Y, pointToCheck.X] != 13;
        }

        public static bool IsSpikeAt(TilePoint point)
        {
            var pointToCheck = MapController.GetPointFromCoordinates(point);

            return MapController.Map[pointToCheck.Y, pointToCheck.X] == 13;
        }
    }
}