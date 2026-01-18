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

        private int direction = -1;

        private float patrolSpeed = 1.2f;
        private float chaseSpeed = 2.2f;

        private float detectRange = 260f;
        private float loseRange = 320f;

        private bool isChasing = false;

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
            float dx = hero.Position.X - Position.X;
            float absDx = Math.Abs(dx);

            if (!isChasing)
            {
                if (absDx <= detectRange)
                {
                    isChasing = true;
                }
            }
            else
            {
                if (absDx >= loseRange)
                {
                    isChasing = false;
                }
            }

            float desiredXVel;

            if (isChasing)
            {
                direction = dx < 0 ? -1 : 1;
                desiredXVel = direction * chaseSpeed;

                Animator.Play("Run");
            }
            else
            {
                desiredXVel = direction * patrolSpeed;

                Animator.Play("Run");
            }

            physics.Velocity = physics.Velocity with { X = desiredXVel };

            Position = physics.Update(Position, Hitbox, HitboxOffset, level, gameTime);
            IsGrounded = physics.IsGrounded;
            velocity = physics.Velocity;

            if (!isChasing && velocity.X == 0)
            {
                direction *= -1;
            }

            SpriteEffect = direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Animator.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Animator.Draw(spriteBatch, Position, SpriteEffect, Scale = 1f);
        }
    }
}
