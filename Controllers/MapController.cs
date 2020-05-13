using System.Drawing;
using System.IO;
using Timeshift.Domain;

namespace Timeshift.Controllers
{
    public static class MapController
    {
        public const int MapHeight = 20;
        public const int MapWidth = 20;
        public static int[,] Map = new int[MapHeight, MapWidth];
        public static Image SpriteSheet;
        public const int TileSize = 32;
        public static GameState State;

        public static void Initialize()
        {
            Map = GetMap();
            SpriteSheet = new Bitmap(Path.Combine(new DirectoryInfo(
                Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(), "Sprites\\Tiles.png"));
        }

        public static int[,] GetMap()
        {
            return new int[,]{
                { 6,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,7 },
                { 3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4 },
                { 3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4 },
                { 3,1,1,10,1,1,1,1,1,1,1,1,1,1,1,1,10,1,1,4 },
                { 3,1,1,11,1,1,1,1,1,1,1,1,1,1,1,1,11,1,1,4 },
                { 3,1,1,12,1,1,1,1,1,1,1,1,1,1,1,1,12,1,1,4 },
                { 3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4 },
                { 3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4 },
                { 3,1,1,1,1,1,1,10,1,1,1,1,10,1,1,1,1,1,1,4 },
                { 3,1,1,1,1,1,1,11,1,1,1,1,11,1,1,1,1,1,1,4 },
                { 3,1,1,1,1,1,1,12,1,1,1,1,12,1,1,1,1,1,1,4 },
                { 3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4 },
                { 3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4 },
                { 3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4 },
                { 3,1,1,10,1,1,1,1,1,1,1,1,1,1,1,1,10,1,1,4 },
                { 3,1,1,11,1,1,1,1,1,1,1,1,1,1,1,1,11,1,1,4 },
                { 3,1,1,12,1,1,1,1,1,1,1,1,1,1,1,1,12,1,1,4 },
                { 3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4 },
                { 3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4 },
                { 8,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,9 },
            };
        }

        public static void SeedMap(Graphics g)
        {
            for (int i = 0; i < MapHeight; i++)
                for (int j = 0; j < MapWidth; j++)
                {
                    if (Map[i, j] == 10)
                    {
                        g.DrawImage(SpriteSheet, new Rectangle(new Point(j * TileSize, i * TileSize), new Size(TileSize, TileSize)),
                            80, 80, TileSize / 2, TileSize / 2, GraphicsUnit.Pixel);
                    }
                    else if (Map[i, j] == 11)
                    {
                        g.DrawImage(SpriteSheet, new Rectangle(new Point(j * TileSize, i * TileSize), new Size(TileSize, TileSize)),
                            80, 96, TileSize / 2, TileSize / 2, GraphicsUnit.Pixel);
                    }
                    else if (Map[i, j] == 12)
                    {
                        g.DrawImage(SpriteSheet, new Rectangle(new Point(j * TileSize, i * TileSize), new Size(TileSize, TileSize)),
                            80, 112, TileSize / 2, TileSize / 2, GraphicsUnit.Pixel);
                    }
                }
        }

        public static void DrawMap(Graphics g)
        {
            for (int i = 0; i < MapHeight; i++)
                for (int j = 0; j < MapWidth; j++)
                {
                    g.DrawImage(SpriteSheet, new Rectangle(new Point(j * TileSize, i * TileSize), new Size(TileSize, TileSize)),
                        16, 64, TileSize / 2, TileSize / 2, GraphicsUnit.Pixel);
                    if (Map[i, j] == 2)
                    {
                        g.DrawImage(SpriteSheet, new Rectangle(new Point(j * TileSize, i * TileSize), new Size(TileSize, TileSize)),
                            16, 16, TileSize / 2, TileSize / 2, GraphicsUnit.Pixel);
                    }
                    else if (Map[i, j] == 3)
                    {
                        g.DrawImage(SpriteSheet, new Rectangle(new Point(j * TileSize, i * TileSize), new Size(TileSize, TileSize)),
                            16, 128, TileSize / 2, TileSize / 2, GraphicsUnit.Pixel);
                    }
                    else if (Map[i, j] == 4)
                    {
                        g.DrawImage(SpriteSheet, new Rectangle(new Point(j * TileSize, i * TileSize), new Size(TileSize, TileSize)),
                            0, 128, TileSize / 2, TileSize / 2, GraphicsUnit.Pixel);
                    }
                    else if (Map[i, j] == 5)
                    {
                        g.DrawImage(SpriteSheet, new Rectangle(new Point(j * TileSize, i * TileSize), new Size(TileSize, TileSize)),
                            16, 0, TileSize / 2, TileSize / 2, GraphicsUnit.Pixel);
                    }
                    else if (Map[i, j] == 6)
                    {
                        g.DrawImage(SpriteSheet, new Rectangle(new Point(j * TileSize, i * TileSize), new Size(TileSize, TileSize)),
                            80, 144, TileSize / 2, TileSize / 2, GraphicsUnit.Pixel);
                    }
                    else if (Map[i, j] == 7)
                    {
                        g.DrawImage(SpriteSheet, new Rectangle(new Point(j * TileSize, i * TileSize), new Size(TileSize, TileSize)),
                            64, 144, TileSize / 2, TileSize / 2, GraphicsUnit.Pixel);
                    }
                    else if (Map[i, j] == 8)
                    {
                        g.DrawImage(SpriteSheet, new Rectangle(new Point(j * TileSize, i * TileSize), new Size(TileSize, TileSize)),
                            80, 128, TileSize / 2, TileSize / 2, GraphicsUnit.Pixel);
                    }
                    else if(Map[i, j] == 9)
                    {
                        g.DrawImage(SpriteSheet, new Rectangle(new Point(j * TileSize, i * TileSize), new Size(TileSize, TileSize)),
                            64, 128, TileSize / 2, TileSize / 2, GraphicsUnit.Pixel);
                    }
                }
            SeedMap(g);
        }

        public static bool InBounds(int x, int y)
        {
            return x >= 0 && x < MapWidth && y >= 0 && y < MapHeight;
        }

        public static int GetWidth()
        {
            return TileSize * MapWidth + 15;
        }

        public static int GetHeight()
        {
            return TileSize * MapHeight + 39;
        }
    }
}