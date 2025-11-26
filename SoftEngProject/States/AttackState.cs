using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject.States
{
    internal class AttackState : PlayerState
    {
        public AttackState(Hero hero) : base(hero) { }

        public override void Enter()
        {
            hero.Animator.Play("Attack");
        }

        public override void Update(GameTime gameTime)
        {
            if (hero.Animator.IsAnimationComplete())
            {
                hero.TransitionTo(new IdleState(hero));
            }
        }
    }
}
