using Microsoft.Xna.Framework.Graphics;

namespace BeanMachine.Graphics.Animations
{
    public class Animation
    {

        public string AnimationName;

        public int AnimationRow { get; set; }

        public int CurrentFrame { get; set; }

        public int FrameCount { get; set; }

        public int FrameHeight { get; set; }

        public int FrameWidth { get; set; }

        public float FrameSpeed { get; set; }

        public bool IsLooping { get; set; }

        public Texture2D Texture { get; set; }


        public Animation(Texture2D texture, string animationName, int frameCount, int frameWidth, int frameHeight, int row)
        {
            this.Texture = texture;

            this.AnimationName = animationName;

            this.FrameCount = frameCount;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;

            this.AnimationRow = row;

            this.IsLooping = true;

            this.FrameSpeed = 0.2f;
        }
    }
}
