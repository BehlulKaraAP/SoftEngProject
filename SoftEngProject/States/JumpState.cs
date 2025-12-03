using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject.States
{
    internal class JumpState : PlayerState
    {
        public JumpState(Hero hero) : base(hero){ }

        public override void Enter()
        {
            hero.Animator.Play("Jump");
            hero.Physics.Jump();
        }

        public override void Update(GameTime gameTime)
        {
            var input = hero.InputReader.ReadInput();
            if (input.X != 0)
            {
                hero.Physics.velocity.X = input.X * 5f;

                if (input.X > 0) hero.SpriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                if (input.X < 0) hero.SpriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;

            }
            else
            {
                hero.Physics.velocity.X = 0;
            }

            if (hero.Physics.IsGrounded && hero.Physics.velocity.Y >= 0)
            {
                if (input.X == 0)
                {
                    hero.TransitionTo(new IdleState(hero));
                }
                else
                {
                    hero.TransitionTo(new RunState(hero));

                }
            }
        }
    }
}
