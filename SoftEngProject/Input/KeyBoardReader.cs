using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject.Input
{
    internal class KeyBoardReader : IInputReader
    {
        private KeyboardState previous;
        public Vector2 ReadInput()
        {
            var direction = Vector2.Zero;
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Left))
                direction = new Vector2(-1, 0);
            if (state.IsKeyDown(Keys.Right))
                direction = new Vector2(1, 0);

            return direction;
        }

        public bool AttackPressed()
        {
            return Keyboard.GetState().IsKeyDown(Keys.E);
        }
        public bool JumpJustPressed()
        {
            var state = Keyboard.GetState();

            bool downNow = state.IsKeyDown(Keys.Space) || state.IsKeyDown(Keys.Up);
            bool downBefore = previous.IsKeyDown(Keys.Space) || previous.IsKeyDown(Keys.Up);

            previous = state;

            return downNow && !downBefore;
        }
    }
}
