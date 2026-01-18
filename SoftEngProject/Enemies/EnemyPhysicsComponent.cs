using Microsoft.Xna.Framework;
using SoftEngProject.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject.Enemies
{
    internal class EnemyPhysicsComponent
    {
        private const float gravity = 0.5f;
        private const float maxFallSpeed = 15f;

        public Vector2 Velocity;
        public bool IsGrounded { get; private set; }

        public Vector2 Update(Vector2 spritePosition, Rectangle hitbox, Point hitboxOffset, Level level, GameTime gameTime)
        {
            // gravity
            if (!IsGrounded)
            {
                Velocity.Y += gravity;
                if (Velocity.Y > maxFallSpeed) Velocity.Y = maxFallSpeed;
            }

            IsGrounded = false;

            Rectangle movedBox = hitbox;

            movedBox.X += (int)System.Math.Round(Velocity.X);
            ResolveCollisionsX(ref movedBox, level);

            movedBox.Y += (int)System.Math.Round(Velocity.Y);
            ResolveCollisionsY(ref movedBox, level);

            var bounds = level.WorldBounds;
            if (movedBox.Left < bounds.Left)
            {
                movedBox.X = bounds.Left;
                Velocity.X = 0;
            }
            if (movedBox.Right > bounds.Right)
            {
                movedBox.X = bounds.Right - movedBox.Width;
                Velocity.X = 0;
            }

            return new Vector2(movedBox.X - hitboxOffset.X, movedBox.Y - hitboxOffset.Y);
        }

        private void ResolveCollisionsX(ref Rectangle box, Level level)
        {
            foreach (var tileRect in GetSolidTilesAround(box, level))
            {
                if (!box.Intersects(tileRect)) continue;

                if (Velocity.X > 0) box.X = tileRect.Left - box.Width;
                else if (Velocity.X < 0) box.X = tileRect.Right;

                Velocity.X = 0;
            }
        }

        private void ResolveCollisionsY(ref Rectangle box, Level level)
        {
            foreach (var tileRect in GetSolidTilesAround(box, level))
            {
                if (!box.Intersects(tileRect)) continue;

                if (Velocity.Y > 0)
                {
                    box.Y = tileRect.Top - box.Height;
                    IsGrounded = true;
                }
                else if (Velocity.Y < 0)
                {
                    box.Y = tileRect.Bottom;
                }

                Velocity.Y = 0;
            }
        }

        private IEnumerable<Rectangle> GetSolidTilesAround(Rectangle box, Level level)
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

        private int Clamp(int v, int min, int max)
        {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }
    }
}
