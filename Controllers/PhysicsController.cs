using System.Drawing;
using Timeshift.Domain;

namespace Timeshift.Controllers
{
    public static class PhysicsController
    {
        public static bool IsCollide(TilePoint point)
        {
            var pointToMove = MapController.GetPointFromCoordinates(point);

            return !MapController.InBounds(pointToMove.X, pointToMove.Y) 
                || MapController.Map[pointToMove.Y, pointToMove.X] != 1;
        }
    }
}