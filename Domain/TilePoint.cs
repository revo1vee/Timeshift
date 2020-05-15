using System;
using System.Drawing;
using Timeshift.Controllers;

namespace Timeshift.Domain
{
    public class TilePoint
    {
        public int X;
        public int Y;

        public TilePoint()
        {
            X = 0;
            Y = 0;
        }

        public TilePoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            var point = obj as TilePoint;
            return point.X == X && point.Y == Y;
        }

        public override int GetHashCode()
        {
            return X * 397 + Y;
        }

        public override string ToString()
        {
            return X.ToString() + " X " + Y.ToString() + " Y";
        }

        public static TilePoint operator +(TilePoint point, Size size)
            => new TilePoint(point.X + size.Width, point.Y + size.Height);
    }
}
