using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject.Animation
{
    internal static class AnimationHelpers
    {
        public static List<Rectangle> BuildHorizontalFrames(int frameCount, int frameWidth, int frameHeight)
        {
            var frames = new List<Rectangle>(frameCount);
            for (int i = 0; i < frameCount; i++)
            {
                frames.Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
            }
            return frames;
        }

        public static List<Rectangle> BuildHorizontalFramesFromSheetWidth(int frameCount, int sheetWidth, int frameHeight)
        {
            int frameWidth = sheetWidth / frameCount;
            return BuildHorizontalFrames(frameCount, frameWidth, frameHeight);
        }
    }
}
