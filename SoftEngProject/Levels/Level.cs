using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject.Levels
{
    internal class Level
    {
        public int[,] Map { get; }
        public int TileSize { get; }
        public Vector2 HeroSpawn { get; }

        private readonly Texture2D tileTexture;
        public Rectangle WorldBounds =>
             new Rectangle(0, 0, Map.GetLength(1) * TileSize, Map.GetLength(0) * TileSize);

        public Level(int[,] map, int tileSize, Texture2D tileTexture, Vector2 heroSpawn)
        {
            this.Map = map;
            this.TileSize = tileSize;
            this.tileTexture = tileTexture;
            this.HeroSpawn = heroSpawn;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < Map.GetLength(0); y++)
            {
                for (int x = 0; x < Map.GetLength(1); x++)
                {
                    if (Map[y, x] == 1)
                    {
                        Vector2 position = new Vector2(x * TileSize, y * TileSize);
                        spriteBatch.Draw(tileTexture, position, Color.White);
                    }
                }
            }
        }
    }
}
