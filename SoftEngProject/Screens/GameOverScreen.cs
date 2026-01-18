using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject.Screens
{
    internal class GameOverScreen
    {
        private Texture2D gameOverScreen;
        public GameOverScreen(Texture2D gameOverScreen)
        {
            this.gameOverScreen = gameOverScreen;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            spriteBatch.Draw(gameOverScreen, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), Color.White);

        }
    }
}
