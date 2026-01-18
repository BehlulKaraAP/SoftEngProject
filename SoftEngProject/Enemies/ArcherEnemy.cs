using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using SoftEngProject.Animation;
using SoftEngProject.Levels;
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

        private float speed = 1.2f;
        private int direction = -1;

        public ArcherEnemy(ContentManager content, Vector2 spawn) : base(spawn)
        {
            Texture2D idleTex = content.Load<Texture2D>("ArcherIdle");
            Texture2D walkTex = content.Load<Texture2D>("ArcherWalk");
            Texture2D shotTex = content.Load<Texture2D>("ArcherShot");

            var idleFrames = AnimationHelpers.BuildHorizontalFramesFromSheetWidth(frameCount: 9, sheetWidth: 1152, frameHeight: 128);
            var walkFrames = AnimationHelpers.BuildHorizontalFramesFromSheetWidth(frameCount: 8, sheetWidth: 1024, frameHeight: 128);
            var shotFrames = AnimationHelpers.BuildHorizontalFramesFromSheetWidth(frameCount: 14, sheetWidth: 1792, frameHeight: 128);

            Animator.AddAnimation("Idle", new SpriteAnimation(idleTex, idleFrames, frameSpeed: 0.12f, isLooping: true));
            Animator.AddAnimation("Walk", new SpriteAnimation(walkTex, walkFrames, frameSpeed: 0.10f, isLooping: true));
            Animator.AddAnimation("Shot", new SpriteAnimation(shotTex, shotFrames, frameSpeed: 0.06f, isLooping: false));

            Animator.Play("Idle");

            Width = 22;
            Height = 46;
            HitboxOffset = new Point(60, 74);

            MaxHealth = 2;
            Health = 2;
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

            Animator.Play("Walk");
            Animator.Update(gameTime);
        }
    }
}
