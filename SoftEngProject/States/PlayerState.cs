using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject.States
{
    internal abstract class PlayerState
    {
        protected Hero hero;

        protected PlayerState(Hero hero)
        {
            this.hero = hero;
        }

        public abstract void Enter();
        public abstract void Update(GameTime gameTime);
    }
}
