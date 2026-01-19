using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SoftEngProject.Animation;
using SoftEngProject.Levels;
using SoftEngProject.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftEngProject.Enemies
{
    internal class ArcherEnemy : Enemy
    {
        private readonly EnemyPhysicsComponent physics = new EnemyPhysicsComponent();

        private readonly EnemyManager enemyManager;
        private readonly Texture2D arrowTexture;

        private float shootRange = 320f;
        private float shootCooldown = 1.2f;
        private float shootTimer = 0f;

        private bool isShooting = false;

        private float arrowSpeed = 300f;
        private float speed = 1.2f;

        private int direction = -1;
        private int patrolDirection = -1;


        public ArcherEnemy(ContentManager content, EnemyManager enemyManager, Texture2D arrowTexture, Vector2 spawn) : base(spawn)
        {
            Texture2D idleTex = content.Load<Texture2D>("ArcherIdle");
            Texture2D walkTex = content.Load<Texture2D>("ArcherWalk");
            Texture2D shotTex = content.Load<Texture2D>("ArcherShot");

            this.enemyManager = enemyManager;
            this.arrowTexture = arrowTexture;

            var idleFrames = AnimationHelpers.BuildHorizontalFramesFromSheetWidth(frameCount: 9, sheetWidth: 1152, frameHeight: 128);
            var walkFrames = AnimationHelpers.BuildHorizontalFramesFromSheetWidth(frameCount: 8, sheetWidth: 1024, frameHeight: 128);
            var shotFrames = AnimationHelpers.BuildHorizontalFramesFromSheetWidth(frameCount: 14, sheetWidth: 1792, frameHeight: 128);

            Animator.AddAnimation("Idle", new SpriteAnimation(idleTex, idleFrames, frameSpeed: 0.12f, isLooping: true));
            Animator.AddAnimation("Walk", new SpriteAnimation(walkTex, walkFrames, frameSpeed: 0.10f, isLooping: true));
            Animator.AddAnimation("Shot", new SpriteAnimation(shotTex, shotFrames, frameSpeed: 0.1f, isLooping: false));

            Animator.Play("Idle");

            Width = 22;
            Height = 46;
            HitboxOffset = new Point(60, 74);

            MaxHealth = 2;
            Health = 2;
        }

        public override void Update(GameTime gameTime, Level level, Hero hero)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            shootTimer -= dt;

            float dxToHero = hero.Hitbox.Center.X - Hitbox.Center.X;
            float absDx = Math.Abs(dxToHero);

            bool inRange = absDx <= shootRange;

            if (inRange)
            {
                direction = dxToHero < 0 ? -1 : 1;
                physics.Velocity = physics.Velocity with { X = 0 };

                Position = physics.Update(Position, Hitbox, HitboxOffset, level, gameTime);
                IsGrounded = physics.IsGrounded;
                velocity = physics.Velocity;

                SpriteEffect = direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                if (isShooting)
                {
                    Animator.Play("Shot");

                    if (Animator.IsAnimationComplete())
                    {
                        isShooting = false;
                    }

                    Animator.Update(gameTime);
                    return;
                }

                if (shootTimer <= 0f)
                {
                    isShooting = true;
                    Animator.Play("Shot");

                    Vector2 spawnPos = new Vector2(
                        direction == 1 ? Hitbox.Right : (Hitbox.Left - arrowTexture.Width),
                        Hitbox.Top + 20
                    );

                    Vector2 vel = new Vector2(direction * arrowSpeed, 0f);

                    enemyManager.AddArrow(new ArrowProjectile(arrowTexture, spawnPos, vel));
                    shootTimer = shootCooldown;
                }

                Animator.Update(gameTime);
                return;
            }

            physics.Velocity = physics.Velocity with { X = patrolDirection * speed };

            Position = physics.Update(Position, Hitbox, HitboxOffset, level, gameTime);
            IsGrounded = physics.IsGrounded;
            velocity = physics.Velocity;

            SpriteEffect = patrolDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            if (velocity.X == 0)
                patrolDirection *= -1;

            Animator.Play("Walk");
            Animator.Update(gameTime);
        }
    }
}
