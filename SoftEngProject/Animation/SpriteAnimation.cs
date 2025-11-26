using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject.Animation
{
    public class SpriteAnimation
    {
        public Texture2D Texture { get; private set; }
        public List<Rectangle> Frames { get; private set; }
        public float FrameSpeed { get; private set; }
        public bool IsLooping { get; private set; }

        public SpriteAnimation(Texture2D texture, List<Rectangle> frames, float frameSpeed = 0.1f, bool isLooping = true)
        {
            this.Texture = texture;
            this.Frames = frames;
            this.FrameSpeed = frameSpeed;
            this.IsLooping = isLooping;
        }
    }
}