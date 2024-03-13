using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace BeanMachine.Graphics.Animations
{
    public class AnimationManager : Addon
    {
        public Dictionary<string, Animation> Animations;

        public Animation CurrentAnimation;

        public EventHandler<FrameEventArgs> FrameEvent;

        private bool _isPlaying;

        private float _timer;

        public AnimationManager(Sprite parent, Dictionary<string, Animation> animations)
        {   
            this.Animations = animations;

            this._isPlaying = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(this.CurrentAnimation.AnimationRow == 0)
            {
                spriteBatch.Draw(this.CurrentAnimation.Texture, base.Parent.Position,
                new Rectangle(this.CurrentAnimation.CurrentFrame * this.CurrentAnimation.FrameWidth, 0, this.CurrentAnimation.FrameWidth, this.CurrentAnimation.FrameHeight),
                Parent.Colour, MathHelper.ToRadians(base.Parent.Rotation), base.Parent.Origin, 1, base.Parent.Flipped, 0);
            }
            else
            {
                spriteBatch.Draw(this.CurrentAnimation.Texture, base.Parent.Position,
                new Rectangle(this.CurrentAnimation.CurrentFrame * this.CurrentAnimation.FrameWidth, this.CurrentAnimation.AnimationRow * this.CurrentAnimation.FrameHeight, this.CurrentAnimation.FrameWidth, this.CurrentAnimation.FrameHeight),
                Parent.Colour, MathHelper.ToRadians(base.Parent.Rotation), base.Parent.Origin, 1, base.Parent.Flipped, 0);
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

        public void Pause()
        {
            this._isPlaying = false;
        }

        public void Resume()
        {
            this._isPlaying = true;
        }

        public void Stop()
        {
            this._isPlaying = false;

            this.CurrentAnimation.CurrentFrame = 0;
            this._timer = 0;
        }

        public override void Update()
        {

            if(!this._isPlaying)
                return;

            this._timer += Time.Instance.DeltaTime;

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
        public string AnimationName;

        public int CurrentFrame;
    }
}
