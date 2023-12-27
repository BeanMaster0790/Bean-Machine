using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BeanMachine.Graphics.Animations
{
    public class AnimationManager
    {
        private Sprite _parent;

        public Animation CurrentAnimation;

        private float _timer;

        private bool _isPlaying;

        public EventHandler<FrameEventArgs> FrameEvent;

        public AnimationManager(Sprite parent, Animation animation)
        {
            this._parent = parent;
            
            this.CurrentAnimation = animation;

            this._isPlaying = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(this.CurrentAnimation.AnimationRow == 0)
            {
                spriteBatch.Draw(this.CurrentAnimation.Texture, this._parent.Position,
                new Rectangle(this.CurrentAnimation.CurrentFrame * this.CurrentAnimation.FrameWidth, 0, this.CurrentAnimation.FrameWidth, this.CurrentAnimation.FrameHeight),
                Color.White, this._parent.Rotation, this._parent.Origin, 1, this._parent.Flipped, 0);
            }
            else
            {
                spriteBatch.Draw(this.CurrentAnimation.Texture, this._parent.Position,
                new Rectangle(this.CurrentAnimation.CurrentFrame * this.CurrentAnimation.FrameWidth, this.CurrentAnimation.AnimationRow * this.CurrentAnimation.FrameHeight, this.CurrentAnimation.FrameWidth, this.CurrentAnimation.FrameHeight),
                Color.White, this._parent.Rotation, this._parent.Origin, 1, this._parent.Flipped, 0);
            }
        }

        public void Play(Animation animation)
        {
            if (this.CurrentAnimation == animation)
                return;

            this._isPlaying = true;

            this.CurrentAnimation = animation;
            this.CurrentAnimation.CurrentFrame = 0;

            this._timer = 0;
        }

        public void Stop()
        {
            this._isPlaying = false;

            this.CurrentAnimation.CurrentFrame = 0;
            this._timer = 0;
        }

        public void Pause()
        {
            this._isPlaying = false;
        }

        public void Resume()
        {
            this._isPlaying = true;
        }

        public void Update(GameTime gameTime)
        {

            if(!this._isPlaying)
                return;

            this._timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(this._timer >= CurrentAnimation.FrameSpeed)
            {
                this._timer = 0;

                this.CurrentAnimation.CurrentFrame++;

                FrameEventArgs args = new FrameEventArgs();
                args.AnimationName = this.CurrentAnimation.AnimationName;
                args.CurrentFrame = this.CurrentAnimation.CurrentFrame;

                FrameEvent?.Invoke(this, args);

                if(this.CurrentAnimation.CurrentFrame >= this.CurrentAnimation.FrameCount && this.CurrentAnimation.IsLooping)
                {
                    this.CurrentAnimation.CurrentFrame = 0;
                }
                else if (this.CurrentAnimation.CurrentFrame > this.CurrentAnimation.FrameCount)
                {
                    Stop();
                }
            }
        }

    }

    public class FrameEventArgs : EventArgs
    {
        public string AnimationName { get; set; }
        public int CurrentFrame;
    }
}
