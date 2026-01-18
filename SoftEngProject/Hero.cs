using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftEngProject.Animation;
using SoftEngProject.Input;
using SoftEngProject.Levels;
using SoftEngProject.States;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject
{
    internal class Hero
    {
        public Animator Animator { get; private set; }
        public Vector2 Position { get; set; }
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;
        public IInputReader InputReader { get; set; }

        private PlayerState currentState;
        public PhysicsComponent Physics { get; private set; }

        //Player stats
        public int MaxHealth { get; private set; } = 5;
        public int health { get; private set; } = 5;

        private float iVulTimer = 0f;
        private const float IVulDuration = 0.5f;

        public bool IsVulnerable => iVulTimer > 0f;

        
        //Hitbox player
        public int Width { get; } = 22;
        public int Height { get; } = 46;
        public Point HitboxOffset { get; } = new Point(60, 74);
        public Rectangle Hitbox => new Rectangle((int)Position.X + HitboxOffset.X, (int)Position.Y + HitboxOffset.Y, Width, Height);
        public Hero(IInputReader reader)
        {
            InputReader = reader;
            Animator = new Animator();

            Physics = new PhysicsComponent();
            Position = new Vector2(50, 50);
        }

        public void Start()
        {
            TransitionTo(new IdleState(this));
        }
        public void AddAnimation(string name, SpriteAnimation animation)
        {
            Animator.AddAnimation(name, animation);
        }

        public void TransitionTo(PlayerState state)
        {
            currentState = state;
            currentState.Enter();
        }

        public void TakeDamage(int amount)
        {
            if (IsVulnerable) return;

            health -= amount;
            if (health < 0) health = 0;

            iVulTimer = IVulDuration;
        }


        public void Update(GameTime gameTime, Level level)
        {
            if (currentState == null) return;

            currentState.Update(gameTime);

            Position = Physics.Update(Position, Hitbox, HitboxOffset,level, gameTime);

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (iVulTimer > 0f) iVulTimer -= dt;


            Animator.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Animator.Draw(spriteBatch, Position, SpriteEffect);
        }
    }
}
