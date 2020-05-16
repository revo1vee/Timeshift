using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Timeshift.Domain;
using Timeshift.Models;

namespace Timeshift.Controllers
{
    public static class MapController
    {
        public const int MapHeight = 20;
        public const int MapWidth = 20;
        public static int[,] Map = new int[MapHeight, MapWidth];
        public static Image SpriteSheet;
        public static Dictionary<int, TilePoint> TilesSprites;
        public const int TileSize = 32;
        public static GameState State;
        public static HashSet<Enemy> Enemies;
        public static TilePoint LastPlayerPosition;

        public static void Initialize()
        {
            Map = GetMap();
            SpriteSheet = new Bitmap(Path.Combine(new DirectoryInfo(
                Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(), "Sprites\\Tiles.png"));
            Enemies = new HashSet<Enemy>();
            TilesSprites = new Dictionary<int, TilePoint>();
            SetTilesSprites();
        }

        public static void SpawnEnemies(Image spriteSheet)
        {
            Enemies.Add(new SmallOrc(new TilePoint(352, 144), EnemyModel.OrcFrames, spriteSheet));
            Enemies.Add(new MaskedOrc(new TilePoint(352, 352), EnemyModel.OrcFrames, spriteSheet));
        }

        public static void SetTilesSprites()
        {
            TilesSprites[1] = new TilePoint(16, 64);
            TilesSprites[2] = new TilePoint(16, 16);
            TilesSprites[3] = new TilePoint(16, 128);
            TilesSprites[4] = new TilePoint(0, 128);
            TilesSprites[5] = new TilePoint(16, 0);
            TilesSprites[6] = new TilePoint(80, 144);
            TilesSprites[7] = new TilePoint(64, 144);
            TilesSprites[8] = new TilePoint(80, 128);
            TilesSprites[9] = new TilePoint(64, 128);
            TilesSprites[10] = new TilePoint(80, 80);
            TilesSprites[11] = new TilePoint(80, 96);
            TilesSprites[12] = new TilePoint(80, 112);
            TilesSprites[13] = new TilePoint(16*4, 16*11);
        }

        public static int[,] GetMap()
        {
            return new int[,]{
                { 6,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,7 },
                { 3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4 },
                { 3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4 },
                { 3,1,1,10,13,13,13,13,13,13,13,13,13,13,13,13,10,1,1,4 },
                { 3,1,1,11,1,1,1,1,1,1,1,1,1,1,1,1,11,1,1,4 },
                { 3,1,1,12,1,1,1,1,1,1,1,1,1,1,1,1,12,1,1,4 },
                { 3,1,1,13,1,1,1,1,1,1,1,1,1,1,1,1,13,1,1,4 },
                { 3,1,1,13,1,1,1,1,1,1,1,1,1,1,1,1,13,1,1,4 },
                { 3,1,1,13,1,1,1,10,1,1,1,1,10,1,1,1,13,1,1,4 },
                { 3,1,1,13,1,1,1,11,13,13,13,13,11,1,1,1,13,1,1,4 },
                { 3,1,1,13,1,1,1,12,1,1,1,1,12,1,1,1,13,1,1,4 },
                { 3,1,1,13,1,1,1,1,1,1,1,1,1,1,1,1,13,1,1,4 },
                { 3,1,1,13,1,1,1,1,1,1,1,1,1,1,1,1,13,1,1,4 },
                { 3,1,1,13,1,1,1,1,1,1,1,1,1,1,1,1,13,1,1,4 },
                { 3,1,1,10,1,1,1,1,1,1,1,1,1,1,1,1,10,1,1,4 },
                { 3,1,1,11,1,1,1,1,1,1,1,1,1,1,1,1,11,1,1,4 },
                { 3,1,1,12,13,13,13,13,13,13,13,13,13,13,13,13,12,1,1,4 },
                { 3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4 },
                { 3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4 },
                { 8,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,9 },
            };
        }

        public static void DrawMap(Graphics g)
        {
            for (int i = 0; i < MapHeight; i++)
                for (int j = 0; j < MapWidth; j++)
                {
                    g.DrawImage(SpriteSheet, new Rectangle(new Point(j * TileSize, i * TileSize), new Size(TileSize, TileSize)),
                        TilesSprites[1].X, TilesSprites[1].Y, TileSize / 2, TileSize / 2, GraphicsUnit.Pixel);
                    g.DrawImage(SpriteSheet, new Rectangle(new Point(j * TileSize, i * TileSize), new Size(TileSize, TileSize)),
                            TilesSprites[Map[i,j]].X, TilesSprites[Map[i, j]].Y, TileSize / 2, TileSize / 2, GraphicsUnit.Pixel);
                }
            SeedMap(g);
        }

        public static void SeedMap(Graphics g)
        {
            for (int i = 0; i < MapHeight; i++)
                for (int j = 0; j < MapWidth; j++)
                    g.DrawImage(SpriteSheet, new Rectangle(new Point(j * TileSize, i * TileSize), new Size(TileSize, TileSize)),
                            TilesSprites[Map[i, j]].X, TilesSprites[Map[i, j]].Y, TileSize / 2, TileSize / 2, GraphicsUnit.Pixel);
        }

        public static TilePoint GetPointFromCoordinates(TilePoint point)
        {
            return new TilePoint(point.X / TileSize, (point.Y / TileSize) + 2);
        }

        public static bool InBounds(int x, int y)
        {
            return x >= 0 && x < MapWidth && y >= 0 && y < MapHeight;
        }

        public static int GetPixelWidth()
        {
            return TileSize * MapWidth + 15;
        }

        public static int GetPixelHeight()
        {
            return TileSize * MapHeight + 39;
        }
    }
}