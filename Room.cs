using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace MyGame.Domain
{
    public class Room
    {
        public TileType[,] Tiles { get; set; }
        public HashSet<Enemy> Enemies { get; set; }
        public Player Player { get; set; }
        public int Width { get { return Tiles.GetLength(0); } }
        public int Height { get { return Tiles.GetLength(1); } }

        public Room(int width, int height)
        {
            Enemies = new HashSet<Enemy>();
            Tiles = new TileType[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    Tiles[i, j] = TileType.Ground;
        }

        public void AddEnemy(Enemy enemy)
        {
            Enemies.Add(enemy);
        }

        public bool InRoom(Point point)
        {
            return point.X >= 0 && point.X < Tiles.GetLength(0) && point.Y >= 0 && point.Y < Tiles.GetLength(1); 
        }

        public bool IsHoleAt(Point point)
        {
            return Tiles[point.X, point.Y] == TileType.Hole;
        }

        public bool IsSpikeAt(Point point)
        {
            return Tiles[point.X, point.Y] == TileType.Spike;
        }

        public bool IsEnemyAt(Point point)
        {
            return Enemies.Select(enemy => enemy.Position).Contains(point);
        }
    }
}
