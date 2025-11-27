using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject.Animation
{
    internal class Animator
    {
        private Dictionary<string, SpriteAnimation> animations;
        private SpriteAnimation currentAnimation;
        private float timer;
        public int CurrentFrameIndex { get; private set; }

        public Animator()
        {
            animations = new Dictionary<string, SpriteAnimation>();
        }

        public void AddAnimation(string name, SpriteAnimation animation)
        {
            animations[name] = animation;
        }

        public void Play(string name)
        {
            if (animations.TryGetValue(name, out var anim) && currentAnimation != anim)
            {
                currentAnimation = anim;
                CurrentFrameIndex = 0;
                timer = 0;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (currentAnimation == null) return;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer > currentAnimation.FrameSpeed)
            {
                timer = 0f;
                CurrentFrameIndex++;

                if (CurrentFrameIndex >= currentAnimation.Frames.Count)
                {
                    if (currentAnimation.IsLooping)
                    {
                        CurrentFrameIndex = 0;
                    }
                    else
                    {
                        CurrentFrameIndex = currentAnimation.Frames.Count - 1;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, float scale = 1.5f)
        {
            if (currentAnimation == null) return;

            spriteBatch.Draw(currentAnimation.Texture, position, currentAnimation.Frames[CurrentFrameIndex], Color.White, 0f, Vector2.Zero, scale, spriteEffects, 0f);
        }

        public bool IsAnimationComplete()
        {
            if (currentAnimation == null || currentAnimation.IsLooping) return false;
                return CurrentFrameIndex >= currentAnimation.Frames.Count - 1;   
        }
    }
}
