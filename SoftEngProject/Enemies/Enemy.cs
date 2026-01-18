using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject.Enemies
{
    internal abstract class Enemy
    {
        public Vector2 Position { get; protected set; }

        //Basic stats
        public int MaxHealth { get; protected set; } = 3;
        public int Health { get; protected set; } = 3;
        public int ContactDamage { get; protected set; } = 3;

        public bool IsDead => Health <= 0;

        //Hitbox
        public int Width { get; protected set; } = 32;
        public int Height { get; protected set; } = 48;
        public Point HitboxOffset { get; protected set; } = new Point(0, 0);

        public Rectangle Hitbox =>
            new Rectangle(
                (int)Position.X + HitboxOffset.X,
                (int)Position.Y + HitboxOffset.Y,
                Width,
                Height);

        protected Vector2 velocity;

        protected Enemy(Vector2 spawn)
        {
            Position = spawn;
        }

        public virtual void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health < 0) Health = 0;
        }

        public abstract void Update(GameTime gameTime, Levels.Level level, Hero hero);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
