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

        public bool DebugIsAttackActive =>
    isAttacking && attackTimer >= AttackWindup && attackTimer <= (AttackWindup + AttackActive);

        public Rectangle DebugAttackHitbox => GetAttackHitbox();

        private int direction = -1;

        private float patrolSpeed = 1.2f;
        private float chaseSpeed = 2.2f;

        private float detectRange = 260f;
        private float loseRange = 320f;
        private float attackRange = 60f;

        private bool isChasing = false;
        private bool isAttacking = false;
        private float attackTimer = 0f;
        private const float AttackDuration = 0.45f; 
        private const float AttackWindup = 0.15f;   
        private const float AttackActive = 0.12f;  

        private float attackCooldownTimer = 0f;
        private const float AttackCooldown = 0.35f;

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

        private bool IsHeroInFrontAndClose(Hero hero)
        {
            int heroX = hero.Hitbox.Center.X;
            int myX = this.Hitbox.Center.X;

            int dx = heroX - myX;

            bool inFront = (direction == 1 && dx > 0) || (direction == -1 && dx < 0);
            bool close = Math.Abs(dx) <= attackRange;

            return inFront && close;
        }

        private Rectangle GetAttackHitbox()
        {
            int w = 40;
            int h = 30;

            int x = direction == 1 ? Hitbox.Right : Hitbox.Left - w;
            int y = Hitbox.Top + 10;

            return new Rectangle(x, y, w, h);
        }

        public override void Update(GameTime gameTime, Level level, Hero hero)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (attackCooldownTimer > 0f)
                attackCooldownTimer -= dt;

            float dxToHero = hero.Hitbox.Center.X - Hitbox.Center.X;
            if (Math.Abs(dxToHero) < loseRange) 
                direction = dxToHero < 0 ? -1 : 1;

            //ATTACK STATE
            if (isAttacking)
            {
                attackTimer += dt;

                physics.Velocity = physics.Velocity with { X = 0 };

                Position = physics.Update(Position, Hitbox, HitboxOffset, level, gameTime);
                IsGrounded = physics.IsGrounded;
                velocity = physics.Velocity;

                SpriteEffect = direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                Animator.Play("Attack");
                Animator.Update(gameTime);

                // damage window
                bool active =
                    attackTimer >= AttackWindup &&
                    attackTimer <= (AttackWindup + AttackActive);

                if (active)
                {
                    Rectangle atk = GetAttackHitbox();
                    if (atk.Intersects(hero.Hitbox))
                    {
                        hero.TakeDamage(1);
                    }
                }

                // end attack
                if (attackTimer >= AttackDuration)
                {
                    isAttacking = false;
                    attackTimer = 0f;
                    attackCooldownTimer = AttackCooldown;
                }

                return;
            }

            //CHASE / PATROL DECISION
            float absDx = System.Math.Abs(dxToHero);

            if (!isChasing)
            {
                if (absDx <= detectRange) isChasing = true;
            }
            else
            {
                if (absDx >= loseRange) isChasing = false;
            }

            if (isChasing && attackCooldownTimer <= 0f && IsHeroInFrontAndClose(hero))
            {
                isAttacking = true;
                attackTimer = 0f;
                Animator.Play("Attack");
                return;
            }

            //MOVE 
            float desiredXVel;
            if (isChasing)
                desiredXVel = direction * chaseSpeed;
            else
                desiredXVel = direction * patrolSpeed;

            physics.Velocity = physics.Velocity with { X = desiredXVel };

            Position = physics.Update(Position, Hitbox, HitboxOffset, level, gameTime);
            IsGrounded = physics.IsGrounded;
            velocity = physics.Velocity;

            if (!isChasing && velocity.X == 0)
                direction *= -1;

            SpriteEffect = direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Animator.Play(Math.Abs(desiredXVel) > 0 ? "Run" : "Idle");
            Animator.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Animator.Draw(spriteBatch, Position, SpriteEffect, Scale = 1f);
        }
    }
}
