using Microsoft.Xna.Framework;
using SoftEngProject.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject
{
    internal class PhysicsComponent
    {
        private const float gravity = 0.5f;
        private const float maxFallSpeed = 15f;
        private const float jumpForce = -12f;
        //private const float friction = 0.8f;
        //private const float groundLevel =200f;

        public Vector2 velocity;

        public bool IsGrounded { get; private set; }

        public PhysicsComponent()
        {
            velocity = Vector2.Zero;
            IsGrounded = false;
        }

        public Vector2 Update(Vector2 currentPosition, Rectangle hitbox, Level level, GameTime gameTime)
        {
            //if (!IsGrounded)
            //{
            //    velocity.Y += gravity;
            //    if (velocity.Y > maxFallSpeed) velocity.Y = maxFallSpeed;
            //}

            //Vector2 nextPosition = currentPosition + velocity;

            //if (nextPosition.Y >= groundLevel)
            //{
            //    nextPosition.Y = groundLevel;
            //    velocity.Y = 0;
            //    IsGrounded = true;
            //}
            //else
            //{
            //    IsGrounded = false;
            //}

            //return nextPosition;

            //Gravity
            if (!IsGrounded)
            {
                velocity.Y += gravity;
                if (velocity.Y > maxFallSpeed) velocity.Y = maxFallSpeed;
            }

            IsGrounded = false;

            Vector2 nextPosition = currentPosition;
            //X Collision
            nextPosition.X += velocity.X;

            var xBox = new Rectangle((int)nextPosition.X, (int)currentPosition.Y, hitbox.Width, hitbox.Height);
            ResolveCollisionsX(ref nextPosition, xBox, level);

            //Y Collision
            nextPosition.Y += velocity.Y;

            var yBox = new Rectangle((int)nextPosition.X, (int)currentPosition.Y, hitbox.Width, hitbox.Height);
            ResolveCollisionsY(ref nextPosition, yBox, level);

            return nextPosition;
        }

        private void ResolveCollisionsX(ref Vector2 pos, Rectangle box, Level level)
        {
            foreach(var tileRect in GetSolidTilesAround(box, level))
            {
                if (box.Intersects(tileRect))
                {
                    if (velocity.X > 0)
                    {
                        pos.X = tileRect.Left - box.Width;
                    }
                    else if (velocity.X < 0)
                    {
                        pos.X = tileRect.Right - box.Width;
                    }

                    velocity.X = 0;
                    box.X = (int)pos.X;
                }
            }
        }

        private void ResolveCollisionsY(ref Vector2 pos, Rectangle box, Level level)
        {
            foreach (var tileRect in GetSolidTilesAround(box, level))
            {
                if (box.Intersects(tileRect))
                {
                    if (velocity.Y > 0)
                    {
                        pos.Y = tileRect.Top - box.Height;
                    }
                    else if (velocity.Y < 0)
                    {
                        pos.X = tileRect.Bottom;
                    }

                    velocity.Y = 0;
                    box.Y = (int)pos.Y;
                }
            }
        }

        private IEnumerable<Rectangle> GetSolidTilesAround(Rectangle box, Level level)
        {
            int tileSize = level.TileSize;
            int[,] map = level.Map;

           
            int leftTile = Clamp(box.Left / tileSize, 0, map.GetLength(1) - 1);
            int rightTile = Clamp(box.Right / tileSize, 0, map.GetLength(1) - 1);
            int topTile = Clamp(box.Top / tileSize, 0, map.GetLength(0) - 1);
            int bottomTile = Clamp(box.Bottom / tileSize, 0, map.GetLength(0) - 1);

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
            if (IsGrounded)
            {
                velocity.Y = jumpForce;
                IsGrounded = false;
            }
        }

        public void StopHorizontal()
        {
            velocity.X = 0;
        }
    }
}
