using Microsoft.Xna.Framework.Graphics;
using SoftEngProject.Animation;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject.Screens
{
    internal class StartScreen 
    {
        Texture2D startScreenTexture;
        Animatie animatie;
        private Vector2 positie;

        public StartScreen(Texture2D texture)
        {
            startScreenTexture = texture;
            animatie = new Animatie();
            positie = new Vector2(0, 0);

            animatie.AddFrames(new AnimationFrame(new Rectangle(0,0,800,480)));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(startScreenTexture, positie, animatie.CurrentFrame.SourceRectangle, Color.White);
        }

        
    }
}
