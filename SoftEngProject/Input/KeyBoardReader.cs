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
        public Vector2 ReadInput()
        {
            var direction = Vector2.Zero;
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Left))
                direction = new Vector2(-1, 0);
            if (state.IsKeyDown(Keys.Right))
                direction = new Vector2(1, 0);
            if (state.IsKeyDown(Keys.Down))
                direction = new Vector2(0, 1);
            if (state.IsKeyDown(Keys.Up))
                direction = new Vector2(0, -1);
            return direction;
        }

        

        
    }
}
