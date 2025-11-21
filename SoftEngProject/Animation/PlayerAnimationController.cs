using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject.Animation
{
    public enum PlayerState { Idle,Run,Attack }
    internal class PlayerAnimationController
    {
        private readonly Dictionary<PlayerState, Animatie> animations =  new Dictionary<PlayerState, Animatie>();
        private readonly Dictionary<PlayerState, Texture2D> textures = new Dictionary<PlayerState, Texture2D>();
        public Animatie CurrentAnimation { get; private set; }
        public Texture2D CurrentTexture { get; private set; }
        public SpriteEffects SpriteEffect { get; private set; } = SpriteEffects.None;

        public void Register(PlayerState state, Animatie anim, Texture2D texture)
        {
            animations[state] = anim;
            textures[state] = texture;

            if (CurrentAnimation == null)
            {
                CurrentAnimation = anim;
                CurrentTexture = texture;
            }
        }

        public void Update(PlayerState state, Vector2 direction, GameTime gameTime)
        {
            //CurrentAnimation = animations[state];

            //SpriteEffect = direction.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            if (animations.ContainsKey(state))
            {
                if (CurrentAnimation != animations[state])
                {
                    CurrentAnimation = animations[state];
                    CurrentTexture = textures[state];
                }
            }

            if (direction.X < 0) SpriteEffect = SpriteEffects.FlipHorizontally;
            else if (direction.X > 0) SpriteEffect = SpriteEffects.None;

                CurrentAnimation.Update(gameTime);
        }
    }
}
