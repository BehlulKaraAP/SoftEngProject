using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SoftEngProject.Animation;
using SoftEngProject.Input;
using SoftEngProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject
{
    internal class HeroFactory : IHeroFactory
    {
        private ContentManager content;
        private const int frameHeight = 96;
        public HeroFactory(ContentManager content)
        {
            this.content = content;
        }
        public Hero CreateHero(IInputReader inputReader)
        {
            var hero = new Hero(inputReader);

            Texture2D idleTexture = this.content.Load<Texture2D>("Samurai_IDLE");
            Texture2D runTexture = this.content.Load<Texture2D>("Samurai_RUN");
            Texture2D attackTexture = this.content.Load<Texture2D>("Samurai_ATTACK 1");
            Texture2D jumpTexture = this.content.Load<Texture2D>("Jump");

            hero.AddAnimation("Idle", CreateAnimation(idleTexture, 10, frameHeight, 0.1f));
            hero.AddAnimation("Run", CreateAnimation(runTexture, 16, frameHeight, 0.06f));
            hero.AddAnimation("Attack", CreateAnimation(attackTexture, 7, frameHeight, 0.08f, false));
            hero.AddAnimation("Jump", CreateAnimation(jumpTexture, 8, frameHeight, 0.08f, false));


            hero.Start();

            return hero;

        }

        private SpriteAnimation CreateAnimation(Texture2D texture, int frameCount, int frameHeight, float frameSpeed, bool isLooping = true)
        {
            int frameWidth = texture.Width / frameCount;

            var frames = new List<Rectangle>();
            for (int i = 0; i < frameCount; i++)
            {
                frames.Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
            }

            return new SpriteAnimation(texture, frames, frameSpeed, isLooping);
        }
    }
}
