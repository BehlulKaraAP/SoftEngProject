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

        //Hitbox player
        public int Width { get; } = 48;
        public int Height { get; } = 80;
        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

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
        
        public void Update(GameTime gameTime, Level level)
        {
            if (currentState == null) return;

            currentState.Update(gameTime);

            Position = Physics.Update(Position, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), level, gameTime);

            Animator.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Animator.Draw(spriteBatch, Position, SpriteEffect);
        }
    }
}
