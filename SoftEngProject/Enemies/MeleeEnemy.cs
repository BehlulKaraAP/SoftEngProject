using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftEngProject.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject.Enemies
{
    internal class MeleeEnemy : Enemy
    {
        private readonly Texture2D texture;

        private float speed = 1.5f;
        private int direction = -1;

        public MeleeEnemy(Texture2D texture, Vector2 spawn) : base(spawn)
        {
            this.texture = texture;

            Width = 28;
            Height = 46;
            HitboxOffset = new Point(2, 2);

            MaxHealth = 3;
            Health = 3;
            ContactDamage = 1;
        }

        public override void Update(GameTime gameTime, Level level, Hero hero)
        {
            velocity.X = direction * speed;

            Position = EnemyPhysics.MoveWithTileCollision(Position, ref velocity, Hitbox, HitboxOffset, level);

            if (velocity.X == 0)
                direction *= -1;

            if (EnemyPhysics.WillWalkOffEdge(Position, Hitbox, HitboxOffset, direction, level))
                direction *= -1;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}
