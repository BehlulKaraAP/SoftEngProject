using Microsoft.Xna.Framework;
using SoftEngProject.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject
{
    internal class PhysicsComponent
    {
        private const float gravity = 0.5f;
        private const float maxFallSpeed = 15f;
        private const float jumpForce = -12f;
        private float coyoteTimer = 0f;
        private const float coyoteTime = 0.1f;

        public Vector2 velocity;

        public bool IsGrounded { get; private set; }

        public PhysicsComponent()
        {
            velocity = Vector2.Zero;
            IsGrounded = false;
        }

        public Vector2 Update(Vector2 currentPosition, Rectangle hitbox, Point hitboxOffset, Level level, GameTime gameTime)
        {
            //return nextPosition;
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            coyoteTimer = IsGrounded ? coyoteTime : MathHelper.Max(0f, coyoteTimer - dt);

            //Gravity
            if (!IsGrounded)
            {
                velocity.Y += gravity;
                if (velocity.Y > maxFallSpeed) velocity.Y = maxFallSpeed;
            }

            IsGrounded = false;

            Rectangle movedBox = hitbox;

            movedBox.X += (int)velocity.X;
            ResolveCollisionsX(ref movedBox, level);

            movedBox.Y += (int)velocity.Y;
            ResolveCollisionsY(ref movedBox, level);

            return new Vector2(movedBox.X - hitboxOffset.X, movedBox.Y - hitboxOffset.Y);
        }

        private void ResolveCollisionsX(ref Rectangle box, Level level)
        {
            foreach(var tileRect in GetSolidTilesAround(box, level))
            {
                if (!box.Intersects(tileRect)) continue;

                if (velocity.X > 0)
                {
                    box.X = tileRect.Left - box.Width;
                }
                else if (velocity.X < 0)
                {
                    box.X = tileRect.Right;
                }

                velocity.X = 0;
            }
        }

        private void ResolveCollisionsY(ref Rectangle box, Level level)
        {
            foreach (var tileRect in GetSolidTilesAround(box, level))
            {
                if (!box.Intersects(tileRect)) continue;
                if (velocity.Y > 0)
                {
                    box.Y = tileRect.Top - box.Height;
                    IsGrounded = true;
                }
                else if (velocity.Y < 0)
                {
                    box.Y = tileRect.Bottom;
                }

                velocity.Y = 0;

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
            {
                for (int x = leftTile; x <= rightTile; x++)
                {
                    if (map[y, x] == 1)
                    {
                        yield return new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);
                    }
                }
            }
        }

        private int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public void Jump()
        {
            if (coyoteTimer > 0f)
            {
                velocity.Y = jumpForce;
                IsGrounded = false;
                coyoteTimer = 0f;
            }
        }

        public void StopHorizontal()
        {
            velocity.X = 0;
        }
    }
}
