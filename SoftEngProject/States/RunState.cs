using Microsoft.Xna.Framework;
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
        public RunState(Hero hero) : base(hero) { }

        public override void Enter()
        {
            hero.Animator.Play("Run");
        }

        public override void Update(GameTime gameTime)
        {
            var input = hero.InputReader.ReadInput();

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                hero.TransitionTo(new AttackState(hero));
                return;
            }
            if (input.X == 0)
            {
                hero.TransitionTo(new IdleState(hero));
                return;
            }

            hero.Position += input * 4;

            if (input.X > 0) hero.SpriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
            if (input.X < 0) hero.SpriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
        }
    }
}
