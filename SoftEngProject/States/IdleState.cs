using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SoftEngProject.Animation;

namespace SoftEngProject.States
{
    internal class IdleState : PlayerState
    {
        public IdleState(Hero hero) : base(hero) { }

        public override void Enter()
        {
            hero.Animator.Play("Idle");
        }

        public override void Update(GameTime gameTime)
        {
            var input = hero.InputReader.ReadInput();

            //if (Keyboard.GetState().IsKeyDown(Keys.Space))
            //{
            //    hero.TransitionTo(new AttackState(hero));
            //}
            //else if (input.X != 0)
            //{
            //    hero.TransitionTo(new RunState(hero));
            //}

            if (hero.InputReader.AttackPressed())
            {
                hero.TransitionTo(new AttackState(hero));
            }
            else if (input.X != 0)
            {
                hero.TransitionTo(new RunState(hero));
            }
        }
    }
}
