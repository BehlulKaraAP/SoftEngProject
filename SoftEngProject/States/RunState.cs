using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject.States
{
    internal class RunState : PlayerState
    {
        private float speed = 4f;
        public RunState(Hero hero) : base(hero) { }

        public override void Enter()
        {
            hero.Animator.Play("Run");
        }

        public override void Update(GameTime gameTime)
        {
            var input = hero.InputReader.ReadInput();

            if (hero.InputReader.AttackPressed())
            {
                hero.TransitionTo(new AttackState(hero));
                return;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                hero.TransitionTo(new JumpState(hero));
                return;
            }
            if (input.X == 0)
            {
                hero.TransitionTo(new IdleState(hero));
                return;
            }

            hero.Physics.velocity.X = input.X * speed;

            hero.SpriteEffect = input.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }
    }
}
