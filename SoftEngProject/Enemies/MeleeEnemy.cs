using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SoftEngProject.Animation;
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
        private readonly EnemyPhysicsComponent physics = new EnemyPhysicsComponent();
        private readonly Texture2D texture;

        private float speed = 1.5f;
        private int direction = -1;

        public MeleeEnemy(ContentManager content, Vector2 spawn) : base(spawn)
        {
            Texture2D idleTex = content.Load<Texture2D>("MeleeIdle");
            Texture2D runTex = content.Load<Texture2D>("MeleeRun");
            Texture2D attackTex = content.Load<Texture2D>("MeleeAttack_1");

            var idleFrames = AnimationHelpers.BuildHorizontalFramesFromSheetWidth(frameCount: 6, sheetWidth: 768, frameHeight: 128);
            var runFrames = AnimationHelpers.BuildHorizontalFramesFromSheetWidth(frameCount: 8, sheetWidth: 1024, frameHeight: 128);
            var attackFrames = AnimationHelpers.BuildHorizontalFramesFromSheetWidth(frameCount: 4, sheetWidth: 512, frameHeight: 128);

            Animator.AddAnimation("Idle", new SpriteAnimation(idleTex, idleFrames, frameSpeed: 0.12f, isLooping: true));
            Animator.AddAnimation("Run", new SpriteAnimation(runTex, runFrames, frameSpeed: 0.10f, isLooping: true));
            Animator.AddAnimation("Attack", new SpriteAnimation(attackTex, attackFrames, frameSpeed: 0.09f, isLooping: false));

            Animator.Play("Idle");


            Width = 28;
            Height = 50;
            HitboxOffset = new Point(40, 74);

            MaxHealth = 3;
            Health = 3;
            ContactDamage = 1;
        }

        public override void Update(GameTime gameTime, Level level, Hero hero)
        {
            physics.Velocity = physics.Velocity with { X = direction * speed };

            Position = physics.Update(Position, Hitbox, HitboxOffset, level, gameTime);
            IsGrounded = physics.IsGrounded;
            velocity = physics.Velocity;

            SpriteEffect = direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            if (velocity.X == 0)
                direction *= -1;

            if (Math.Abs(direction) > 0)
                Animator.Play("Run");
            else
                Animator.Play("Idle");

            Animator.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Animator.Draw(spriteBatch, Position, SpriteEffect, Scale = 1f);
        }
    }
}
