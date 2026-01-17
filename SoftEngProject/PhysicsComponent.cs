using Microsoft.Xna.Framework;
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
        private const float friction = 0.8f;
        private const float groundLevel =200f;

        public Vector2 velocity;

        public bool IsGrounded { get; private set; }

        public PhysicsComponent()
        {
            velocity = Vector2.Zero;
            IsGrounded = false;
        }

        public Vector2 Update(Vector2 currentPosition, GameTime gameTime)
        {
            if (!IsGrounded)
            {
                velocity.Y += gravity;
                if (velocity.Y > maxFallSpeed) velocity.Y = maxFallSpeed;
            }

            Vector2 nextPosition = currentPosition + velocity;

            if (nextPosition.Y >= groundLevel)
            {
                nextPosition.Y = groundLevel;
                velocity.Y = 0;
                IsGrounded = true;
            }
            else
            {
                IsGrounded = false;
            }

            return nextPosition;
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
