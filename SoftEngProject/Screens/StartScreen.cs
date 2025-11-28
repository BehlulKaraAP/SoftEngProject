using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject.Screens
{
    internal class StartScreen
    {
        private Texture2D startScreen;
        public bool StartPressed { get; private set; }

        public StartScreen(Texture2D startScreen)
        {
            this.startScreen = startScreen;
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                StartPressed = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            spriteBatch.Draw(startScreen, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), Color.White);

        }
    }
}
