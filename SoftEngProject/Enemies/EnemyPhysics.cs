using Microsoft.Xna.Framework;
using SoftEngProject.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject.Enemies
{
    internal static class EnemyPhysics
    {
        public static Vector2 MoveWithTileCollision(
            Vector2 spritePosition,
            ref Vector2 velocity,
            Rectangle hitbox,
            Point hitboxOffset,
            Level level)
        {
            Rectangle box = hitbox;

            box.X += (int)velocity.X;
            ResolveX(ref box, ref velocity, level);

            velocity.Y = 10f; 
            box.Y += (int)velocity.Y;
            ResolveY(ref box, ref velocity, level);

            return new Vector2(box.X - hitboxOffset.X, box.Y - hitboxOffset.Y);
        }

        public static bool WillWalkOffEdge(
            Vector2 spritePosition,
            Rectangle hitbox,
            Point hitboxOffset,
            int direction,
            Level level)
        {
            Rectangle box = hitbox;

            int lookAheadX = direction > 0 ? box.Right + 2 : box.Left - 2;
            int feetY = box.Bottom + 1;

            int tileSize = level.TileSize;
            int[,] map = level.Map;

            int tx = lookAheadX / tileSize;
            int ty = feetY / tileSize;

            if (tx < 0 || tx >= map.GetLength(1) || ty < 0 || ty >= map.GetLength(0))
                return true;

            return map[ty, tx] == 0;
        }

        private static void ResolveX(ref Rectangle box, ref Vector2 velocity, Level level)
        {
            foreach (var tile in GetSolidTilesAround(box, level))
            {
                if (!box.Intersects(tile)) continue;

                if (velocity.X > 0)
                    box.X = tile.Left - box.Width;
                else if (velocity.X < 0)
                    box.X = tile.Right;

                velocity.X = 0;
            }
        }

        private static void ResolveY(ref Rectangle box, ref Vector2 velocity, Level level)
        {
            foreach (var tile in GetSolidTilesAround(box, level))
            {
                if (!box.Intersects(tile)) continue;

                if (velocity.Y > 0)
                    box.Y = tile.Top - box.Height;
                else if (velocity.Y < 0)
                    box.Y = tile.Bottom;

                velocity.Y = 0;
            }
        }

        private static IEnumerable<Rectangle> GetSolidTilesAround(Rectangle box, Level level)
        {
            int tileSize = level.TileSize;
            int[,] map = level.Map;

            int leftTile = Clamp(box.Left / tileSize, 0, map.GetLength(1) - 1);
            int rightTile = Clamp((box.Right - 1) / tileSize, 0, map.GetLength(1) - 1);
            int topTile = Clamp(box.Top / tileSize, 0, map.GetLength(0) - 1);
            int bottomTile = Clamp((box.Bottom - 1) / tileSize, 0, map.GetLength(0) - 1);

            for (int y = topTile; y <= bottomTile; y++)
                for (int x = leftTile; x <= rightTile; x++)
                    if (map[y, x] == 1)
                        yield return new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);
        }

        private static int Clamp(int v, int min, int max)
        {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }
    }
}
